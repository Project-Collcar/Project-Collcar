using UnityEngine;

public class Car : MonoBehaviour
{
    public float speed;
    public float turnSpeed;

    Rigidbody rigid;
    float turn;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        float steering = h * turnSpeed;
        turn = steering * (rigid.linearVelocity.magnitude > 0.1f ? 1.0f : 0f) * Time.deltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        rigid.MoveRotation(rigid.rotation * turnRotation);

        bool brakeSystem = Input.GetButton("Brake");
        if (brakeSystem)
        {
            rigid.linearVelocity = Vector3.Lerp(rigid.linearVelocity, Vector3.zero, Time.deltaTime * 0.1f);
            rigid.angularVelocity = Vector3.Lerp(rigid.angularVelocity, Vector3.zero, Time.deltaTime * 0.1f);
        }
        else
        {
            Vector3 move = transform.forward * v * speed;
            rigid.AddForce(move, ForceMode.Force);
        }
    }
    
}
