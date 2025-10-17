using UnityEngine;

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
        //rb.velocity = new Vector3((currentDir*speed).x, rb.velocity.y, 0);
    }


    // Update is called once per frame
    void Update()
    {
        currentDir = GetInputDirection();
        if (currentDir == Vector3.zero) return;

        transform.Translate(currentDir * speed * Time.deltaTime);
        //LookDirection(currentDir);
    }

    Vector3 GetInputDirection()
    {
        Vector3 inputDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, 0);

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
