using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlerpComp : MonoBehaviour
{
    public AnimationCurve AnimationCurve;
    public Transform EndPosition;
    public float Speed;

    private Vector3 _StartPosition;


    private void Start()
    {
        _StartPosition = transform.position;
    }

    private void Update()
    {
        transform.position =
            Vector3.Slerp(transform.position, EndPosition.position, AnimationCurve.Evaluate(Time.deltaTime *Speed ));
    }
}
