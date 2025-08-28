using UnityEngine;
using Fusion;

// 모든 차량의 기반이 되는 부모 클래스
public class Car : NetworkBehaviour
{
    [Header("Wheels")] // 주행에 사용될 4개의 Wheel 컴포넌트 연결
    public Wheel frontLeft;
    public Wheel frontRight;
    public Wheel rearLeft;
    public Wheel rearRight;

    [Header("Physics")] // 물리 관련 설정
    public Transform centerOfMassObject; // 안정적인 주행을 위한 무게 중심 오브젝트
    protected Rigidbody rigidBody;

    [Header("Ability")] // 차량의 기본 능력치, 자식 클래스에서 재정의 가능
    protected float motorTorque;
    protected float steerAngle;
    protected float brakeTorque;
    protected float gravityMultiplier = 1.0f;

    [Header("Network Properties")] // 네트워크를 통해 동기화될 플레이어의 입력 값
    [Networked] protected float steerInput { get; set; }
    [Networked] protected float throttleInput { get; set; }
    [Networked] protected bool handBrake { get; set; }
    
    private NetworkInputData previousInputData;
    
    // 컴포넌트 초기화, 자식 클래스에서 확장할 수 있도록 virtual로 선언
    protected virtual void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        if (centerOfMassObject != null)
            rigidBody.centerOfMass = centerOfMassObject.localPosition;
    }

    // 자식 클래스에서 주행 로직을 확장할 수 있도록 virtual로 선언
    public override void FixedUpdateNetwork()
    {
        // NetworkManager가 보내준 이번 틱의 입력 데이터를 가져옴
        if (GetInput(out NetworkInputData data))
        {
            // 입력 값을 바탕으로 실제 물리 효과 적용
            float steer = data.steerInput * steerAngle;
            float motor = data.throttleInput * motorTorque;
            float brake = 0f;
            if (data.handBrake)
                brake = brakeTorque;

            // 각 바퀴에 계산된 값을 전달
            frontLeft.SetSteerAngle(steer);
            frontRight.SetSteerAngle(steer);
            rearLeft.SetMotorTorque(motor);
            rearRight.SetMotorTorque(motor);
            frontLeft.SetBrakeTorque(brake);
            frontRight.SetBrakeTorque(brake);
            rearLeft.SetBrakeTorque(brake);
            rearRight.SetBrakeTorque(brake);
            
            // 자식 클래스가 스킬을 처리하도록 현재와 이전 입력 데이터를 넘김
            HandleSkills(data, previousInputData);

            // 다음 틱에서 사용하기 위해 현재 입력을 저장
            previousInputData = data;
        }

        // 중력은 권한을 가진 클라이언트에서만 계산하여 모두에게 적용
        if (Object.HasStateAuthority)
        {
            rigidBody.AddForce(Vector3.down * (9.81f * gravityMultiplier), ForceMode.Acceleration);
        }
    }
    
    // 모든 물리 및 네트워크 계산(보간)이 끝난 후 호출
    // 시각적 요소를 최종적으로 업데이트하기에 적합
    public override void Render()
    {
        frontLeft.UpdatePose();
        frontRight.UpdatePose();
        rearLeft.UpdatePose();
        rearRight.UpdatePose();
    }
    
    protected virtual void HandleSkills(NetworkInputData current, NetworkInputData previous)
    {
        
    }
    
    protected virtual void UseSkill1()
    {
        Debug.Log("이 차량은 1번 스킬이 없음");
    }

    protected virtual void UseSkill2()
    {
        Debug.Log("이 차량은 2번 스킬이 없음");
    }
}