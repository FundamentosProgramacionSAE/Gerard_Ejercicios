using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player.Input;
using Player.Manager;
using Sirenix.OdinInspector;


namespace Player.Locomotion
{
    public class PlayerLocomotion : MonoBehaviour
    {
        public Rigidbody Rigidbody;
        public Vector3 moveDirection;
        
        [BoxGroup("Ground Stats"), SerializeField] private float groundDetectionStart = 0.5f;
        [BoxGroup("Ground Stats"), SerializeField] private float groundDirection = 0.5f;
        [BoxGroup("Ground Stats"), SerializeField] private float minDistanceToFall = 0.5f;
        [BoxGroup("Ground Stats")] public LayerMask groundLayer;
        [BoxGroup("Ground Stats")] public float InAirTimer;



        [BoxGroup("Movement Stats"), SerializeField] private float movementSpeed = 5;
        [BoxGroup("Movement Stats"), SerializeField] private float sprintSpeed = 5;
        [BoxGroup("Movement Stats"), SerializeField] private float rotationSpeed = 10;
        [BoxGroup("Movement Stats"), SerializeField] private float fallingSpeed = 45;

        private Transform camera;
        private InputHandler inputHandler;
        private Transform myTransform;
        private AnimatorHandler animatorHandler;
        private PlayerManager playerManager;
        private void Start()
        {
            playerManager = GetComponent<PlayerManager>();
            Rigidbody = GetComponent<Rigidbody>();
            inputHandler = GetComponent<InputHandler>();
            animatorHandler = GetComponentInChildren<AnimatorHandler>();
            camera = Camera.main.transform;
            myTransform = transform;
            
            animatorHandler.Initialize();

            playerManager.IsGrounded = true;
        }
        
        

        #region Movement
        
        private Vector3 normalVector;
        private Vector3 targetPosition;

        public void Handles()
        {
            if (inputHandler.MoveAmount > 0 && playerManager.CanCombo)
            {
                animatorHandler.DisableCombo();
            }
            
            HandleMovement();
            HandleRollingAndSprinting();
            HandleFalling();
        }

        private void HandleRotation(float delta)
        {
            Vector3 targetDir = Vector3.zero;
            float moveOverride = inputHandler.MoveAmount;


            targetDir = camera.forward * inputHandler.Vertical;
            targetDir += camera.right * inputHandler.Horizontal;
            
            targetDir.Normalize();
            targetDir.y = 0;


            if (targetDir == Vector3.zero) targetDir = myTransform.forward;

            float rs = rotationSpeed;
            
            Quaternion tr = Quaternion.LookRotation(targetDir);
            Quaternion targetRotation = Quaternion.Slerp(myTransform.rotation, tr,rs * delta);
            
            myTransform.rotation = targetRotation;
        }

        private void HandleMovement()
        {
            
            if(inputHandler.RollFlag) return;
            if(playerManager.IsInteracting) return;

            moveDirection = camera.forward * inputHandler.Vertical;
            moveDirection += camera.right * inputHandler.Horizontal;
            moveDirection.Normalize();
            moveDirection.y = 0;

            float speed = movementSpeed;

            if (inputHandler.SprintFlag && inputHandler.MoveAmount > 0.5f)
            {
                speed = sprintSpeed;
                playerManager.IsSprinting = true;
                moveDirection *= speed;
            }
            else
            {
                if (inputHandler.MoveAmount < 0.5f)
                {
                    moveDirection *= movementSpeed;
                    playerManager.IsSprinting = false;
                }
                else
                {
                    moveDirection *= speed;
                    playerManager.IsSprinting = false;
                }

            }
            


            Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);
            Rigidbody.velocity = projectedVelocity;
            
            animatorHandler.UpdateAnimatorValues(inputHandler.MoveAmount,0, playerManager.IsSprinting);

            if (animatorHandler.CanRotate)
            {
                HandleRotation(Time.deltaTime);
            }
        }

        private void HandleRollingAndSprinting()
        {
            if(animatorHandler.Animator.GetBool("isInteracting")) return;

            if (inputHandler.RollFlag)
            {
                moveDirection = camera.forward * inputHandler.Vertical;
                moveDirection += camera.right * inputHandler.Horizontal;

                if (inputHandler.MoveAmount > 0)
                {
                    animatorHandler.PlayTargetAnimation("Rolling", true);
                    moveDirection.y = 0;
                    Quaternion rollRotation = Quaternion.LookRotation(moveDirection);
                    myTransform.rotation = rollRotation;
                }

            }
        }

        public void HandleFalling()
        {
            
            playerManager.IsGrounded = true;
            RaycastHit hit;
            Vector3 origin = myTransform.position;
            origin.y += groundDetectionStart;
            
            if(Physics.Raycast(origin, myTransform.forward, out hit, 0.4f))
            {
                moveDirection = Vector3.zero;
            }

            if (playerManager.IsInAir)
            {
                Rigidbody.AddForce(-Vector3.up * fallingSpeed);
                Rigidbody.AddForce(moveDirection * fallingSpeed / 10f);
            }

            Vector3 dir = moveDirection;
            dir.Normalize();
            origin = origin + dir * groundDirection;

            targetPosition = myTransform.position;
            
            Debug.DrawRay(origin, -Vector3.up * minDistanceToFall, Color.red, 0.1f, false);
            
            if(Physics.Raycast(origin, -Vector3.up, out hit, minDistanceToFall, groundLayer))
            {
                normalVector = hit.normal;
                Vector3 tp = hit.point;
                playerManager.IsGrounded = true;
                targetPosition.y = tp.y;

                if (playerManager.IsInAir)
                {
                    if (InAirTimer > 0.5f)
                    {
                        animatorHandler.PlayTargetAnimation("Land", false);
                        InAirTimer = 0;
                    }
                    else
                    {
                        animatorHandler.PlayTargetAnimation("Empty", false);
                        InAirTimer = 0;
                    }

                    playerManager.IsInAir = false;
                }
            }
            else
            {
                if (playerManager.IsGrounded)
                {
                    playerManager.IsGrounded = false;
                }

                if (playerManager.IsInAir == false)
                {
                    if (playerManager.IsInteracting == false)
                    {
                        animatorHandler.PlayTargetAnimation("Falling", true);
                    }
                    
                    Vector3 vel = Rigidbody.velocity;
                    vel.Normalize();
                    Rigidbody.velocity = vel * (movementSpeed / 2f);
                    playerManager.IsInAir = true;
                }
            }

            if (playerManager.IsInteracting || inputHandler.MoveAmount > 0)
            {
                myTransform.position = Vector3.Lerp(myTransform.position, targetPosition, Time.deltaTime / 0.1f);
            }
            else
            {
                myTransform.position = targetPosition;

            }
        }

        #endregion
    }
}


