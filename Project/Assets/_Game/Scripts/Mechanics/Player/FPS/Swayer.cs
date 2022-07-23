using System.Collections;
using System.Collections.Generic;
using Game.UI;
using UnityEngine;

namespace Game.Mechanics.Player.FPS
{
    public class Swayer : MonoBehaviour
    {
        public float swaySmoothing;
        public float swayingAmount;
        
        void Start()
        {
            GameMenuController.Instance.OnStop.AddListener(Disable);
            GameMenuController.Instance.OnResume.AddListener(Enable);
        }

        void OnDestroy()
        {
            GameMenuController.Instance.OnStop.RemoveListener(Disable);
            GameMenuController.Instance.OnResume.RemoveListener(Enable);
        }

        void Enable() => enabled = true;
        void Disable() => enabled = false;

        void Update()
        {
            float mouseX = Input.GetAxisRaw("Mouse X") * swayingAmount;
            float mouseY = Input.GetAxisRaw("Mouse Y") * swayingAmount;

            Quaternion rotationX = Quaternion.AngleAxis(mouseY, Vector3.left);
            Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);

            Quaternion targetRotation = rotationX * rotationY;

            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, swaySmoothing * Time.deltaTime);
        }
    }
}