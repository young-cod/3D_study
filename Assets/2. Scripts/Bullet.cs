using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Transactions;
using Unity.EditorCoroutines.Editor;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    public float rotateSpeed = 5f;
    public float searchRadius = 10f;
    [SerializeField] private float forcePower = 15f;
    private float speed = 45f;
    private bool isTraking = false;

    private Weapon pool; 

    private Coroutine destroyRoutine;
    private Rigidbody rb;
    [SerializeField]   private GameObject currentTarget;

    public void SetPool(Weapon weapon)
    {
        pool = weapon;
    }

    private void Awake()
    {
        TryGetComponent(out rb);
    }

    private void OnEnable()
    {   
        //todo : pool�� �����ϱ�

        //transform.parent.TryGetComponent(out pool);
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        Propulsion(forcePower);
    }
    IEnumerator DestroyRoutine(){
        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }

    private void OnDisable()
    {
        if (destroyRoutine != null)
        {
            StopCoroutine(destroyRoutine);
            destroyRoutine = null;
        }
    }

    private void FixedUpdate()
    {
        if(isTraking)Move();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy")){
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        // Ÿ���� ��ȿ�ϸ� ���� ���� ����
        if (currentTarget != null)
        {
            CheckEnemy(); // ȸ�� ���� (Slerp)
        }
        else
        {
            isTraking = true;
        }
    }


    void CheckEnemy(){
        if (currentTarget != null)
        {
            Vector3 targetDir = currentTarget.transform.position - transform.position;
            Quaternion targetRotate = Quaternion.LookRotation(targetDir);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotate,
                Time.deltaTime * rotateSpeed
            );
        }
    }

    void Propulsion(float power){
        rb.AddForce(transform.forward * forcePower, ForceMode.Impulse);
    }

    void Move(){
        rb.velocity = speed  * transform.forward;
    }

    public void SelectTargetMove(GameObject target){
        currentTarget = target;

        Vector3 dir = currentTarget.transform.position - transform.position;
        //transform.rotation = Quaternion.LookRotation(dir);

        isTraking = true;

        destroyRoutine = StartCoroutine(DestroyRoutine());
    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawWireSphere(transform.position, radius);
    }
}
