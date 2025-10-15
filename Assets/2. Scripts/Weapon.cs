using System.Collections;
using System.Collections.Generic;
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
        bullet.SetActive(true);

        return bullet;
    }

    public void Reload(GameObject bullet)
    {
        bullet.SetActive(false);
        bulletPool.Enqueue(bullet);
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
}
