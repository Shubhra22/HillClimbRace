/*
Author: Shubhra Sarker
Company: JoystickLab
Email: ssuvro22@outlook.com
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using Random = UnityEngine.Random;

namespace JoystickLab
{
    public class TerrainGenerator2D : MonoBehaviour
    {
        private SpriteShapeController shape;


        public float x;
        public float y;

        public float pe;
        private void Start()
        {
            shape = GetComponent<SpriteShapeController>();


            Vector2 endPos = shape.spline.GetPosition(2) + Vector3.right * 1000;
            shape.spline.SetPosition(2,endPos);
            shape.spline.SetPosition(3,shape.spline.GetPosition(3) + Vector3.right * 100);
            
            for (int i = 0; i < 150; i++)
            {
                float xPos = shape.spline.GetPosition(i + 1).x + 6.67f;
                
                shape.spline.InsertPointAt(i+2,new Vector3(xPos,12*Mathf.PerlinNoise(i*Random.Range(5.0f,15.0f),0)));
            }

            for (int i = 2; i < 152; i++)
            {
                shape.spline.SetTangentMode(i,ShapeTangentMode.Continuous);
                shape.spline.SetLeftTangent(i,new Vector3(-2, 0, 0));
                shape.spline.SetRightTangent(i,new Vector3(2, 0, 0));
            }
            
        }
    }

}
