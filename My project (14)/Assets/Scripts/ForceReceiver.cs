using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceReceiver : MonoBehaviour
{
     [SerializeField] private CharacterController _controller;
     [SerializeField] private float drag = 0.3f;
     
     private Vector3 dampingVelocity;
     private Vector3 _impact;
     private float _verticalVelocity;
     

     public Vector3 Movement => _impact +  Vector3.up * _verticalVelocity;
 
     private void Update()
     {
          if (_verticalVelocity < 0f && _controller.isGrounded)
          {
               _verticalVelocity = Physics.gravity.y * Time.deltaTime;
          }
          else
          {
               _verticalVelocity += Physics.gravity.y * Time.deltaTime;
          }

          _impact = Vector3.SmoothDamp(_impact, Vector3.zero, ref dampingVelocity, drag);
     }

     public void AddForce(Vector3 force)
     {
          _impact += force;
     }
}
