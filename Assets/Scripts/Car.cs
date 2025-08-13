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
            if (visual is null) return;

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
    public float motorTorque = 5000f;
    public float steerAngle = 30f;
    public float brakeTorque = 3000f;

    private Rigidbody rb; // Rigidbody 컴포넌트를 캐싱할 변수

    private void Start()
    {
        // 매번 GetComponent를 호출하는 것은 비효율적이므로 시작할 때 한 번만 찾아옴
        rb = GetComponent<Rigidbody>();
    }

    // 물리 업데이트 주기에 맞춰 고정된 간격으로 실행되는 함수
    private void FixedUpdate()
    {
        // 1. 입력 받기 및 데드존 처리
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        v = Mathf.Abs(v) < 0.1f ? 0 : v; // 데드존

        bool handbrake = Input.GetButton("Handbrake");

        // 2. 조향, 구동, 브레이크 값 계산
        float steer = h * steerAngle;
        float motor = v * motorTorque;
        float brake = 0f;

        // 풋 브레이크 로직
        if ((v < 0 && frontLeft.collider.rpm > 10) || (v > 0 && frontLeft.collider.rpm < -10))
        {
            brake = this.brakeTorque;
            motor = 0;
        }

        // 핸드브레이크 로직
        if (handbrake)
        {
            brake = this.brakeTorque;
        }

        // 아이들링 브레이크 로직
        // *** 여기가 수정된 핵심 부분입니다 ***
        if (v == 0 && h == 0 && !handbrake && rb.linearVelocity.magnitude < 0.2f)
        {
            motor = 0f;
            brake = 200f; // 아이들링 브레이크 힘
        }

        // 3. 계산된 값을 실제 바퀴에 적용
        frontLeft.SetSteerAngle(steer);
        frontRight.SetSteerAngle(steer);

        if (!handbrake)
        {
            rearLeft.SetMotorTorque(motor);
            rearRight.SetMotorTorque(motor);
        }
        else
        {
            rearLeft.SetMotorTorque(0);
            rearRight.SetMotorTorque(0);
        }

        frontLeft.SetBrakeTorque(brake);
        frontRight.SetBrakeTorque(brake);
        rearLeft.SetBrakeTorque(brake);
        rearRight.SetBrakeTorque(brake);

        // 4. 시각적 업데이트
        frontLeft.UpdatePose();
        frontRight.UpdatePose();
        rearLeft.UpdatePose();
        rearRight.UpdatePose();

        // 5. 디버깅 (필요할 때만 사용)
        #if UNITY_EDITOR
        // if (!rearLeft.collider.GetGroundHit(out var hit))
        // {
        //     Debug.Log("Rear Left Wheel is NOT grounded.");
        // }
        #endif
    }
}