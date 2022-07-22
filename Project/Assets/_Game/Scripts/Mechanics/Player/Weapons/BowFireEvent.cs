using System.Collections;
using System.Collections.Generic;
using Game.Mechanics.Player;
using UnityEngine;

public class BowFireEvent : MonoBehaviour
{
    public void Fire()
    {
        PlayerController.Instance.FireArrow();
    }
}
