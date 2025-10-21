using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMovement : MonoBehaviour
{
    [Header("이동 관련 설정")]
    public float cubeSize = 1f;            // 큐브 한 변의 길이
    public float moveDuration = 0.2f;      // 한 칸 이동 시간
    public LayerMask obstacleMask;         // 이동 불가능 영역 (벽 등)
    public LayerMask groundMask;           // 바닥 감지용 레이어
    public bool useKeyboard = true;

    [Header("입력 키 설정")]
    public KeyCode upKey = KeyCode.W;
    public KeyCode downKey = KeyCode.S;
    public KeyCode leftKey = KeyCode.A;
    public KeyCode rightKey = KeyCode.D;

    private bool isMoving = false;

    void Start()
    {
        // 시작 시 위치를 격자에 정렬
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
        //    Debug.Log("이동 불가: 앞에 장애물이 있습니다!");
        //    return;
        //}

        //Vector3 nextPos = transform.position + dir * cubeSize;
        //Vector3 groundCheckOrigin = nextPos + Vector3.up * 0.1f;
        //if (!Physics.Raycast(groundCheckOrigin, Vector3.down, 1f, groundMask))
        //{
        //    Debug.Log("이동 불가: 다음 칸 아래에 바닥이 없습니다!");
        //    return;
        //}

        StartCoroutine(Roll(dir));
    }

    IEnumerator Roll(Vector3 dir)
    {
        isMoving = true;

        // 회전 축 계산
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

        // 위치/회전 보정
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





