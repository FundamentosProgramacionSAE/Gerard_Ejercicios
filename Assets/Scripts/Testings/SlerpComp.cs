using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlerpComp : MonoBehaviour
{
    public AnimationCurve AnimationCurve;
    public Transform EndPosition;
    public float Speed;
    
    private bool _return = true;
    private Vector3 _startPosition;
    private float _timer;
    private float _curveTime;

    private void Start()
    {
        _startPosition = transform.position;
    }

    private void Update()
    {
        LerpMovement();
    }

    private void LerpMovement()
    {
        _timer += Time.deltaTime;
        _curveTime = AnimationCurve.Evaluate(_timer * Speed);
        
        if (_return)
        {
            transform.position =
                Vector3.Slerp(transform.position, EndPosition.position, Mathf.InverseLerp(0,1,_curveTime));

            if (Vector3.Distance(transform.position, EndPosition.position) < 0.1f)
            {
                _timer = 0;
                _return = false;
            }
        }
        else
        {
            transform.position =
                Vector3.Slerp(transform.position, _startPosition, Mathf.InverseLerp(0,1,_curveTime));

            if (Vector3.Distance(transform.position, _startPosition) < 0.1f)
            {
                _timer = 0;
                _return = true;
            }
        }
    }
}