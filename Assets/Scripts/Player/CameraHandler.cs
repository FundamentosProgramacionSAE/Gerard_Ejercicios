using System;
using System.Collections.Generic;
using Cinemachine;
using Player.Input;
using Player.Manager;
using Unity.Mathematics;
using UnityEngine;

namespace Player.CameraManager
{
    public class CameraHandler : MonoBehaviour
    {
        public static CameraHandler Instance { get; set; }


        public PlayerManager PlayerManager;
        public InputHandler InputHandler;
        public Transform Camera;
        public Transform Target;
        public float MaxLockOnDistance = 30;
        public List<CharacterManager> AvaliablesTargets = new List<CharacterManager>();
        public Animator CameraAnimator;
        public CinemachineVirtualCamera VirtualCamera;

        internal CharacterManager _nearestLockOn;
        internal CharacterManager _currentLockOnTarget;


        private void Awake()
        {
            Instance = this;
            Camera = UnityEngine.Camera.main.transform;
        }

        private void Update()
        {
            if (InputHandler.LockOnFlag == false)
            {
                CameraAnimator.CrossFade("FollowCamera", 0.2f);
            }
            else
            {
                CameraAnimator.CrossFade("TargetCamera", 0.2f);
                RaycastHit hit;
                if (_currentLockOnTarget != null)
                {
                    if (Physics.Linecast(PlayerManager.LockOn.position, _currentLockOnTarget.LockOn.position, out hit))
                    {
                        if (!hit.transform.CompareTag("Enemy"))
                        {
                            InputHandler.ClearCamera();
                        }
                    }
                }
            }
        }
        

        public void HandleLockOn()
        {
            float shortestDistance = Mathf.Infinity;
            float minDistanceOfLeftTarget = -Mathf.Infinity;
            float minDistanceOfRightTarget = Mathf.Infinity;

            Collider[] colliders = Physics.OverlapSphere(Target.position, 26);

            for (int i = 0; i < colliders.Length; i++)
            {
                CharacterManager character = colliders[i].GetComponent<CharacterManager>();

                if (character != null)
                {
                    Vector3 lockTargetDirection = character.transform.position - Target.position;
                    float distanceFromTarget = Vector3.Distance(Target.position , character.transform.position);
                    float viewableAngle = Vector3.Angle(lockTargetDirection, Camera.forward);
                    RaycastHit hit;

                    if (character.transform.root != Target.transform.root && viewableAngle > -50 &&
                        viewableAngle < 50 && distanceFromTarget <= MaxLockOnDistance)
                    {
                        if (Physics.Linecast(PlayerManager.LockOn.position, character.LockOn.position, out hit))
                        {
                            if (hit.transform.CompareTag("Enemy"))
                            {
                                AvaliablesTargets.Add(character);
                            }

                        }

                    }
                }
            }

            
            for (int k = 0; k < AvaliablesTargets.Count; k++)
            {
                float distanceFromTarget = Vector3.Distance(Target.position, AvaliablesTargets[k].transform.position);

                if (distanceFromTarget < shortestDistance)
                {
                    shortestDistance = distanceFromTarget;
                    _nearestLockOn = AvaliablesTargets[k];
                }
            }
            
            
        }

        public void ClearLockOnTargets()
        {
            AvaliablesTargets.Clear();
            _nearestLockOn = null;
            _currentLockOnTarget = null;
        }
    }
}