using System;
using System.Collections;
using UnityEngine;

namespace Game.Utility
{
    public static class Coroutines
    {
        public static IEnumerator WaitThen(float seconds, Action callback)
        {
            yield return new WaitForSeconds(seconds);
            callback?.Invoke();
        }
    }
}