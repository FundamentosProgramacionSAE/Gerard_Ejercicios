using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;


public class CinemachineVirtualCameraShake : MonoBehaviour
{

    private CinemachineVirtualCamera _cinemachineVirtualCamera;

    private float _shakeTimer;

    private void Awake()
    {
        _cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    public void ShakeBoss(float intensity)
    {
        ShakeCamera(intensity, .75f);
    }
    public void ShakeCamera(float intensity, float time)
    {
        CinemachineBasicMultiChannelPerlin topRig =
            _cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        topRig.m_AmplitudeGain = intensity / 2;

        _shakeTimer = time;

    }

    private void Update()
    {

        if (_shakeTimer > 0)
        {
            _shakeTimer -= Time.deltaTime;
            if (_shakeTimer <= 0)
            {
                CinemachineBasicMultiChannelPerlin topRig =
                    _cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

                topRig.m_AmplitudeGain = 0;
            }
        }
    }
}


