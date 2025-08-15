using UnityEngine;

public class Juggernaut : Car
{
    //[Header("Juggernaut 고유 설정")]

    protected override void Awake()
    {
        base.Awake();
        
        motorTorque = 6000f;
        steerAngle = 40f;
        brakeTorque = 5000f;
        gravityMultiplier = 1.5f;
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
