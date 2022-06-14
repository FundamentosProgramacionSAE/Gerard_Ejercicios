using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class SlerpComp : MonoBehaviour
{
    public AnimationCurve AnimationCurve;
    public Transform EndPosition;
    public float Duration;
    public LayerMask CollisionMask;
    
    private bool _return = true;
    private Vector3 _startPosition;
    private float _evaluate;

    public int RandomVal;
    private void Start()
    {
        _startPosition = transform.position;
    }

    private void Update()
    {
        //LerpMovement();

        if (Keyboard.current[Key.H].wasPressedThisFrame)
        {
            var colliders = Physics.OverlapSphere(transform.position, 50, CollisionMask);

            foreach (var collider in colliders)
            {
                Debug.LogError(collider.name);
            }
        }
    }

    // private void LerpMovement()
    // {
    //     _evaluate += Time.deltaTime / Duration;
    //     if (_return)
    //     {
    //         transform.position =
    //             Vector3.Slerp(_startPosition, EndPosition.position, AnimationCurve.Evaluate(_evaluate));
    //
    //         CompareEvaluation(false);
    //     }
    //     else
    //     {
    //         transform.position =
    //             Vector3.Slerp(EndPosition.position, _startPosition, AnimationCurve.Evaluate(_evaluate));
    //
    //         CompareEvaluation(true);
    //     }
    // }
    //
    //
    // private void CompareEvaluation(bool _isReturn)
    // {
    //     if (_evaluate > 1f)
    //     {
    //         _evaluate = 0;
    //         _return = _isReturn;
    //     }
    // }
}