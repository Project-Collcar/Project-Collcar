using Unity.VisualScripting;
using UnityEngine;

// 이 스크립트를 가진 게임 오브젝트에 'WheelCollider' 컴포넌트 자동 추가
[RequireComponent(typeof(WheelCollider))]
public class Wheel : MonoBehaviour
{
    // 바퀴의 시각적 모델
    public Transform visual;
    // 물리적 바퀴를 제어하는 콜라이더
    private WheelCollider wheelCollider;

    private void Awake()
    {
        // 게임 시작 시, 동일한 오브젝트의 WheelCollider 컴포넌트를 가져옴
        wheelCollider = GetComponent<WheelCollider>();
    }
    
    // 바퀴 콜라이더의 위치와 회전을 시각적 모델에 동기화
    internal void UpdatePose()
    {
        // visual 트랜스폼이 없으면 함수 종료
        if (visual is null) return;
        
        Vector3 pos;
        Quaternion rot;
        
        // WheelCollider의 현재 위치와 회전을 가져옴
        wheelCollider.GetWorldPose(out pos, out rot);
        
        // 시각적 모델의 위치와 회전을 물리적 콜라이더에 맞춤
        visual.position = pos;
        visual.rotation = rot;
    }

    // 바퀴에 구동 토크를 적용
    public void SetMotorTorque(float torque)
    {
        wheelCollider.motorTorque = torque;
    }

    // 바퀴의 조향 각도를 설정
    public void SetSteerAngle(float angle)
    {
        wheelCollider.steerAngle = angle;
    }

    // 바퀴에 브레이크 토크를 적용
    public void SetBrakeTorque(float brake)
    {
        wheelCollider.brakeTorque = brake;
    }
}