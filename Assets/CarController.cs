using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] private WheelJoint2D backWheel;

    private JointMotor2D motorBack;

    public float backwardSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float speed = Input.GetAxis("Horizontal");
        motorBack.motorSpeed = speed * backwardSpeed;
        backWheel.motor = motorBack;


    }
}
