using System;
using Player.Input;
using Unity.Mathematics;
using UnityEngine;

namespace Player.CameraManager
{
    public class CameraHandler : MonoBehaviour
    {
        public static CameraHandler Instance { get; set; }

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
        public float LookAngle;
        public float PivotAngle;
        public float MinPivot;
        public float MaxPivot;







        private float _defaultPosition;
        private Vector3 cameraFollowVelocity = Vector3.zero;
        private Vector3 _cameraPosition;


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
                targetPosition = targetPosition - (distance - CameraCollisionOffset);
            }

            if (Mathf.Abs(targetPosition) < MinCollisionOffset)
            {
                targetPosition = targetPosition - MinCollisionOffset;
            }

            _cameraPosition.z = Mathf.Lerp(Camera.localPosition.z, targetPosition, Time.fixedDeltaTime / 0.2f);
            Camera.localPosition = _cameraPosition;
        }
    }
}