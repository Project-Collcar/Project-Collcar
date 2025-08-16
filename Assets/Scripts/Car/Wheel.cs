using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(WheelCollider))]
public class Wheel : MonoBehaviour
{
    public Transform visual;
    private WheelCollider wheelCollider;

    private void Awake()
    {
        wheelCollider = GetComponent<WheelCollider>();
    }
    
    private void LateUpdate()
    {
        UpdatePose();
    }

    public void UpdatePose()
    {
        if (visual is null) return;
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        visual.position = pos;
        visual.rotation = rot;
    }
    // 'Car' 스크립트에서 이 함수를 호출하여 바퀴에 구동 힘을 전달합니다.
    public void SetMotorTorque(float torque)
    {
        wheelCollider.motorTorque = torque;
    }

    // 'Car' 스크립트에서 이 함수를 호출하여 바퀴의 조향 각도를 설정합니다.
    public void SetSteerAngle(float angle)
    {
        wheelCollider.steerAngle = angle;
    }

    // 'Car' 스크립트에서 이 함수를 호출하여 바퀴에 브레이크 힘을 전달합니다.
    public void SetBrakeTorque(float brake)
    {
        wheelCollider.brakeTorque = brake;
    }
}
