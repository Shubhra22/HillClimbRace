using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CarController : MonoBehaviour
{
    [SerializeField] private WheelJoint2D backWheel;
    [SerializeField] private WheelJoint2D frontWheel;
    
    private JointMotor2D motor;
    
    public float carSpeed;
    public float motorTorque;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float speed = Input.GetAxis("Horizontal");
        motor.motorSpeed = speed * carSpeed;
        motor.maxMotorTorque = motorTorque;
        
        backWheel.motor = motor;
        frontWheel.motor = motor;

    }
}
