using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Transactions;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public float rotateSpeed = 10f;
    public float radius = 10f;

    private Weapon pool; 

    private Coroutine destroyRoutine;

    public void SetPool(Weapon weapon)
    {
        pool = weapon;
    }

    private void OnEnable()
    {
        //transform.parent.TryGetComponent(out pool);
        destroyRoutine = StartCoroutine(AutoDestroy(5f));
    }

    private void OnDisable()
    {
        if (destroyRoutine != null)
        {
            StopCoroutine(destroyRoutine);
            destroyRoutine = null;
        }
    }

    IEnumerator AutoDestroy(float timer)
    {
        yield return new WaitForSeconds(timer);

        // 풀로 반환
        if (pool != null)
        {
            pool.Reload(gameObject);
        }
        else
        {
            Debug.LogWarning("Weapon pool 참조가 없음!");
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        CheckEnemy();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy")){
            if (destroyRoutine != null)
            {
                StopCoroutine(destroyRoutine);
                destroyRoutine = null;
                pool.Reload(gameObject);
            }
        }
    }

    void CheckEnemy(){
        Collider[] cols = Physics.OverlapSphere(transform.position, radius);

        foreach(Collider col in cols){
            if(col.CompareTag("Enemy")){
                Vector3 targetDir = col.transform.position - transform.position;
                Quaternion targetRotate = Quaternion.LookRotation(targetDir);

                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    targetRotate,
                    Time.deltaTime * rotateSpeed
                );

            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
