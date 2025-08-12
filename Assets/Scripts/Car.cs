using UnityEngine;

// 자동차의 전체 동작을 제어하는 메인 클래스
public class Car : MonoBehaviour
{
    // 각 바퀴의 물리(콜라이더)와 외형(모델)을 함께 관리하는 클래스
    [System.Serializable]
    public class Wheel
    {
        public WheelCollider collider; // 물리 계산을 담당하는 휠 콜라이더
        public Transform visual;       // 화면에 보이는 바퀴 모델의 Transform

        // 물리 바퀴의 위치와 회전을 시각적 바퀴 모델에 동기화하는 함수
        public void UpdatePose()
        {
            if (visual == null) return;
            Vector3 pos;
            Quaternion rot;
            collider.GetWorldPose(out pos, out rot);
            visual.position = pos;
            visual.rotation = rot;
        }

        // WheelCollider의 주요 속성을 설정하는 함수들
        public void SetMotorTorque(float torque) => collider.motorTorque = torque;
        public void SetSteerAngle(float angle) => collider.steerAngle = angle;
        public void SetBrakeTorque(float brake) => collider.brakeTorque = brake;
    }

    // 인스펙터 창에서 연결할 4개의 바퀴
    public Wheel frontLeft;
    public Wheel frontRight;
    public Wheel rearLeft;
    public Wheel rearRight;

    // 자동차의 주요 능력치 (힘, 최대 조향각, 브레이크 힘)
    public float motorTorque = 1000000f;
    public float steerAngle = 30f;
    public float brakeTorque = 7000f;


    // 물리 업데이트 주기에 맞춰 고정된 간격으로 실행되는 함수
    private void FixedUpdate()
    {
        // 1. 입력 받기
        float h = Input.GetAxisRaw("Horizontal"); // 좌/우 키보드 입력 (-1, 0, 1)
        float v = Input.GetAxisRaw("Vertical");   // 앞/뒤 키보드 입력 (-1, 0, 1)
        bool handbrakePressed = Input.GetButton("Handbrake"); // 핸드브레이크(기본: 스페이스바) 입력

        // 2. 입력 값으로 실제 물리력 계산
        float motor = v * motorTorque;        // 모터 힘 계산
        float steer = h * steerAngle;         // 조향 각도 계산
        float brake = handbrakePressed ? brakeTorque : 0f; // 브레이크 힘 계산

        // 3. 예외 처리
        bool isForward = v > 0;
        bool isBackward = v < 0;

        // 전진/후진과 브레이크 동시 입력 시 동력을 0으로 만들어 충돌 방지
        if ((isForward && isBackward) || (isForward && handbrakePressed) || (isBackward && handbrakePressed))
        {
            motor = 0;
        }

        // 디버깅용: 각 바퀴의 토크와 지면 접지 여부 콘솔에 출력

        //Debug.Log($"Left MotorTorque: {rearLeft.collider.motorTorque}, IsGrounded: {rearLeft.collider.GetGroundHit(out var hit1)}");
        //Debug.Log($"Right MotorTorque: {rearRight.collider.motorTorque}, IsGrounded: {rearRight.collider.GetGroundHit(out var hit2)}");

        if(!rearLeft.collider.GetGroundHit(out var hit1))
        {
            Debug.Log($"IsGrounded: {rearLeft.collider.GetGroundHit(out var hit)}");
        }
        if(!rearRight.collider.GetGroundHit(out var hit2))
        {
            Debug.Log($"IsGrounded: {rearRight.collider.GetGroundHit(out var hit)}");
        }

        // 4. 계산된 값을 실제 바퀴에 적용
        // 조향: 앞바퀴에만 적용
        frontLeft.SetSteerAngle(steer);
        frontRight.SetSteerAngle(steer);

        // 구동: 뒷바퀴에만 적용 (후륜 구동)
        rearLeft.SetMotorTorque(motor);
        rearRight.SetMotorTorque(motor);

        // 브레이크: 모든 바퀴에 적용
        frontLeft.SetBrakeTorque(brake);
        frontRight.SetBrakeTorque(brake);
        rearLeft.SetBrakeTorque(brake);
        rearRight.SetBrakeTorque(brake);

        // 5. 시각적 업데이트
        // 모든 바퀴의 3D 모델 위치와 회전을 물리 상태에 맞춰 업데이트
        frontLeft.UpdatePose();
        frontRight.UpdatePose();
        rearLeft.UpdatePose();
        rearRight.UpdatePose();
    }
}