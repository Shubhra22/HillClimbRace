/*
Author: Shubhra Sarker
Company: JoystickLab
Email: ssuvro22@outlook.com
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace JoystickLab
{
    public class WheelPhysics2D : MonoBehaviour
    {
        public Transform wheelGraphics;
        private bool isGrounded;
        [SerializeField] [Range(0,5)] private float radius;// radius of my wheel
        [SerializeField] private float suspensionLen; // How big the spring is
        [SerializeField] private float stiffness; // kind of the force applied by the suspension spring (tightness of the spring). Differs from car to car
        [SerializeField] private float damper; // Damper is used to slow down the force caused by the suspension spring... it kind of causes reverse force with the stiffness??
        // we need this two compressions to find the displacement (ds) of the spring.. we use the displacement to calculate the relative velocity. (hooks law)
        private Vector3 _wheelVelocity;
        
        private float springCompression;
        private float lastSpringCompression;

        public float throttleSpeed;
        public float brakeForce;

        [SerializeField]private Rigidbody2D rbody;

        
        private void FixedUpdate()
        {
            isGrounded = false;
            Ray2D ray = new Ray2D(transform.position,-transform.up);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.up, radius + suspensionLen,
                LayerMask.GetMask("Ground"));
            if (hit.collider != null)
            {
                lastSpringCompression = springCompression;
                springCompression = radius + suspensionLen - hit.distance;
                float relativeVelocity = ( springCompression - lastSpringCompression) / Time.fixedDeltaTime;
                float suspensionForce = stiffness * springCompression + damper * relativeVelocity;
                _wheelVelocity = transform.InverseTransformDirection(rbody.GetPointVelocity(hit.point));
                float forwardDriveForce = throttleSpeed * suspensionForce;

                float forwardFriction = (_wheelVelocity.x + transform.up.x) * suspensionForce;
                print(forwardFriction);
                forwardDriveForce -= brakeForce;
                
                Vector3 resultantForce = transform.up * suspensionForce +
                                         transform.right * (forwardDriveForce - forwardFriction);
                
                
                rbody.AddForceAtPosition(resultantForce,hit.point);
                isGrounded = true;

            }
        }

        private void Update()
        {
            WheelGraphicsPlacements();
        }

        void WheelGraphicsPlacements()
        {
            float springSize = suspensionLen - springCompression;
            Vector3 wheelCenter = transform.position - transform.up * springSize;
            wheelGraphics.transform.position = wheelCenter;

            float rotationDir = Vector3.Dot(_wheelVelocity.normalized, transform.right);
            // Calculate Angular Velocity... w = 2*pi*f from HighSchool physics.
            float circumference = 2f * Mathf.PI * radius; // d = 2*pi*r
            float frequency = _wheelVelocity.magnitude/circumference;// f = 1/t ; but d = vt, so f = 1/d/v ; => f=v/d
            float angularVelocity = 360 * frequency * rotationDir; // so angular velocity w = 2*pi*f. convert it to degree 2*pi = 360 

            wheelGraphics.transform.Rotate(0,0,-angularVelocity * Time.deltaTime);
        }
        
        private void DrawWheel(Vector3 wheelCenter)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, wheelCenter);
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(wheelCenter, 0.01f);
            
            Gizmos.color = Color.red;
            Gizmos.DrawLine(wheelCenter, wheelCenter - transform.up * radius );
            Handles.color = Color.red;
            Handles.DrawWireDisc(wheelCenter,transform.forward,radius);
            
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(wheelCenter - transform.up * radius, 0.01f);
        }
        
        private void OnDrawGizmosSelected()
        {
#if UNITY_EDITOR
            float springSize = suspensionLen;
            Vector3 wheelCenter = Vector3.zero;
            
            
            if (Application.isPlaying && isGrounded)
            {
                springSize = (suspensionLen - springCompression);
                wheelCenter = transform.position - transform.up * springSize;
            }
            else
            {
                wheelCenter = transform.position - transform.up * springSize;
            }
            DrawWheel(wheelCenter);
            //DrawSpring(springSize);
#endif
        }
    }

}
