using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(Rigidbody))]
public class Missile : MonoBehaviour
{
    List<GameObject> targets = new List<GameObject>();
    private Rigidbody rb;

    [Header("ù �̻��� ����")]
    [SerializeField] private float forcePower = 15f;
    [SerializeField] private float divideTimer = 1.5f;
    [SerializeField] private float searchRadius = 1000f;

    [Header("�п� �̻��� ����")]
    public GameObject divideMissiePrefab;
    [SerializeField] private int maxCount = 10;

    private void Awake()
    {
        TryGetComponent(out rb);
    }

    private void OnEnable()
    {
        ObjectManager.Instance.OnSpawnedObject += SpawnObj;
        ObjectManager.Instance.OnReturnedObject += ReturnObj;
        rb.AddForce(transform.forward * forcePower, ForceMode.Impulse);
        StartCoroutine(SearchTarget(divideTimer));
    }

    private void OnDisable()
    {
        ObjectManager.Instance.OnSpawnedObject -= SpawnObj;
        ObjectManager.Instance.OnReturnedObject -= ReturnObj;
    }

    void ReturnObj(GameObject go, ObjectManager.OBJECT type){
        Debug.Log($"{go.name}�� ��ȯ�Ǿ����ϴ�");
    }
    void SpawnObj(GameObject go, ObjectManager.OBJECT type)
    {
        Debug.Log($"{type}�� {go.name}�� �����Ǿ����ϴ�.");
    }

    IEnumerator SearchTarget(float timer)
    {
        yield return new WaitForSeconds(timer);

        SearchTarget();
    }

    public void DivideMissile(int count)
    {
        if (targets.Count == 0)
        {
            Debug.LogWarning("�п��� Ÿ���� �����ϴ�. �̻��� �п� ����.");
            return;
        }

        ObjectManager.Instance.ReturnObject(gameObject);

        float angleUnit = 360 / count;

        int maxCount = Mathf.Min(count, targets.Count);
        //�ش� ����ŭ �п��Ұ��ΰ� �ƴϸ� �ִ밳���� ���� �п��Ұ��ΰ�
        //ȣ���Ҷ��� �ִ�п������� �ޱ���
        for (int i = 0; i < maxCount; i++)
        {
            GameObject currentTarget = targets[i];
            float angle = angleUnit * i;
            Quaternion rotation = Quaternion.Euler(0, angle, 0);

            GameObject missileGO = Instantiate(divideMissiePrefab, transform.position, rotation);
            missileGO.GetComponent<Bullet>()?.SelectTargetMove(currentTarget);
        }
    }

    public void SearchTarget()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, searchRadius);

        if (cols.Count() > 0)
        {
            foreach (Collider col in cols)
            {
                if (col.CompareTag("Enemy"))
                {
                    targets.Add(col.gameObject);
                }
            }

            DivideMissile(maxCount);
            gameObject.SetActive(false);

        }
    }
}
