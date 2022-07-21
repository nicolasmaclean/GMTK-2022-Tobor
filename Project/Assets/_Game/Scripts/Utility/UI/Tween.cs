using System;
using System.Collections;
using UnityEngine;
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
    }
}