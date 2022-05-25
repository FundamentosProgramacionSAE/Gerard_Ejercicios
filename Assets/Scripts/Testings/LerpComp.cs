using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpComp : MonoBehaviour
{
    public AnimationCurve AnimationCurve;
    public Transform EndPosition;
    public float Speed;
    
    private bool _return = true;
    private Vector3 _StartPosition;
    private float _timer;
    private float _curveTime;

    private void Start()
    {
        _StartPosition = transform.position;
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
                Vector3.Lerp(transform.position, EndPosition.position, 1 * _curveTime);

            if (Vector3.Distance(transform.position, EndPosition.position) < 0.1f)
            {
                _timer = 0;
                _return = false;
            }
        }
        else
        {
            transform.position =
                Vector3.Lerp(transform.position, _StartPosition, 1 * _curveTime);

            if (Vector3.Distance(transform.position, _StartPosition) < 0.1f)
            {
                _timer = 0;
                _return = true;
            }
        }
    }
}
