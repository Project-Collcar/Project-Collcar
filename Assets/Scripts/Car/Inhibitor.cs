using UnityEngine;

public class Inhibitor : Car
{
    //[Header("Speeder 고유 설정")]

    protected override void Awake()
    {
        base.Awake(); // 부모의 초기화를 먼저 실행
        
        motorTorque = 4500f;
        steerAngle = 40f;
        brakeTorque = 3000f;
        gravityMultiplier = 0.7f;
    }

    // 물리 효과는 FixedUpdateNetwork에서 처리합니다.
    public override void FixedUpdateNetwork()
    {
        // 1. 부모의 기본 운전 로직(핸들링, 엑셀 등)을 먼저 실행합니다.
        base.FixedUpdateNetwork();
    }
    
    private void Update()
    {
        
    }
    
}
