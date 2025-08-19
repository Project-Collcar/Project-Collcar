using UnityEngine;
using System.Collections;

public class Speeder : Car
{
    [Header("Speeder 고유 설정")]
    public float boostForce = 10f;   // 부스터가 가하는 힘
    public float boostDuration = 1.2f; // 부스터 지속 시간

    private bool isBoosting = false; // 부스터 사용 여부

    // Speeder의 능력치를 설정
    protected override void Awake()
    {
        base.Awake(); // 부모 초기화
        
        motorTorque = 6000f;
        steerAngle = 40f;
        brakeTorque = 4500f;
        gravityMultiplier = 0.9f;
    }
    
    public override void FixedUpdateNetwork()
    {
        // 부모 클래스 주행 로직 실행
        base.FixedUpdateNetwork();

        // 부스터 상태일 때 추가적인 전진 힘 적용
        if (isBoosting)
        {
            rigidBody.AddForce(transform.forward * boostForce, ForceMode.Acceleration);
        }
    }
    
    // 부스터 코루틴 시작
    public override void UseSkill1()
    {
        // 이미 부스터 사용 중이면 종료
        if (isBoosting) return;
        StartCoroutine(BoostCoroutine());
    }

    // 부스터 지속 시간 관리
    private IEnumerator BoostCoroutine()
    {
        Debug.Log("Speeder 스킬: 부스터 발동!");
        isBoosting = true;
        yield return new WaitForSeconds(boostDuration);
        isBoosting = false;
        Debug.Log("부스터 종료!");
    }
}