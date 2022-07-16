using System;
using System.Collections;
using System.Collections.Generic;
using Game.Mechanics.Player;
using TMPro;
using UnityEngine;

public class StatsView : MonoBehaviour
{
    public PlayerStats Stats;
        
    [SerializeField]
    GameObject PF_Stat;

    [SerializeField]
    Transform _statsParent;

    void Start()
    {
        Init();
    }

    void Init()
    {
        String[] propertyNames = PlayerStats.GetFieldNames();

        for (int i = 0; i < 5; i++)
        {
            Transform stat = Instantiate(PF_Stat, _statsParent, true).transform;
            TMP_Text stat_name = stat.Find("TXT_Name").GetComponent<TMP_Text>();
            TMP_Text stat_num  = stat.Find("TXT_Num").GetComponent<TMP_Text>();

            stat_name.text = propertyNames[i];
            stat_num.text = ((int) Stats.GetType()
                .GetField(propertyNames[i])
                .GetValue(Stats)).ToString();
        }
    }
}
