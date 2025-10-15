using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Experimental.AI;
using UnityEngine.Rendering;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(Weapon))]
public class Player : MonoBehaviour
{
    public float rotateSpeed;
    public GameObject target;
    Weapon weapon;
    // Start is called before the first frame update
    void Start()
    {
        TryGetComponent(out weapon);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)){
            weapon.Fire();
        }
    }

    void LookEnemy(){
        Vector3 targetDir = target.transform.position - transform.position;
        Quaternion targetRotate = Quaternion.LookRotation(targetDir);

        transform.rotation = Quaternion.Slerp(
            transform.rotation, // 본인 회전각
            targetRotate, //바라볼 회전각
            Time.deltaTime * rotateSpeed
        );
    }
}
