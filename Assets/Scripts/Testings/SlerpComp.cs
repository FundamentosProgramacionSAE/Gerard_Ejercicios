﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlerpComp : MonoBehaviour
{
    public AnimationCurve AnimationCurve;
    public Transform EndPosition;
    public float Speed;
    
    private bool Return = true;
    private Vector3 _StartPosition;


    private void Start()
    {
        _StartPosition = transform.position;
    }

    private void Update()
    {
        SlerpMovement();
    }

    private void SlerpMovement()
    {
        if (Return)
        {
            transform.position =
                Vector3.Slerp(transform.position, EndPosition.position, AnimationCurve.Evaluate(Time.deltaTime * Speed));

            if (Vector3.Distance(transform.position, EndPosition.position) < 0.5f) Return = false;
        }
        else
        {
            transform.position =
                Vector3.Slerp(transform.position, _StartPosition, AnimationCurve.Evaluate(Time.deltaTime * Speed));

            if (Vector3.Distance(transform.position, _StartPosition) < 0.5f) Return = true;
        }
    }
}