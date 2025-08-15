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

    private void Update()
    {
        
    }
    
}
