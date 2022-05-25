using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlerpComp : MonoBehaviour
{
    public AnimationCurve AnimationCurve;
    public Transform EndPosition;
    public float Duration;
    
    private bool _return = true;
    private Vector3 _startPosition;
    private float _evaluate;


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
        _evaluate += Time.deltaTime / Duration;
        if (_return)
        {
            transform.position =
                Vector3.Slerp(_startPosition, EndPosition.position, AnimationCurve.Evaluate(_evaluate));

            CompareEvaluation(false);
        }
        else
        {
            transform.position =
                Vector3.Slerp(EndPosition.position, _startPosition, AnimationCurve.Evaluate(_evaluate));

            CompareEvaluation(true);
        }
    }


    private void CompareEvaluation(bool _isReturn)
    {
        if (_evaluate > 1f)
        {
            _evaluate = 0;
            _return = _isReturn;
        }
    }
}