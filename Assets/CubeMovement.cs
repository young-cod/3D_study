using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMovement : MonoBehaviour
{
    [Header("�̵� ���� ����")]
    public float cubeSize = 1f;            // ť�� �� ���� ����
    public float moveDuration = 0.2f;      // �� ĭ �̵� �ð�
    public LayerMask obstacleMask;         // �̵� �Ұ��� ���� (�� ��)
    public LayerMask groundMask;           // �ٴ� ������ ���̾�
    public bool useKeyboard = true;

    [Header("�Է� Ű ����")]
    public KeyCode upKey = KeyCode.W;
    public KeyCode downKey = KeyCode.S;
    public KeyCode leftKey = KeyCode.A;
    public KeyCode rightKey = KeyCode.D;

    private bool isMoving = false;

    void Start()
    {
        // ���� �� ��ġ�� ���ڿ� ����
        Vector3 p = transform.position;
        transform.position = new Vector3(
            Mathf.Round(p.x / cubeSize) * cubeSize,
            p.y,
            Mathf.Round(p.z / cubeSize) * cubeSize
        );
    }

    void Update()
    {
        if (isMoving) return;

        Vector3 dir = Vector3.zero;

        if (useKeyboard)
        {
            if (Input.GetKeyDown(upKey) || Input.GetKeyDown(KeyCode.UpArrow)) dir = Vector3.forward;
            else if (Input.GetKeyDown(downKey) || Input.GetKeyDown(KeyCode.DownArrow)) dir = Vector3.back;
            else if (Input.GetKeyDown(leftKey) || Input.GetKeyDown(KeyCode.LeftArrow)) dir = Vector3.left;
            else if (Input.GetKeyDown(rightKey) || Input.GetKeyDown(KeyCode.RightArrow)) dir = Vector3.right;
        }

        if (dir != Vector3.zero)
        {
            TryMove(dir);
        }
    }

    void TryMove(Vector3 dir)
    {
        //Vector3 origin = transform.position + Vector3.up * (cubeSize / 2f);
        //float distance = cubeSize;

        //if (Physics.Raycast(origin, dir, distance, obstacleMask))
        //{
        //    Debug.Log("�̵� �Ұ�: �տ� ��ֹ��� �ֽ��ϴ�!");
        //    return;
        //}

        //Vector3 nextPos = transform.position + dir * cubeSize;
        //Vector3 groundCheckOrigin = nextPos + Vector3.up * 0.1f;
        //if (!Physics.Raycast(groundCheckOrigin, Vector3.down, 1f, groundMask))
        //{
        //    Debug.Log("�̵� �Ұ�: ���� ĭ �Ʒ��� �ٴ��� �����ϴ�!");
        //    return;
        //}

        StartCoroutine(Roll(dir));
    }

    IEnumerator Roll(Vector3 dir)
    {
        isMoving = true;

        // ȸ�� �� ���
        Vector3 pivot = transform.position + (dir * (cubeSize / 2f)) + Vector3.down * (cubeSize / 2f);
        Vector3 axis = Vector3.Cross(Vector3.up, dir);

        float elapsed = 0f;
        float angle = 0f;
        float targetAngle = 90f;

        while (elapsed < moveDuration)
        {
            float delta = Time.deltaTime;
            elapsed += delta;
            float t = Mathf.Min(elapsed / moveDuration, 1f);
            float step = Mathf.Lerp(0f, targetAngle, t) - angle;
            transform.RotateAround(pivot, axis, step);
            angle += step;
            yield return null;
        }

        // ��ġ/ȸ�� ����
        //Vector3 finalPos = transform.position;
        //finalPos.x = Mathf.Round(finalPos.x / cubeSize) * cubeSize;
        //finalPos.z = Mathf.Round(finalPos.z / cubeSize) * cubeSize;
        //transform.position = finalPos;

        //Vector3 euler = transform.eulerAngles;
        //euler.x = Mathf.Round(euler.x / 90f) * 90f;
        //euler.y = Mathf.Round(euler.y / 90f) * 90f;
        //euler.z = Mathf.Round(euler.z / 90f) * 90f;
        //transform.eulerAngles = euler;

        isMoving = false;
    }

    void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying) return;

        Gizmos.color = Color.red;
        Vector3 origin = transform.position + Vector3.up * (cubeSize / 2f);
        Gizmos.DrawLine(origin, origin + transform.forward * cubeSize);

        Gizmos.color = Color.yellow;
        Vector3 nextPos = transform.position + transform.forward * cubeSize;
        Gizmos.DrawLine(nextPos + Vector3.up * 0.1f, nextPos + Vector3.down * 1f);
    }
}





