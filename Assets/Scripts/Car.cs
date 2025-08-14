using UnityEngine;

// 자동차의 전체 동작을 제어하는 메인 클래스
public class Car : MonoBehaviour
{
    [Header("Wheels")] // 바퀴 연결 
    public Wheel frontLeft;
    public Wheel frontRight;
    public Wheel rearLeft;
    public Wheel rearRight;

    [Header("Car Specs")] // 차량 능력치
    public float motorTorque = 0f;
    public float maxMotorTorque = 0f;
    public float steerAngle = 0f;
    public float brakeTorque = 0f;
    
    [Header("Physics")] // 물리 기능
    public Transform centerOfMassObject;
    private Rigidbody rigidBody;

    
    /*
    [Header("Effects & Sounds")]  // 효과 및 사운드
    public GameObject[] explosionEffect;
    public AudioClip engineSound;
    */

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.centerOfMass = centerOfMassObject.localPosition;
    }

    private void FixedUpdate()
    {
        // 1. 사용자 입력
        float steerInput = Input.GetAxis("Horizontal");
        float throttleInput  = Input.GetAxis("Vertical");
        bool handBrake = Input.GetButton("HandBrake");
        
        // 2. 조향, 구동, 브레이크 값 계산
        float steer = steerInput * steerAngle;
        float motor = throttleInput * motorTorque;
        float brake = 0f;
        // 핸드브레이크 로직
        if (handBrake)
            brake = brakeTorque;

        // 3. 계산된 값 적용
        // 앞바퀴 조향
        frontLeft.SetSteerAngle(steer);
        frontRight.SetSteerAngle(steer);
        
        // 뒷바퀴 구동
        rearLeft.SetMotorTorque(motor);
        rearRight.SetMotorTorque(motor);
        
        // 모든 바퀴 브레이크
        frontLeft.SetBrakeTorque(brake);
        frontRight.SetBrakeTorque(brake);
        rearLeft.SetBrakeTorque(brake);
        rearRight.SetBrakeTorque(brake);
    }
}