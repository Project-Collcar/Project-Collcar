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
        base.Awake(); // 부모의 초기화를 먼저 실행
        
        motorTorque = 6000f;
        steerAngle = 40f;
        brakeTorque = 4500f;
        gravityMultiplier = 0.9f;
    }
    
    public override void FixedUpdateNetwork()
    {
        // 부모 클래스의 기본 주행 로직을 먼저 실행
        base.FixedUpdateNetwork();

        // 부스터 상태일 경우, 추가적인 전진 힘을 가함
        if (isBoosting)
        {
            rigidBody.AddForce(transform.forward * boostForce, ForceMode.Acceleration);
        }
    }
    
    // 부모의 UseSkill1을 덮어써서 부스터 코루틴을 시작시킵니다.
    public override void UseSkill1()
    {
        // 이미 부스터 사용 중이면 또 사용하지 않도록 방지
        if (isBoosting) return;
        StartCoroutine(BoostCoroutine());
    }

    // 부스터의 지속 시간을 관리하는 코루틴
    private IEnumerator BoostCoroutine()
    {
        Debug.Log("Speeder 스킬: 부스터 발동!");
        isBoosting = true;
        yield return new WaitForSeconds(boostDuration);
        isBoosting = false;
        Debug.Log("부스터 종료!");
    }
}