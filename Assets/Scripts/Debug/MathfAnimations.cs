using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathfAnimations : MonoBehaviour
{

    [Header("BOBBING ANIMATION")]
    public float BobFrequency;
    public float BobbingAmount;
    
    
    private Vector3 _startPosition;
    private Vector3 _startScale;
    private float _startTime;
    private float angle;
    private void Start()
    {
        _startPosition = transform.position;
        _startScale = transform.localScale;
        _startTime = Time.time;
    }

    private void Update()
    {
        //transform.localScale = _startScale + Vector3.up * Extensions.BobbingAnimation(BobFrequency, BobbingAmount);
        //transform.position = _startPosition + Vector3.up * Extensions.BobbingAnimation(BobFrequency, BobbingAmount);

    }
}
