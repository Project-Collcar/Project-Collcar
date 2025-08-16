using UnityEngine;
using System.Collections;

public class Speeder : Car
{
    [Header("Speeder 고유 설정")]
    public float boostForce = 10f;   // 부스터가 가하는 힘
    public float boostDuration = 1.2f; // 부스터 지속 시간

    private bool isBoosting = false; // 현재 부스터 사용 중인지 확인하는 변수

    // Speeder의 고유 능력치를 설정합니다.
    protected override void Awake()
    {
        base.Awake(); // 부모의 초기화(Rigidbody 찾기)를 먼저 실행
        
        motorTorque = 6000f;
        steerAngle = 40f;
        brakeTorque = 4500f;
        gravityMultiplier = 0.9f;
    }
    
    // 물리 효과는 FixedUpdateNetwork에서 처리합니다.
    public override void FixedUpdateNetwork()
    {
        // 1. 부모의 기본 운전 로직(핸들링, 엑셀 등)을 먼저 실행합니다.
        base.FixedUpdateNetwork();

        // 2. 만약 부스팅 상태라면, 추가로 로켓 힘을 가합니다.
        if (isBoosting)
        {
            // WheelCollider 토크와 별개로, Rigidbody에 직접 힘을 가합니다.
            rigidBody.AddForce(transform.forward * boostForce, ForceMode.Acceleration);
        }
    }
    
    // 부모의 UseSkill1을 덮어써서 부스터 코루틴을 시작시킵니다.
    public override void UseSkill1()
    {
        StartCoroutine(BoostCoroutine());
    }

    // 부스터의 지속 시간을 관리하는 코루틴
    private IEnumerator BoostCoroutine()
    {
        Debug.Log("Speeder 스킬: 부스터 발동!");
        isBoosting = true;

        // boostDuration(2초) 만큼 기다립니다.
        yield return new WaitForSeconds(boostDuration);

        isBoosting = false;
        Debug.Log("부스터 종료!");
    }
}