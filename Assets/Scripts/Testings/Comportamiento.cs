using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Comportamiento : MonoBehaviour
{
    
    [Header("BOBBING ANIMATION")]
    public float BobFrequency; //Velocidad
    public float BobbingAmount; //Unidades para mover

    public Color StartColor;
    public Color FinishColor;
    public Transform Cube;
    public bool UseScale;
    public bool UseMove;
    public bool UseColor;

    private Vector3 _startPosition;
    private Vector3 _startScale;
    private Material _material;


    private void Start()
    {
        Cube = transform;
        
        _material = Cube.GetComponent<MeshRenderer>().material;
        Cube.GetComponent<MeshRenderer>().sharedMaterial = _material;
        
        _startPosition = Cube.position;
        _startScale = Cube.localScale;
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
            _material.color = Color.Lerp(StartColor, FinishColor, Extensions.BobbingAnimation(BobFrequency, BobbingAmount));
        }

        if (UseMove)
        {
            Cube.position = _startPosition + Vector3.up * Extensions.BobbingAnimation(BobFrequency, BobbingAmount);
        }

    }
}
