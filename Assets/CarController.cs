using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace JoystickLab
{

    public class CarController : MonoBehaviour
    {
        private Rigidbody2D rbody;
        
        [Header("Wheel Configuration")]
        public WheelPhysics2D[] wheels;
        public Transform com;

        private float _throtle;// store input throttle
        private float _steer; // store input steer
        private float _brake; // store input brake
        public float brakeForce;
        private float finalSpeed;
        
        [Header("Car Speed and Steering")]
        public float acceleration;
        public float accelRate; // How fast should it gain the max speed

        public bool useLerp;
        void Start()
        {
            rbody = GetComponent<Rigidbody2D>();
            rbody.centerOfMass = com.localPosition;
            
        }
        
        void FixedUpdate()
        {
            _throtle = Input.GetAxis("Horizontal");
           
            finalSpeed = useLerp
                ? Mathf.SmoothStep(finalSpeed, (_throtle * acceleration),
                    accelRate * Time.deltaTime)
                : _throtle * acceleration;
            DriveCar();
        }

        void DriveCar()
        {
            foreach (WheelPhysics2D wheel in wheels)
            {
                wheel.throttleSpeed = finalSpeed;
                ApplyBrake(wheel); 
            }
        }
        
        void ApplyBrake(WheelPhysics2D wheel)
        {
            wheel.brakeForce = _brake * brakeForce;
        }
    }

}
