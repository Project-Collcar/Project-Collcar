using UnityEngine;
using Fusion;

public class Speeder : Car
{
    [Header("Speeder 고유 설정")]
    public float boostForce = 10f;   // 부스터가 가하는 힘
    public float boostDuration = 1.5f; // 부스터 지속 시간

    [Networked] private TickTimer boostTimer { get; set; }  
    
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
        
        // 1. 타이머가 만료되었는지 확인
        if (boostTimer.Expired(Runner))
        {
            // 2. 만료되었다면 타이머를 초기화
            boostTimer = default; // 또는 TickTimer.None

            // 3. 부스터 종료 시점에 실행  
            Debug.Log($"<color=orange>부스터 종료! (Object ID: {Object.Id})</color>");
        }

        // 4. 타이머가 실행 중일 때만 힘을 가함
        if (boostTimer.IsRunning)
        {
            rigidBody.AddForce(transform.forward * boostForce, ForceMode.Acceleration);
        }
    }
    
    // 부모가 호출해주는 HandleSkills 메서드의 내용을 채워서 스킬 발동
    protected override void HandleSkills(NetworkInputData current, NetworkInputData previous)
    {
        // 부모에게 받은 입력 데이터를 사용해 스킬이 "방금" 눌렸는지 확인
        if (current.Buttons.WasPressed(previous.Buttons, InputButtons.SKILL1))
        {
            UseSkill1();
        }
    }

    protected override void UseSkill1()
    {
        if (boostTimer.IsRunning)
        {
            Debug.Log("부스터 실행중");
            return;
        }

    Debug.Log($"<color=cyan>부스터 시작! (Object ID: {Object.Id})</color>");
        boostTimer = TickTimer.CreateFromSeconds(Runner, boostDuration);
    }
}