using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.AI;
using UnityEngine.Rendering;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(Weapon))]
[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    public float speed = 10f;
    public float rotateSpeed = 10f;

    private Weapon weapon;
    private Rigidbody rb;

    private Vector3 currentDir;

    // Start is called before the first frame update
    private void Awake()
    {
        TryGetComponent(out weapon);
        TryGetComponent(out rb);
    }

    private void FixedUpdate()
    {
        //rb.velocity = Time.deltaTime * speed * currentDir;
    }

    // Update is called once per frame
    void Update()
    {
        currentDir = GetInputDirection();
        if (currentDir == Vector3.zero) return;

        LookDirection(currentDir);
        transform.Translate(Time.deltaTime * speed * Vector3.forward);
    }

    Vector3 GetInputDirection()
    {
        Vector3 inputDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        return inputDir.normalized;
    }

    void LookDirection(Vector3 dir)
    {
        if (dir == Vector3.zero) return;

        Quaternion targetRotate = Quaternion.LookRotation(dir);

        transform.rotation = Quaternion.Slerp(
            transform.rotation, // 본인 회전각
            targetRotate, //바라볼 회전각
            Time.deltaTime * rotateSpeed
        );
    }
}
