using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereAnimation : MonoBehaviour
{
    public bool FinishAnimation;

    private Animator _animator;
    private int _state;
    private void Start()
    {
        _animator = GetComponent<Animator>();
        FinishAnimation = true;
    }


    private void Update()
    {
        if(FinishAnimation==false) return;
        
        
        if (Input.anyKeyDown)
        {
            switch (_state)
            {
                case 0:
                    _animator.SetTrigger("Go");
                    _state++;
                    break;
                case 1:
                    _animator.SetTrigger("Return");
                    _state--;
                    break;
                default:
                    Extensions.DebugColor("Ha ocurrido un error", Color.red);
                    break;
            }
        }
    }

    public void SetFinish() => FinishAnimation = true;
    public void WaitToFinish() => FinishAnimation = false;

}
