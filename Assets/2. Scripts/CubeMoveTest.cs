using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CubeMoveTest : MonoBehaviour
{
    // �ν����� â���� ������ �� �ִ� ����
    [Header("�̵� �� ȸ�� ����")]
    [Tooltip("ť���� �̵� �ӵ�")]
    public float moveSpeed = 5.0f;
    [Tooltip("ȸ�� �ִϸ��̼� �ӵ�")]
    public float rotationSpeed = 10.0f;

    private Rigidbody rb;
    private Quaternion targetRotation; // ��ǥ ȸ���� ����

    void Start()
    {
        // Rigidbody ������Ʈ ��������
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody ������Ʈ�� �ʿ��մϴ�!");
            enabled = false; // Rigidbody�� ������ ��ũ��Ʈ ��Ȱ��ȭ
            return;
        }

        // �ʱ� ��ǥ ȸ������ ���� ȸ�������� ����
        targetRotation = transform.rotation;
    }

    void Update()
    {
        // 1. Ű �Է� ���� �� ��ǥ ȸ���� ���
        Vector3 moveDirection = Vector3.zero;

        if (Input.GetKeyDown(KeyCode.W))
        {
            moveDirection = Vector3.forward;
            // ���� ȸ������ ����(forward)�� ���ϴ� ȸ�� �߰�
            targetRotation *= Quaternion.Euler(90, 0, 0); // X�� ���� 90�� ȸ��
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            moveDirection = Vector3.back;
            targetRotation *= Quaternion.Euler(-90, 0, 0); // X�� ���� -90�� ȸ��
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            moveDirection = Vector3.left;
            targetRotation *= Quaternion.Euler(0, 0, 90); // Z�� ���� 90�� ȸ��
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            moveDirection = Vector3.right;
            targetRotation *= Quaternion.Euler(0, 0, -90); // Z�� ���� -90�� ȸ��
        }

        // Ű �Է��� �ִٸ� Rigidbody�� ���� ���� �̵�
        if (moveDirection != Vector3.zero)
        {
            // ���� ��ǥ ��� ���� ������Ʈ�� ���� ��ǥ �������� �̵� ���� ����
            rb.velocity = transform.TransformDirection(moveDirection) * moveSpeed;
        }
    }

    void FixedUpdate()
    {
        // 2. ��ǥ ȸ�������� �ε巴�� ȸ��
        // ���� ȸ��(transform.rotation)�� ��ǥ ȸ��(targetRotation)���� rotationSpeed ��ŭ ����
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime * 100);

        // 3. ��ǥ ȸ���� �����ϸ� ť�� ����
        // ���� ȸ���� ��ǥ ȸ���� ���� ���̰� �ſ� �۴ٸ� (���� ȸ���� �����ٸ�)
        if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
        {
            // ȸ���� ���� �Ŀ��� Rigidbody�� �ӵ��� 0���� ����� ť�긦 ����
            rb.velocity = Vector3.zero;
        }
    }
}
