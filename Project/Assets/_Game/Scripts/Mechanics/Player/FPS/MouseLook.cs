using System;
using UnityEngine;

namespace Game.Mechanics.Player.FPS
{
    /// <summary>
    /// Custom script based on the version from the Standard Assets.
    /// </summary>
    [Serializable]
    public class MouseLook
    {
        [SerializeField]
        float m_XSensitivity = 2f;
        
        [SerializeField]
        float m_YSensitivity = 2f;
        
        [SerializeField]
        bool m_ClampVerticalRotation = true;
        
        [SerializeField]
        float m_MinimumX = -90F;
        
        [SerializeField]
        float m_MaximumX = 90F;
        
        [SerializeField]
        bool m_Smooth = false;
        
        [SerializeField]
        float m_SmoothTime = 5f;
        
        [SerializeField]
        bool m_LockCursor = true;

        Quaternion m_CharacterTargetRot;
        Quaternion m_CameraTargetRot;

        public void Init(Transform character, Transform camera)
        {
            m_CharacterTargetRot = character.localRotation;
            m_CameraTargetRot = camera.localRotation;
        }

        public void LookRotation(Transform character, Transform camera)
        {
            float yRot = Input.GetAxis("Mouse X") * m_XSensitivity;
            float xRot = Input.GetAxis("Mouse Y") * m_YSensitivity;

            m_CharacterTargetRot *= Quaternion.Euler(0f, yRot, 0f);
            m_CameraTargetRot *= Quaternion.Euler(-xRot, 0f, 0f);

            if (m_ClampVerticalRotation)
            {
                m_CameraTargetRot = ClampRotationAroundXAxis(m_CameraTargetRot);
            }

            if (m_Smooth)
            {
                character.localRotation = Quaternion.Slerp(character.localRotation, m_CharacterTargetRot,
                    m_SmoothTime * Time.deltaTime);
                camera.localRotation = Quaternion.Slerp(camera.localRotation, m_CameraTargetRot,
                    m_SmoothTime * Time.deltaTime);
            }
            else
            {
                character.localRotation = m_CharacterTargetRot;
                camera.localRotation = m_CameraTargetRot;
            }
        }

        public void SetCursorLock(bool value)
        {
            m_LockCursor = value;
            if (!m_LockCursor)
            {//we force unlock the cursor if the user disable the cursor locking helper
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        Quaternion ClampRotationAroundXAxis(Quaternion q)
        {
            q.x /= q.w;
            q.y /= q.w;
            q.z /= q.w;
            q.w = 1.0f;

            float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

            angleX = Mathf.Clamp(angleX, m_MinimumX, m_MaximumX);

            q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

            return q;
        }
    }
}
