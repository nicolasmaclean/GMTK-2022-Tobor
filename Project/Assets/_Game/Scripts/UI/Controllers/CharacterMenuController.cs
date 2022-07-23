using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.Core;
using Game.Mechanics.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CharacterMenuController : MonoBehaviour
{
    [SerializeField]
    TMP_InputField IN_name;

    [SerializeField]
    TMP_Text TXT_str;

    [SerializeField]
    TMP_Text TXT_dex;

    [SerializeField]
    TMP_Text TXT_con;

    [SerializeField]
    TMP_Text TXT_per;
    
    [SerializeField]
    Button _btn_play;

    void Start()
    {
        RollStats();
    }

    public void RollStats()
    {
        PlayerStats stats = PlayerStats.Instance = PlayerStats.CreateRandom();
        TXT_str.text = stats.Strength.ToString();
        TXT_dex.text = stats.Agility.ToString();
        TXT_con.text = stats.Constitution.ToString();
        TXT_per.text = stats.Perception.ToString();
    }

    public String RollName()
    {
        String playerName = names[Random.Range(0, names.Length)];
        IN_name.text = playerName;
        return playerName;
    }
    
    public void Play()
    {
        String playerName = IN_name.text ?? RollName();
        PlayerStats.Instance.PlayerName = playerName;
        
        SceneController.LoadNextScene();
    }

    public void Back()
    {
        SceneController.LoadLastScene();
    }

    readonly String[] names = new[]
    {
        "Lydan",
        "Syrin",
        "Ptorik",
        "Joz",
        "Varog",
        "Gethrod",
        "Hezra",
        "Feron",
        "Ophni",
        "Colborn",
        "Fintis",
        "Gatlin",
        "Jinto",
        "Hagalbar",
        "Krinn",
        "Lenox",
        "Revvyn",
        "Hodus",
        "Dimian",
        "Paskel",
        "Kontas",
        "Weston",
        "Azamarr",
        "Jather",
        "Tekren",
        "Jareth",
        "Adon",
        "Zaden",
        "Eune",
        "Graff",
        "Tez",
        "Jessop",
        "Gunnar",
        "Pike",
        "Domnhar",
        "Baske",
        "Jerrick",
        "Mavrek",
        "Riordan",
        "Wulfe",
        "Straus",
        "Tyvrik",
        "Henndar",
        "Favroe",
        "Whit",
        "Jaris",
        "Renham",
        "Kagran",
        "Lassrin",
        "Vadim",
        "Arlo",
        "Quintis",
        "Vale",
        "Caelan",
        "Yorjan",
        "Khron",
        "Ishmael",
        "Jakrin",
        "Fangar",
        "Roux",
        "Baxar",
        "Hawke",
        "Gatlen",
        "Barak",
        "Nazim",
        "Kadric",
        "Paquin",
    };
}
