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

    private void Update()
    {
        
    }
}
