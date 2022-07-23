using System;
using Game.UI;
using UnityEngine;

namespace Game.Mechanics.Player.FPS
{
    /// <summary>
    /// This script handles Quake III CPM(A) mod style player movement logic.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class Q3PlayerController : MonoBehaviour
    {
        [Header("Aiming")]
        [SerializeField]
        Transform m_Camera;
        
        [SerializeField]
        MouseLook m_MouseLook = new MouseLook();

        [Header("Movement")]
        [SerializeField]
        float m_Friction = 6;
        
        [SerializeField]
        float m_Gravity = 20;
        
        [SerializeField]
        float m_JumpForce = 8;
        
        [SerializeField] 
        [Tooltip("Automatically jump when holding jump button")]
        bool m_AutoBunnyHop = false;
        
        [SerializeField]
        [Tooltip("How precise air control is")]
        float m_AirControl = 0.3f;
        
        [SerializeField]
        MovementSettings m_GroundSettings = new MovementSettings(7, 14, 10);
        
        [SerializeField]
        MovementSettings m_AirSettings    = new MovementSettings(7, 2, 2);
        
        [SerializeField]
        MovementSettings m_StrafeSettings = new MovementSettings(1, 50, 50);
        
        /// <summary>
        /// Returns player's current speed.
        /// </summary>
        public float Speed { get { return m_Character.velocity.magnitude; } }

        CharacterController m_Character;
        Vector3 m_PlayerVelocity = Vector3.zero;

        // Used to queue the next jump just before hitting the ground.
        bool m_JumpQueued = false;

        Vector3 m_MoveInput;
        Transform m_Tran;
        Transform m_CamTran;

        void Start()
        {
            m_Tran = transform;
            m_Character = GetComponent<CharacterController>();

            if (!m_Camera)
            {
                m_Camera = Camera.main.transform;
            }

            m_CamTran = m_Camera.transform;
            m_MouseLook.Init(m_Tran, m_CamTran);
            
            GameMenuController.Instance.OnStop.AddListener(Disable);
            GameMenuController.Instance.OnResume.AddListener(Enable);
        }

        void OnDestroy()
        {
            GameMenuController.Instance.OnStop.RemoveListener(Disable);
            GameMenuController.Instance.OnResume.RemoveListener(Enable);
        }

        void Enable() => enabled = true;
        void OnEnable()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        void Disable() => enabled = false;
        void OnDisable()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        void Update()
        {
            m_MoveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            QueueJump();

            // Set movement state.
            if (m_Character.isGrounded)
            {
                GroundMove();
            }
            else
            {
                AirMove();
            }

            // Rotate the character and camera.
            m_MouseLook.LookRotation(m_Tran, m_CamTran);

            // Move the character.
            m_Character.Move(m_PlayerVelocity * Time.deltaTime);
        }

        // Queues the next jump.
        void QueueJump()
        {
            if (m_AutoBunnyHop)
            {
                m_JumpQueued = Input.GetButton("Jump");
                return;
            }

            if (Input.GetButtonDown("Jump") && !m_JumpQueued)
            {
                m_JumpQueued = true;
            }

            if (Input.GetButtonUp("Jump"))
            {
                m_JumpQueued = false;
            }
        }

        // Handle air movement.
        void AirMove()
        {
            float accel;

            var wishdir = new Vector3(m_MoveInput.x, 0, m_MoveInput.z);
            wishdir = m_Tran.TransformDirection(wishdir);

            float wishspeed = wishdir.magnitude;
            wishspeed *= m_AirSettings.MaxSpeed;

            wishdir.Normalize();

            // CPM Air control.
            float wishspeed2 = wishspeed;
            if (Vector3.Dot(m_PlayerVelocity, wishdir) < 0)
            {
                accel = m_AirSettings.Deceleration;
            }
            else
            {
                accel = m_AirSettings.Acceleration;
            }

            // If the player is ONLY strafing left or right
            if (m_MoveInput.z == 0 && m_MoveInput.x != 0)
            {
                if (wishspeed > m_StrafeSettings.MaxSpeed)
                {
                    wishspeed = m_StrafeSettings.MaxSpeed;
                }

                accel = m_StrafeSettings.Acceleration;
            }

            Accelerate(wishdir, wishspeed, accel);
            if (m_AirControl > 0)
            {
                AirControl(wishdir, wishspeed2);
            }

            // Apply gravity
            m_PlayerVelocity.y -= m_Gravity * Time.deltaTime;
        }

        // Air control occurs when the player is in the air, it allows players to move side 
        // to side much faster rather than being 'sluggish' when it comes to cornering.
        void AirControl(Vector3 targetDir, float targetSpeed)
        {
            // Only control air movement when moving forward or backward.
            if (Mathf.Abs(m_MoveInput.z) < 0.001 || Mathf.Abs(targetSpeed) < 0.001)
            {
                return;
            }

            float zSpeed = m_PlayerVelocity.y;
            m_PlayerVelocity.y = 0;
            /* Next two lines are equivalent to idTech's VectorNormalize() */
            float speed = m_PlayerVelocity.magnitude;
            m_PlayerVelocity.Normalize();

            float dot = Vector3.Dot(m_PlayerVelocity, targetDir);
            float k = 32;
            k *= m_AirControl * dot * dot * Time.deltaTime;

            // Change direction while slowing down.
            if (dot > 0)
            {
                m_PlayerVelocity.x *= speed + targetDir.x * k;
                m_PlayerVelocity.y *= speed + targetDir.y * k;
                m_PlayerVelocity.z *= speed + targetDir.z * k;

                m_PlayerVelocity.Normalize();
            }

            m_PlayerVelocity.x *= speed;
            m_PlayerVelocity.y = zSpeed; // Note this line
            m_PlayerVelocity.z *= speed;
        }

        // Handle ground movement.
        void GroundMove()
        {
            // Do not apply friction if the player is queueing up the next jump
            if (!m_JumpQueued)
            {
                ApplyFriction(1.0f);
            }
            else
            {
                ApplyFriction(0);
            }

            var wishdir = new Vector3(m_MoveInput.x, 0, m_MoveInput.z);
            wishdir = m_Tran.TransformDirection(wishdir);
            wishdir.Normalize();

            var wishspeed = wishdir.magnitude;
            wishspeed *= m_GroundSettings.MaxSpeed;

            Accelerate(wishdir, wishspeed, m_GroundSettings.Acceleration);

            // Reset the gravity velocity
            m_PlayerVelocity.y = -m_Gravity * Time.deltaTime;

            if (m_JumpQueued)
            {
                m_PlayerVelocity.y = m_JumpForce;
                m_JumpQueued = false;
            }
        }

        void ApplyFriction(float t)
        {
            // Equivalent to VectorCopy();
            Vector3 vec = m_PlayerVelocity; 
            vec.y = 0;
            float speed = vec.magnitude;
            float drop = 0;

            // Only apply friction when grounded.
            if (m_Character.isGrounded)
            {
                float control = speed < m_GroundSettings.Deceleration ? m_GroundSettings.Deceleration : speed;
                drop = control * m_Friction * Time.deltaTime * t;
            }

            float newSpeed = speed - drop;
            if (newSpeed < 0)
            {
                newSpeed = 0;
            }

            if (speed > 0)
            {
                newSpeed /= speed;
            }

            m_PlayerVelocity.x *= newSpeed;
            // playerVelocity.y *= newSpeed;
            m_PlayerVelocity.z *= newSpeed;
        }

        // Calculates acceleration based on desired speed and direction.
        void Accelerate(Vector3 targetDir, float targetSpeed, float accel)
        {
            float currentspeed = Vector3.Dot(m_PlayerVelocity, targetDir);
            float addspeed = targetSpeed - currentspeed;
            if (addspeed <= 0)
            {
                return;
            }

            float accelspeed = accel * Time.deltaTime * targetSpeed;
            if (accelspeed > addspeed)
            {
                accelspeed = addspeed;
            }

            m_PlayerVelocity.x += accelspeed * targetDir.x;
            m_PlayerVelocity.z += accelspeed * targetDir.z;
        }
    }
    
    [System.Serializable]
    public class MovementSettings
    {
        public float MaxSpeed;
        public float Acceleration;
        public float Deceleration;

        public MovementSettings(float maxSpeed, float accel, float decel)
        {
            MaxSpeed = maxSpeed;
            Acceleration = accel;
            Deceleration = decel;
        }
    }
}