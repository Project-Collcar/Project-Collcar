using UnityEngine;
using UnityEngine.InputSystem;

public class Car : MonoBehaviour
{
    [System.Serializable]
    public class Wheel
    {
        public WheelCollider collider;
        public Transform visual;

        public void UpdatePose()
        {
            if (visual == null) return;
            Vector3 pos;
            Quaternion rot;
            collider.GetWorldPose(out pos, out rot);
            visual.position = pos;
            visual.rotation = rot;
        }

        public void SetMotorTorque(float torque) => collider.motorTorque = torque;
        public void SetSteerAngle(float angle) => collider.steerAngle = angle;
        public void SetBrakeTorque(float brake) => collider.brakeTorque = brake;
    }

    public Wheel frontLeft;
    public Wheel frontRight;
    public Wheel rearLeft;
    public Wheel rearRight;

    public float motorTorque = 1000000f;
    public float steerAngle = 30f;
    public float brakeTorque = 7000f;

    private Vector2 moveInput;
    private bool isBraking;
    private CarController controls;

    WheelHit hit;


    private void Awake()
    {
        controls = new CarController();

        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        controls.Player.Brake.performed += ctx => isBraking = true;
        controls.Player.Brake.canceled += ctx => isBraking = false;
    }
    private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();


    private void FixedUpdate()
    {
        float h = moveInput.x; // 좌우 입력
        float v = moveInput.y; // 전후 입력

        float motor = v * motorTorque;
        float steer = h * steerAngle;
        float brake = isBraking ? brakeTorque : 0f;

        bool isForward = v > 0;
        bool isBackward = v < 0;

        // 입력 상충 무효화
        if ((isForward && isBackward) || (isForward && isBraking) || (isBackward && isBraking))
        {
            v = 0;
            isBraking = false;
        }

        Debug.Log($"Left MotorTorque: {rearLeft.collider.motorTorque}, IsGrounded: {rearLeft.collider.GetGroundHit(out var hit1)}");
        Debug.Log($"Right MotorTorque: {rearRight.collider.motorTorque}, IsGrounded: {rearRight.collider.GetGroundHit(out var hit2)}");
        //Debug.Log($"Rigidbody velocity: {GetComponent<Rigidbody>().linearVelocity}");

        // 조향
        frontLeft.SetSteerAngle(steer);
        frontRight.SetSteerAngle(steer);

        // 구동
        rearLeft.SetMotorTorque(motor);
        rearRight.SetMotorTorque(motor);

        // 브레이크
        frontLeft.SetBrakeTorque(brake);
        frontRight.SetBrakeTorque(brake);
        rearLeft.SetBrakeTorque(brake);
        rearRight.SetBrakeTorque(brake);

        // 시각 바퀴 업데이트
        frontLeft.UpdatePose();
        frontRight.UpdatePose();
        rearLeft.UpdatePose();
        rearRight.UpdatePose();
    }
}
