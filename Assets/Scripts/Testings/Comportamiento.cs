using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Comportamiento : MonoBehaviour
{
    
    [Header("BOBBING ANIMATION")]
    public float BobFrequency; //Velocidad
    public float BobbingAmount; //Unidades para mover
    public MinMaxColor ColorFade;

    public bool UseScale;
    public bool UseMove;
    public bool UserRotation;
    public bool UseColor;

    private Vector3 _startPosition;
    private Vector3 _startScale;
    private Vector3 _startRotation;
    private Material _material;
    private Transform Cube;

    private void Start()
    {
        Cube = transform;
        
        _material = Cube.GetComponent<MeshRenderer>().material;
        Cube.GetComponent<MeshRenderer>().sharedMaterial = _material;
        
        _startPosition = Cube.position;
        _startScale = Cube.localScale;
        _startRotation = Cube.localEulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        if (UseScale)
        {
            Cube.localScale = _startScale + Vector3.one * Extensions.BobbingAnimation(BobFrequency, BobFrequency);
        }

        if (UseColor)
        {
            _material.color = ColorFade.GetValueFromRatio(Extensions.BobbingAnimation(BobFrequency, BobbingAmount));
        }

        if (UseMove)
        {
            Cube.position = _startPosition + Vector3.forward * Extensions.BobbingAnimation(BobFrequency, BobbingAmount);
        }

        if (UserRotation)
        {
            Cube.localEulerAngles =
                _startRotation + Vector3.up * Extensions.BobbingAnimation(BobFrequency, BobbingAmount);
        }

    }
}
