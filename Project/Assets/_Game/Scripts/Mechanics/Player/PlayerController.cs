using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Game.Mechanics.Player
{
    [SelectionBase]
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        #region public variables
        public static PlayerController Instance { get; set; }
        
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
        float _playerSpeed = 2.0f;

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
            move *= _playerSpeed;
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
        
        
#if UNITY_EDITOR
        // is within FPSController to use nameof() on private fields instead of hardcoded strings
        [CustomEditor(typeof(PlayerController))]
        public class PlayerControllerEditor : Editor
        {
            SerializedProperty _cameraProperty;
            SerializedProperty _useMainCameraProperty;

            SerializedProperty _playerSpeedProperty;
            SerializedProperty _jumpHeightProperty;
            SerializedProperty _canRunProperty;
            SerializedProperty _runKeyCodeProperty;
            SerializedProperty _runMultiplierProperty;
            SerializedProperty _horizontalAxisProperty;
            SerializedProperty _verticalAxisProperty;

            SerializedProperty _gravityProperty;
            SerializedProperty _groundedDistanceProperty;
            SerializedProperty _groundedMaskProperty;

            public void OnEnable()
            {
                _cameraProperty = serializedObject.FindProperty(nameof(Cam));
                _useMainCameraProperty = serializedObject.FindProperty(nameof(_useMainCamera));

                _playerSpeedProperty = serializedObject.FindProperty(nameof(_playerSpeed));
                _jumpHeightProperty = serializedObject.FindProperty(nameof(_jumpHeight));
                _canRunProperty = serializedObject.FindProperty(nameof(_canRun));
                _runKeyCodeProperty = serializedObject.FindProperty(nameof(_runKeyCode));
                _runMultiplierProperty = serializedObject.FindProperty(nameof(_runMultiplier));
                _horizontalAxisProperty = serializedObject.FindProperty(nameof(_horizontalAxis));
                _verticalAxisProperty = serializedObject.FindProperty(nameof(_verticalAxis));

                _gravityProperty = serializedObject.FindProperty(nameof(_gravity));
                _groundedDistanceProperty = serializedObject.FindProperty(nameof(_groundedDistance));
                _groundedMaskProperty = serializedObject.FindProperty(nameof(_groundedMask));
            }

            public override void OnInspectorGUI()
            {
                EditorGUILayout.PropertyField(_useMainCameraProperty);
                if (!_useMainCameraProperty.boolValue)
                {
                    EditorGUI.indentLevel += 1;
                
                    EditorGUILayout.PropertyField(_cameraProperty);
                
                    EditorGUI.indentLevel -= 1;
                }
                
                // movement
                EditorGUILayout.PropertyField(_playerSpeedProperty);
                EditorGUILayout.PropertyField(_jumpHeightProperty);
                EditorGUILayout.PropertyField(_canRunProperty);
                if (_canRunProperty.boolValue)
                {
                    EditorGUI.indentLevel += 1;

                    EditorGUILayout.PropertyField(_runKeyCodeProperty);
                    if (_runKeyCodeProperty.enumValueIndex != 0)
                    {
                        EditorGUILayout.PropertyField(_runMultiplierProperty);
                    }

                    EditorGUI.indentLevel -= 1;
                }
                EditorGUILayout.PropertyField(_horizontalAxisProperty);
                EditorGUILayout.PropertyField(_verticalAxisProperty);

                // physics
                EditorGUILayout.PropertyField(_gravityProperty);
                EditorGUILayout.PropertyField(_groundedDistanceProperty);
                EditorGUILayout.PropertyField(_groundedMaskProperty);

                serializedObject.ApplyModifiedProperties();
            }
        }
#endif
    }
}