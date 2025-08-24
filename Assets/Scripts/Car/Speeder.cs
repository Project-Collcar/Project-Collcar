using UnityEngine;
using Fusion;

public class Speeder : Car
{
    [Header("Speeder 고유 설정")]
    public float boostForce = 10f;   // 부스터가 가하는 힘
    public float boostDuration = 1.5f; // 부스터 지속 시간

    [Networked] private TickTimer boostTimer { get; set; }  
    
    // --- 디버그용 로컬 변수 ---
    private bool wasBoostingLastTick = false;
    
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
            // 2. 만료되었다면 타이머를 리셋(초기화)합니다.
            boostTimer = default; // 또는 TickTimer.None

            // 3. 부스터 종료 시점에 '한 번만' 실행될 로그입니다.  
            Debug.Log($"<color=orange>부스터 종료! (Object ID: {Object.Id})</color>");
        }

        // 4. 타이머가 '실행 중'일 때만 힘을 가합니다.
        //    위에서 만료된 타이머는 리셋되었으므로, 이 코드는 부스터 지속 시간 동안에만 실행됩니다.
        if (boostTimer.IsRunning)
        {
            rigidBody.AddForce(transform.forward * boostForce, ForceMode.Acceleration);
        }
        
        // --- 디버그 로그 로직 (그대로) ---
        bool isBoostingNow = boostTimer.IsRunning;
        if (wasBoostingLastTick && !isBoostingNow)
        {
            Debug.Log($"<color=orange>부스터 종료! (Object ID: {Object.Id})</color>");
        }
        wasBoostingLastTick = isBoostingNow;
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