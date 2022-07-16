using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using Game.Utility;
using UnityEngine.Serialization;

// #if UNITY_EDITOR
// using UnityEditor;
// #endif

namespace Game.Mechanics.Player
{
    [SelectionBase]
    [RequireComponent(typeof(CharacterController))]
    public class FPSController : MonoBehaviour
    {
        #region public variables
        public static FPSController Instance { get; set; }
        
        [HideInInspector]
        public Vector3 Velocity;

        /// <summary>
        /// Grounded check is from transform.position, so the bottom
        /// of the player collider should be at transform.position. 
        /// </summary>
        public bool isGrounded { get; private set; }

        public Transform Cam;
        #endregion

        #region private variables
        [Header("Camera")]
        [SerializeField]
        bool _useMainCamera = true;

        [Header("Movement")]
        [SerializeField]
        Vector2 _speedRange = new Vector2(3, 10);
        
        [Utility.ReadOnly]
        [SerializeField]
        float _speed = 4f;

        [SerializeField]
        float _jumpHeight = 1.0f;

        [SerializeField]
        bool _canRun = true;

        [SerializeField]
        KeyCode _runKeyCode = KeyCode.LeftShift;

        [SerializeField]
        float _runMultiplier = 1.3f;

        [SerializeField]
        [Tooltip("Input Manager virtual axis for horizontal movement.")]
        string _horizontalAxis = "Horizontal";

        [SerializeField]
        [Tooltip("Input Manager virtual axis for vertical movement.")]
        string _verticalAxis = "Vertical";

        [Header("Physics")]
        [SerializeField]
        float _gravity = -9.81f;

        [SerializeField]
        float _groundedDistance = 0.1f;

        [SerializeField]
        [Tooltip("Layers to be considered by isGrounded check. Default is everything")]
        LayerMask _groundedMask = ~0;

        CharacterController _controller;
        #endregion

        #region Monobehaviour
        void Awake()
        {
            Instance = this;
            _controller = gameObject.GetComponent<CharacterController>();

            if (_useMainCamera)
            {
                Cam = Camera.main.transform;
            }
        }

        void Update()
        {
            isGrounded = GroundedCheck();
            MoveByInput();
            Jump();
            ApplyGravity();
        }

        void OnEnable()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        void OnDisable()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        #endregion

        public float UpdateSpeed(float fraction)
        {
            _speed = UpdateRange(_speedRange, fraction);
            return _speed;
        }

        float UpdateRange(Vector2 range, float fraction)
        {
            fraction = Mathf.Clamp(fraction, 0, 1);
            return Mathf.Lerp(range.x, range.y, fraction);
        }

        #region Private Methods
        /// <summary>
        /// Checks if this.transform is on the ground.
        /// </summary>
        /// <returns> true if within <see cref=""/> units of the ground </returns>
        bool GroundedCheck()
        {
            return Physics.CheckSphere(transform.position, _groundedDistance, _groundedMask);
        }

        /// <summary>
        /// Polls Input Systems "Horizontal" and "Vertical" axes 
        /// and moves the character controller accordingly.
        /// </summary>
        void MoveByInput()
        {
            // polls input
            float x = Input.GetAxis(_horizontalAxis);
            float z = Input.GetAxis(_verticalAxis);

            // relate input vector to player's direction
            Vector3 right = Cam.transform.right;
            Vector3 forward = Quaternion.Euler(0, -90, 0) * right;
            Vector3 move = right * x + forward * z;

            // apply multipliers
            move *= _speed;
            if (_canRun && Input.GetKey(_runKeyCode))
            {
                move *= _runMultiplier;
            }

            _controller.Move(move * Time.deltaTime);
        }

        /// <summary>
        /// Polls Input Systems "Jump" button and will jump if appropriate.
        /// This modifies velocity, so ApplyGravity must be called
        /// to apply velocity to the CharacterController.
        /// </summary>
        void Jump()
        {
            if (isGrounded && Input.GetButtonDown("Jump"))
            {
                Velocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
            }
        }

        /// <summary>
        /// Applies gravity to the player.
        /// </summary>
        void ApplyGravity()
        {
            if (isGrounded && Velocity.y < 0)
            {
                Velocity.y = -2f;
            }

            Velocity.y += _gravity * Time.deltaTime;
            _controller.Move(Velocity * Time.deltaTime);
        }
        #endregion
    }
}