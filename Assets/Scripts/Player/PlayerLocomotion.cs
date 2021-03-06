using System;
using System.Collections;
using System.Collections.Generic;
using Player.CameraManager;
using UnityEngine;
using Player.Input;
using Player.Manager;
using Sirenix.OdinInspector;


namespace Player.Locomotion
{
    public class PlayerLocomotion : MonoBehaviour
    {
        internal Rigidbody Rigidbody;

        [TitleGroup("Ground Stats")]
        [SerializeField] private float raycastHeightOffset = 0.5f;
        [SerializeField] private float groundDirection = 0.5f;
        [SerializeField] private float leapingVelocity = 250f;
        [SerializeField] private float fallingSpeed = 600;
        [SerializeField] private float gravity = -5;
        [SerializeField] private float heightJump = 2;
        [SerializeField] private LayerMask GroundLayer;
        [SerializeField] private float InAirTimer;


        [TitleGroup("Movement Stats")]
        [SerializeField] private float movementSpeed = 5;
        [SerializeField] private float sprintSpeed = 10;
        [SerializeField] private float rotationSpeed = 10;
        
        
        public CapsuleCollider CharacterCollider;
        public CapsuleCollider CharacterBlockerCollider;
        

        private Vector3 moveDirection;
        private Transform camera;
        private InputHandler inputHandler;
        private Transform myTransform;
        private PlayerAnimatorManager _playerAnimatorManager;
        private PlayerManager playerManager;
        private CameraHandler _cameraHandler;


        private void Awake()
        {
            _playerAnimatorManager = GetComponentInChildren<PlayerAnimatorManager>();
            _playerAnimatorManager.Initialize();
        }

        private void Start()
        {
            playerManager = GetComponent<PlayerManager>();
            Rigidbody = GetComponent<Rigidbody>();
            inputHandler = GetComponent<InputHandler>();
            _cameraHandler = CameraHandler.Instance;
            camera = Camera.main.transform;
            myTransform = transform;

            playerManager.IsGrounded = true;
            
            
            Physics.IgnoreCollision(CharacterCollider, CharacterBlockerCollider, true);
        }
        
        

        #region Movement
        
        /// <summary>
        /// Funcion encargada de llamar al HandleFalling y HandleMovement
        /// </summary>
        public void Handles()
        {
            HandleFalling();
            
            // Reiniciar el combo al moverse y si esta usando el combo
            if (inputHandler.MoveAmount > 0 && playerManager.CanCombo)
            {
                _playerAnimatorManager.DisableCombo();
            }
            
            HandleMovement();
        }
        

        /// <summary>
        /// Funcion encargada del movimiento del personaje.
        /// </summary>
        private void HandleMovement()
        {
            // Si esta en salto o esta interactuando no seguira ejecutando
            if(playerManager.IsJumping) return;
            if(playerManager.IsInteracting) return;

            // Obtemenos la direccion de movimiento a partir de los inputs
            // y de donde apuntemos con la camara
            moveDirection = camera.forward * inputHandler.Vertical;
            moveDirection += camera.right * inputHandler.Horizontal;
            moveDirection.Normalize();
            moveDirection.y = 0;

            // Velocidad
            float speed = movementSpeed;

            // Si estas corriendo y la velocidad de movimiento es superior a
            // 0.5f la velocidad ser?? de correr. Sino caminara
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


            // Le asignamos la velodicad a nuestro Rigidbody.
            Rigidbody.velocity = new Vector3(moveDirection.x, Rigidbody.velocity.y, moveDirection.z);

            if (inputHandler.LockOnFlag && inputHandler.SprintFlag == false)
            {
                // Actualizamos las animaciones segun el movimiento
                _playerAnimatorManager.UpdateAnimatorValues(inputHandler.Vertical,inputHandler.Horizontal, playerManager.IsSprinting);
            }
            else
            {
                // Actualizamos las animaciones segun el movimiento
                _playerAnimatorManager.UpdateAnimatorValues(inputHandler.MoveAmount,0, playerManager.IsSprinting);
            }
        }
        
        /// <summary>
        /// Funcion encargada de la rotacion del personaje
        /// </summary>
        /// <param name="delta"></param>
        public void HandleRotation(float delta)
        {
            // Si podemos rotar, esta tocando el suelo y no estamos saltando el jugador podra rotar
            if (!_playerAnimatorManager.CanRotate || !playerManager.IsGrounded || playerManager.IsJumping) return;
            
            if (inputHandler.LockOnFlag && inputHandler.SprintFlag == false)
            {
                if (inputHandler.SprintFlag || inputHandler.RollInput)
                {
                    Vector3 targetDirection = Vector3.zero;
                    targetDirection = camera.forward * inputHandler.Vertical;
                    targetDirection += camera.right * inputHandler.Horizontal;
                    targetDirection.Normalize();
                    targetDirection.y = 0;

                    if (targetDirection == Vector3.zero)
                    {
                        targetDirection = transform.forward;
                    }
                    
                    Quaternion tr = Quaternion.LookRotation(targetDirection);
                    Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr,rotationSpeed * Time.deltaTime);
                    transform.rotation = targetRotation;
                }
                else
                {
                    Vector3 rotationDirection = moveDirection;
                    rotationDirection = _cameraHandler._currentLockOnTarget.transform.position - transform.position;
                    rotationDirection.y = 0;
                    rotationDirection.Normalize();
                    Quaternion tr = Quaternion.LookRotation(rotationDirection);
                    Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, rotationSpeed * Time.deltaTime);
                    transform.rotation = targetRotation;
                }

            }
            else
            {
                Vector3 targetDir = Vector3.zero;
                
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


        }

        public void HandleRollingAndSprinting()
        {
            if(_playerAnimatorManager.Animator.GetBool("isInteracting")) return;
            moveDirection = camera.forward * inputHandler.Vertical;
            moveDirection += camera.right * inputHandler.Horizontal;

            if (inputHandler.MoveAmount > 0)
            {
                _playerAnimatorManager.PlayTargetAnimation("Rolling", true);
                moveDirection.y = 0;
                Quaternion rollRotation = Quaternion.LookRotation(moveDirection);
                myTransform.rotation = rollRotation;
            }
        }

        public void HandleFalling()
        {
            RaycastHit hit;
            Vector3 rayCastOrigin = transform.position;
            Vector3 targetPosition;
            rayCastOrigin.y = rayCastOrigin.y + raycastHeightOffset;
            targetPosition = transform.position;


            if (!playerManager.IsGrounded && !playerManager.IsJumping)
            {
                if (!playerManager.IsInteracting)
                {
                    _playerAnimatorManager.PlayTargetAnimation("Falling", true);
                }

                InAirTimer += Time.deltaTime;
                Rigidbody.AddForce(transform.forward * leapingVelocity);
                Rigidbody.AddForce(-Vector3.up * fallingSpeed * InAirTimer);
            }

            if (Physics.SphereCast(rayCastOrigin, 0.2f, Vector3.down, out hit, groundDirection,GroundLayer))
            {
                if (!playerManager.IsGrounded && !playerManager.IsInteracting)
                {
                    _playerAnimatorManager.PlayTargetAnimation("Land", true);
                    Rigidbody.velocity = Vector3.zero;
                }

                Vector3 rayCastHitPoint = hit.point;
                targetPosition.y = rayCastHitPoint.y;
                InAirTimer = 0;
                playerManager.IsGrounded = true;
            }
            else
            {
                playerManager.IsGrounded = false;
            }

            if (playerManager.IsGrounded && !playerManager.IsJumping)
            {
                if (playerManager.IsInteracting || inputHandler.MoveAmount > 0)
                {
                    transform.position = targetPosition;
                }
                else
                {
                    transform.position = targetPosition;
                }
            }
        }


        public void HandleJumping()
        {
            if(playerManager.IsInteracting) return;
            if (playerManager.IsGrounded && inputHandler.MoveAmount >0)
            {
                _playerAnimatorManager.Animator.SetBool("isJumping", true);
                _playerAnimatorManager.PlayTargetAnimation("Jump", false);

                float jumpVelocity = Mathf.Sqrt(-1.5f * gravity * heightJump);
                Vector3 playerVelocity = moveDirection;
                playerVelocity.y = jumpVelocity;
                Rigidbody.velocity = playerVelocity;
            }
        }

        #endregion
    }
}


