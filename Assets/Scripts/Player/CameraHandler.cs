using System;
using System.Collections.Generic;
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
        public LayerMask CollisionLayers;
        public float CameraCollisionRadius = 2;
        public float CameraCollisionOffset = 0.2f;
        public float MinCollisionOffset = 0.2f;
        public Transform Camera;
        public InputHandler InputHandler;
        public Transform Target;
        public Transform CameraPivot;
        public float CameraFollowSpeed;
        public float CameraLookSpeed = 2;
        public float CameraPivotSpeed;
        public float LockedPivotPosition = 2.25f;
        public float UnlockedPivotPosition = 1.65f;
        public float LookAngle;
        public float PivotAngle;
        public float MinPivot;
        public float MaxPivot;
        public float MaxLockOnDistance = 30;
        public List<CharacterManager> AvaliablesTargets = new List<CharacterManager>();






        private float _defaultPosition;
        private Vector3 dollyDir;
        private Vector3 cameraFollowVelocity = Vector3.zero;
        private Vector3 _cameraPosition;
        internal CharacterManager _nearestLockOn;
        internal CharacterManager _currentLockOnTarget;


        private void Awake()
        {
            Instance = this;
            Camera = UnityEngine.Camera.main.transform;
            _defaultPosition = Camera.localPosition.z;
        }

        public void HandleCameraMovement()
        {
            FollowTarget();
            RotateCamera();
            HandleCameraCollisions();
        }
        
        
        private void FollowTarget()
        {
            Vector3 targetPosition = Vector3.SmoothDamp(transform.position, Target.position, ref cameraFollowVelocity,
                CameraFollowSpeed);
            transform.position = targetPosition;
        }

        private void RotateCamera()
        {
            if (InputHandler.LockOnFlag == false)
            {
                Vector3 rotation;
                Quaternion targetRotation;

                LookAngle = LookAngle + (InputHandler.MouseX * CameraLookSpeed);
                PivotAngle = PivotAngle - (InputHandler.MouseY * CameraPivotSpeed);
                PivotAngle = Mathf.Clamp(PivotAngle, MinPivot, MaxPivot);
            
                rotation = Vector3.zero;
                rotation.y = LookAngle;
                targetRotation = Quaternion.Euler(rotation);
                transform.rotation = targetRotation;
            
                rotation = Vector3.zero;
                rotation.x = PivotAngle;
                targetRotation = Quaternion.Euler(rotation);
                CameraPivot.localRotation = targetRotation;
            }
            else
            {
                float velocity = 0;
                Vector3 dir = _currentLockOnTarget.transform.position - transform.position;
                dir.Normalize();
                dir.y = 0;
                
                Quaternion targetRotation = Quaternion.LookRotation(dir);
                transform.rotation = targetRotation;

                dir = _currentLockOnTarget.transform.position - CameraPivot.position;
                dir.Normalize();

                targetRotation = Quaternion.LookRotation(dir);
                Vector3 eulerAngles = targetRotation.eulerAngles;
                eulerAngles.y = 0;
                CameraPivot.localEulerAngles = eulerAngles;
            }
            
            

        }


        private void HandleCameraCollisions()
        {
            float targetPosition = _defaultPosition;
            RaycastHit hit;
            Vector3 direciton = Camera.position - CameraPivot.position;
            direciton.Normalize();
            
            if (Physics.SphereCast(CameraPivot.transform.position, CameraCollisionRadius, direciton, out hit,
                    Mathf.Abs(targetPosition), CollisionLayers))
            {
                float distance = Vector3.Distance(CameraPivot.position, hit.point);
                targetPosition = - (distance - CameraCollisionOffset);
            }
            
            if (Mathf.Abs(targetPosition) < MinCollisionOffset)
            {
                targetPosition = - MinCollisionOffset;
            }
            
            _cameraPosition.z = Mathf.Lerp(Camera.localPosition.z, targetPosition, Time.deltaTime/0.2f);
            Camera.localPosition = _cameraPosition;

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
                            Debug.DrawLine(PlayerManager.LockOn.position, character.LockOn.position);
                            print(hit.transform.gameObject.layer.ToString());
                            if (hit.transform.gameObject == character.gameObject)
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

        public void SetCameraHeight()
        {
            Vector3 velocity = Vector3.zero;
            Vector3 newLockedPosition = new Vector3(0, LockedPivotPosition);
            Vector3 newUnlockedPosition = new Vector3(0, UnlockedPivotPosition);

            if (_currentLockOnTarget != null)
            {
                CameraPivot.transform.localPosition = Vector3.SmoothDamp(CameraPivot.transform.localPosition,
                    newLockedPosition, ref velocity, Time.deltaTime);
            }
            else
            {
                CameraPivot.transform.localPosition = Vector3.SmoothDamp(CameraPivot.transform.localPosition,
                    newUnlockedPosition, ref velocity, Time.deltaTime);
            }

        }
    }
}