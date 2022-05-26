using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public Animator Animator;
    public CharacterController CharacterController;
    public float WalkSpeed;
    public float RunSpeed;

    private float _velocity;
    
    private void Update()
    {
        float xAxis = Input.GetAxis("Horizontal");
        float zAxis = Input.GetAxis("Vertical");

        Vector3 Movement = new Vector3(xAxis, 0, zAxis);
        
        _velocity = Input.GetKey(KeyCode.LeftShift) ? RunSpeed : WalkSpeed;
        
        
        CharacterController.Move(Movement * (Time.deltaTime * _velocity));


        if (_velocity != RunSpeed)
        {
            Animator.SetFloat("Horizontal", xAxis, 0.1f, Time.deltaTime);
            Animator.SetFloat("Forward", zAxis, 0.1f, Time.deltaTime);
        }
        else
        {
            Animator.SetFloat("Forward", 2, 0.1f,Time.deltaTime);
        }



    }
}
