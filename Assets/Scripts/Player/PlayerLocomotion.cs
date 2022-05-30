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
        
        [BoxGroup("Ground Stats"), SerializeField] private float raycastHeightOffset = 0.5f;
        [BoxGroup("Ground Stats"), SerializeField] private float groundDirection = 0.5f;
        [BoxGroup("Ground Stats"), SerializeField] private float LeapingVelocity = 0.5f;
        [BoxGroup("Ground Stats")] public LayerMask groundLayer;
        [BoxGroup("Ground Stats")] public float InAirTimer;



        [BoxGroup("Movement Stats"), SerializeField] private float movementSpeed = 5;
        [BoxGroup("Movement Stats"), SerializeField] private float sprintSpeed = 5;
        [BoxGroup("Movement Stats"), SerializeField] private float rotationSpeed = 10;
        [BoxGroup("Movement Stats"), SerializeField] private float fallingSpeed = 45;


        public float Gravity;
        public float HeightJump;

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
            HandleFalling();
            
            if (inputHandler.MoveAmount > 0 && playerManager.CanCombo)
            {
                animatorHandler.DisableCombo();
            }
            
            HandleMovement();
            HandleRollingAndSprinting();
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
            if(playerManager.IsJumping) return;
            
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



            Vector3 projectedVelocity = moveDirection;
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
            RaycastHit hit;
            Vector3 rayCastOrigin = transform.position;
            rayCastOrigin.y = rayCastOrigin.y + raycastHeightOffset;


            if (!playerManager.IsGrounded && !playerManager.IsJumping)
            {
                if (!playerManager.IsInteracting)
                {
                    animatorHandler.PlayTargetAnimation("Falling", true);
                }

                InAirTimer += Time.deltaTime;
                Rigidbody.AddForce(transform.forward * LeapingVelocity);
                Rigidbody.AddForce(-Vector3.up * fallingSpeed * InAirTimer);
            }

            if (Physics.SphereCast(rayCastOrigin, 0.2f, -Vector3.up, out hit, groundDirection,groundLayer))
            {
                if (!playerManager.IsGrounded && playerManager.IsInteracting)
                {
                    animatorHandler.PlayTargetAnimation("Land", true);
                }

                InAirTimer = 0;
                playerManager.IsGrounded = true;
            }
            else
            {
                playerManager.IsGrounded = false;
            }
        }


        public void HandleJumping()
        {
            if (playerManager.IsGrounded && inputHandler.MoveAmount >0)
            {
                animatorHandler.Animator.SetBool("isJumping", true);
                animatorHandler.PlayTargetAnimation("Jump", false);

                float jumpVelocity = Mathf.Sqrt(-2 * Gravity * HeightJump);
                Vector3 playerVelocity = moveDirection;
                playerVelocity.y = jumpVelocity;
                Rigidbody.velocity = playerVelocity;
            }
        }

        #endregion
    }
}


