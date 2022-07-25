using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI.Hud
{
    public static class HudController
    {
        public static Action OnShow;
        public static Action OnHide;

        public static void Show() => OnShow?.Invoke();
        public static void Hide() => OnHide?.Invoke();

        public static InteractPrompt InteractPrompt;
    }
}