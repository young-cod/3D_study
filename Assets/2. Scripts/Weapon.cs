using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("źȯ ���� ����")]
    public GameObject bulletPrefab;
    public Transform firePos;

    [Header("�߻� ���� ����")]
    public int bulletCount = 10;
    public float fireDelay = 0.3f;
    private float nextFireDelay = 0f;

    [Header("���� ���� ����")]
    public Transform muzzleTrs;
    public float rotateSpeed = 30f;
    public float searchRadius = 10f;
    [SerializeField] private GameObject target;

    private Queue<GameObject> bulletPool = new Queue<GameObject>();

    private void Start()
    {
        InitializePool();
    }

    private void Update()
    {
        // �߻� ���� ����
        if (Input.GetKeyDown(KeyCode.O))
        {
            fireDelay = Mathf.Max(0.05f, fireDelay - 0.1f);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            fireDelay = Mathf.Min(1.0f, fireDelay + 0.1f);
        }

        // �߻�
        if (Time.time > nextFireDelay && Input.GetKey(KeyCode.Space))
        {
            Fire();
            nextFireDelay = Time.time + fireDelay;
        }

        SearchTarget();
    }

    public GameObject Fire()
    {
        if (bulletPool.Count <= 0)
        {
            Debug.LogWarning("��� ������ źȯ�� �����ϴ�. Ǯ�� �ø�����!");
            return null;
        }

        GameObject bullet = bulletPool.Dequeue();
        bullet.transform.position = firePos.position;
        bullet.transform.rotation = firePos.rotation;
        bullet.SetActive(true);
        bullet.transform.SetParent(null);

        return bullet;
    }

    public void Reload(GameObject bullet)
    {
        bullet.SetActive(false);
        bullet.transform.SetParent(transform);
        bulletPool.Enqueue(bullet);
    }

    void LookTarget(Vector3 targetDir)
    {
        Vector3 rotateDir = (targetDir - transform.position).normalized;
        Quaternion currentDir = Quaternion.LookRotation(rotateDir) * Quaternion.Euler(90, 0, 0);

        muzzleTrs.rotation = Quaternion.Slerp(
            muzzleTrs.rotation,
            currentDir,
            Time.deltaTime * rotateSpeed
        );
    }

    void SearchTarget()
    {
        Collider selectEnemy = null;

        Collider[] enemys = Physics.OverlapSphere(transform.position, searchRadius);
        
        if (enemys.Count() > 0)
        {
            float minDistance = int.MaxValue;

            foreach (Collider enemy in enemys)
            {
                if (enemy.CompareTag("Enemy"))
                {
                    float distance = Vector3.Distance(enemy.transform.position, transform.position);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        selectEnemy = enemy;
                        target = selectEnemy.gameObject;
                    }

                }
            }

            if(selectEnemy != null) LookTarget(selectEnemy.transform.position);
        }

    }

    private void InitializePool()
    {
        for (int i = 0; i < bulletCount; i++)
        {
            GameObject go = Instantiate(bulletPrefab, transform);
            go.SetActive(false);

            // Ǯ ������ ���� ���� (�ٽ�)
            var bullet = go.GetComponent<Bullet>();
            bullet.SetPool(this);

            bulletPool.Enqueue(go);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, searchRadius);
    }
}
