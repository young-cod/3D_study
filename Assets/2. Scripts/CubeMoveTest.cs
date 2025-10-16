using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CubeMoveTest : MonoBehaviour
{
    // 인스펙터 창에서 설정할 수 있는 변수
    [Header("이동 및 회전 설정")]
    [Tooltip("큐브의 이동 속도")]
    public float moveSpeed = 5.0f;
    [Tooltip("회전 애니메이션 속도")]
    public float rotationSpeed = 10.0f;

    private Rigidbody rb;
    private Quaternion targetRotation; // 목표 회전값 저장

    void Start()
    {
        // Rigidbody 컴포넌트 가져오기
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody 컴포넌트가 필요합니다!");
            enabled = false; // Rigidbody가 없으면 스크립트 비활성화
            return;
        }

        // 초기 목표 회전값을 현재 회전값으로 설정
        targetRotation = transform.rotation;
    }

    void Update()
    {
        // 1. 키 입력 감지 및 목표 회전값 계산
        Vector3 moveDirection = Vector3.zero;

        if (Input.GetKeyDown(KeyCode.W))
        {
            moveDirection = Vector3.forward;
            // 현재 회전에서 전방(forward)을 향하는 회전 추가
            targetRotation *= Quaternion.Euler(90, 0, 0); // X축 기준 90도 회전
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            moveDirection = Vector3.back;
            targetRotation *= Quaternion.Euler(-90, 0, 0); // X축 기준 -90도 회전
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            moveDirection = Vector3.left;
            targetRotation *= Quaternion.Euler(0, 0, 90); // Z축 기준 90도 회전
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            moveDirection = Vector3.right;
            targetRotation *= Quaternion.Euler(0, 0, -90); // Z축 기준 -90도 회전
        }

        // 키 입력이 있다면 Rigidbody에 힘을 가해 이동
        if (moveDirection != Vector3.zero)
        {
            // 월드 좌표 대신 현재 오브젝트의 로컬 좌표 기준으로 이동 방향 적용
            rb.velocity = transform.TransformDirection(moveDirection) * moveSpeed;
        }
    }

    void FixedUpdate()
    {
        // 2. 목표 회전값으로 부드럽게 회전
        // 현재 회전(transform.rotation)을 목표 회전(targetRotation)으로 rotationSpeed 만큼 보간
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime * 100);

        // 3. 목표 회전에 도달하면 큐브 멈춤
        // 현재 회전과 목표 회전의 각도 차이가 매우 작다면 (거의 회전이 끝났다면)
        if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
        {
            // 회전이 끝난 후에는 Rigidbody의 속도를 0으로 만들어 큐브를 멈춤
            rb.velocity = Vector3.zero;
        }
    }
}
