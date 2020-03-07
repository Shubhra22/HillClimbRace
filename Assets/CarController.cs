using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace JoystickLab
{
    
    public class CarController : MonoBehaviour
    {
        [SerializeField] private WheelJoint2D backWheel;
        [SerializeField] private WheelJoint2D frontWheel;
    
        private JointMotor2D rearMotor;
        private JointMotor2D frontMotor;
        
    
        public float carSpeed;
        public float motorTorque;

        private Rigidbody2D rbody;
        // Start is called before the first frame update
        void Start()
        {
            rbody = GetComponent<Rigidbody2D>();
        }

        private void SetFrontMotorSpeed(float speed)
        {
            //frontMotor = frontWheel.motor;
            frontMotor.motorSpeed += speed;
            frontWheel.motor = frontMotor;
        }

        private void SetRearMotorSpeed(float speed)
        {
            rearMotor = backWheel.motor;
            rearMotor.motorSpeed = speed;
            backWheel.motor = rearMotor;
        }
    
        // Update is called once per frame
        void Update()
        {
            float speed = Input.GetAxis("Horizontal");
            if (speed > 0)
            {
                frontMotor.motorSpeed -= carSpeed;
                frontMotor.maxMotorTorque = 2000;
                frontWheel.motor = frontMotor;

                rearMotor.motorSpeed -= carSpeed;
                rearMotor.maxMotorTorque = 2000;
                backWheel.motor = rearMotor;
                
                
//                rearMotor.motorSpeed += Mathf.Clamp(speed * carSpeed, 0, 3000);
//                if (rearMotor.maxMotorTorque < 0) rearMotor.maxMotorTorque = 1;
//                rearMotor.maxMotorTorque -= motorTorque;
//                backWheel.motor = rearMotor;
//                frontWheel.motor = rearMotor;
            }
            else
            {
                if (rearMotor.motorSpeed > 0)
                {
                    rearMotor.motorSpeed -= Time.deltaTime * carSpeed;
                    rearMotor.maxMotorTorque += motorTorque;
                    backWheel.motor = rearMotor;
                    frontWheel.motor = rearMotor;
                }
            }

        }
    }

}
