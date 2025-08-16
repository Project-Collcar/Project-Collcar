using UnityEngine;
using Fusion;

// 자동차의 전체 동작을 제어하는 메인 클래스
public class Car : NetworkBehaviour
{
    [Header("Wheels")] // 바퀴 연결 
    public Wheel frontLeft;
    public Wheel frontRight;
    public Wheel rearLeft;
    public Wheel rearRight;

    [Header("Physics")] // 물리 기능
    public Transform centerOfMassObject;
    protected Rigidbody rigidBody;
    
    // 자식 클래스에서 덮어쓸 능력치 변수들 
    protected float motorTorque;
    protected float steerAngle;
    protected float brakeTorque;
    protected float gravityMultiplier = 1.0f;

    // 자식이 덮어쓸 수 있도록 protected virtual로 선언
    protected virtual void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        if (centerOfMassObject != null)
            rigidBody.centerOfMass = centerOfMassObject.localPosition;
    }

    // 모든 차량이 공통으로 사용할 운전 로직
    public override void FixedUpdateNetwork()
    {
        if (GetInput(out InputData data))
        {
            Debug.Log($"Input Received - Steer: {data.steerInput}, Throttle: {data.throttleInput}, HandBrake: {data.handBrake}");
            // 입력이 들어왔을 때만 로직을 실행
            //if (HasStateAuthority)     -> 개발용으로 잠시 주석처리함
            //{
                // 1. 사용자 입력
                float steerInput = data.steerInput;
                float throttleInput = data.throttleInput;
                bool handBrake = data.handBrake;

                // 2. 조향, 구동, 브레이크 값 계산
                float steer = steerInput * steerAngle;
                float motor = throttleInput * motorTorque;
                float brake = 0f;
                if (handBrake)
                    brake = brakeTorque;

                // 3. 계산된 값 적용
                frontLeft.SetSteerAngle(steer);
                frontRight.SetSteerAngle(steer);

                rearLeft.SetMotorTorque(motor);
                rearRight.SetMotorTorque(motor);

                frontLeft.SetBrakeTorque(brake);
                frontRight.SetBrakeTorque(brake);
                rearLeft.SetBrakeTorque(brake);
                rearRight.SetBrakeTorque(brake);

                // 4. 커스텀 중력
                rigidBody.AddForce(Vector3.down * (9.81f * gravityMultiplier), ForceMode.Acceleration);
                
                // 5. 스킬 입력 처리
                if (data.skill1) UseSkill1();
                if (data.skill2) UseSkill2();
            }
        //}
    }

    // 자식들이 각자의 스킬을 구현할 수 있는 틀
    public virtual void UseSkill1()
    {
        Debug.Log("이 차량은 1번 스킬이 없습니다.");
    }

    public virtual void UseSkill2()
    {
        Debug.Log("이 차량은 2번 스킬이 없습니다.");
    }
}