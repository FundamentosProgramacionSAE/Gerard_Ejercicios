using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CinemachineFreeLookShake : MonoBehaviour
{
    private CinemachineFreeLook _cinemachineFreeLook;

    private float _shakeTimer;

    private void Awake()
    {
        _cinemachineFreeLook = GetComponent<CinemachineFreeLook>();
    }

    public void ShakeCamera(float intensity, float time)
    {
        CinemachineBasicMultiChannelPerlin topRig =
            _cinemachineFreeLook.GetRig(0).GetComponentInChildren<CinemachineBasicMultiChannelPerlin>();
        CinemachineBasicMultiChannelPerlin midRig =
            _cinemachineFreeLook.GetRig(1).GetComponentInChildren<CinemachineBasicMultiChannelPerlin>();
        CinemachineBasicMultiChannelPerlin botRig =
            _cinemachineFreeLook.GetRig(2).GetComponentInChildren<CinemachineBasicMultiChannelPerlin>();
        
        topRig.m_AmplitudeGain = intensity;
        botRig.m_AmplitudeGain = intensity;
        midRig.m_AmplitudeGain = intensity;
        
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
                    _cinemachineFreeLook.GetRig(0).GetComponentInChildren<CinemachineBasicMultiChannelPerlin>();
                CinemachineBasicMultiChannelPerlin midRig =
                    _cinemachineFreeLook.GetRig(1).GetComponentInChildren<CinemachineBasicMultiChannelPerlin>();
                CinemachineBasicMultiChannelPerlin botRig =
                    _cinemachineFreeLook.GetRig(2).GetComponentInChildren<CinemachineBasicMultiChannelPerlin>();
        
                topRig.m_AmplitudeGain = 0;
                botRig.m_AmplitudeGain = 0;
                midRig.m_AmplitudeGain = 0;
            }
        }
    }
}
