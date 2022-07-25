using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.UI;

namespace Game.Utility.UI
{
    public static class Tween
    {
        public static IEnumerator LerpColor(Graphic graphic, Color from, Color to, float duration, Action callback = null)
        {
            graphic.color = from;

            float elapsedTime = 0;
            while (elapsedTime < duration)
            {
                yield return null;
                elapsedTime += Time.deltaTime;
                graphic.color = Color.Lerp(from, to, elapsedTime / duration);
            }
            
            graphic.color = to;
            callback?.Invoke();
            yield break;
        }

        public static IEnumerator UseCurve(AnimationCurve curve, Action<float> Update)
        {
            Keyframe lastKey = curve.keys[curve.length - 1];
            float curTime = 0;
            float duration = lastKey.time;

            while (curTime < duration)
            {
                
                Update(curve.Evaluate(curTime));
                
                yield return null;
                curTime += Time.deltaTime;
            }
            
            Update(curve.Evaluate(lastKey.value));
        }

        public static IEnumerator SliderNonLerp(Slider slider, float target, float lerpFactor, float EPSILON = .01f)
        {
            float val = slider.value;
            while (Math.Abs(val - target) > EPSILON)
            {
                val = Mathf.Lerp(val, target, Time.deltaTime * lerpFactor);
                slider.value = val;
                yield return null;
            }

            slider.value = target;
        }
    }
}