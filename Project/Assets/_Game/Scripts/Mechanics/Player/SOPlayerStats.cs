using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Mechanics.Player
{
    [CreateAssetMenu(menuName = "Data/Player Stats")]
    public class SOPlayerStats : ScriptableObject
    {
        public PlayerStats Player;
    }
}
