using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(Rigidbody))]
public class Missile : MonoBehaviour
{
    List<GameObject> targets = new List<GameObject>();
    private Rigidbody rb;

    [Header("첫 미사일 관련")]
    [SerializeField] private float forcePower = 15f;
    [SerializeField] private float divideTimer = 1.5f;
    [SerializeField] private float searchRadius = 1000f;

    [Header("분열 미사일 관련")]
    public GameObject divideMissiePrefab;
    [SerializeField] private int maxCount = 10;

    private void Awake()
    {
        TryGetComponent(out rb);
    }

    private void Start()
    {
        rb.AddForce(transform.forward * forcePower, ForceMode.Impulse);
        StartCoroutine(SearchTarget(divideTimer));
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
            Debug.LogWarning("분열할 타겟이 없습니다. 미사일 분열 실패.");
            return;
        }

        for (int i = 0; i < targets.Count; i++)
        {
            GameObject currentTarget = targets[i];

            GameObject missileGO = Instantiate(divideMissiePrefab, transform.position, transform.rotation);

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
