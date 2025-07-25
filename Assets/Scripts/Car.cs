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

        float steering = h * turnSpeed * Mathf.Clamp01(1f - (speed / 3000f));
        turn = steering * Time.deltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        rigid.MoveRotation(rigid.rotation * turnRotation);

        bool breakSystem = Input.GetButton("Break");
        if (breakSystem)
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
