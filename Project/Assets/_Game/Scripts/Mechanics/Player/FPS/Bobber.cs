using System.Collections;
using System.Collections.Generic;
using Game.UI;
using UnityEngine;

namespace Game.Mechanics.Player.FPS
{
    public class Bobber : MonoBehaviour
    {
        public float bobbingSpeed = 0.08f;
        public float bobbingAmount = 0.04f;
    
        float timer;
        const float midpoint = .43f;
        
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
            float waveslice = 0.0f;
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            Vector3 cSharpConversion = transform.localPosition;

            if (Mathf.Abs(horizontal) == 0 && Mathf.Abs(vertical) == 0)
            {
                timer = 0.0f;
            }
            else
            {
                waveslice = Mathf.Sin(timer);
                timer += bobbingSpeed;
                if (timer > Mathf.PI * 2)
                {
                    timer -= (Mathf.PI * 2);
                }
            }
            if (waveslice != 0)
            {
                float translateChange = waveslice * bobbingAmount;
                float totalAxes = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
            
                totalAxes = Mathf.Clamp(totalAxes, 0.0f, 1.0f);
                translateChange = totalAxes * translateChange;
                cSharpConversion.y = midpoint + translateChange;
            }
            else
            {
                cSharpConversion.y = midpoint;
            }

            transform.localPosition = cSharpConversion;
        }
    }
}