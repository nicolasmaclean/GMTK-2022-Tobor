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
    GameObject _charactersParent;
    
    [SerializeField]
    GameObject PF_Character;
    
    [SerializeField]
    Button _btn_play;

    ToggleGroup group;

    void Start()
    {
        DisplayCharacters();
    }

    void Update()
    {
        _btn_play.enabled = group.AnyTogglesOn();
    }

    void DisplayCharacters()
    {
        ToggleGroup g = this.group = _charactersParent.GetComponent<ToggleGroup>();
        
        for (int i = 0; i < 3; i++)
        {
            PlayerStats stats = PlayerStats.CreateRandom();
            Transform statsView = CreateStatsView(stats);
            statsView.SetParent(_charactersParent.transform);
            statsView.localScale = Vector3.one;
            
            Toggle toggle = statsView.GetComponentInChildren<Toggle>();
            toggle.group = g;
            toggle.isOn = false;
        }
        
        g.SetAllTogglesOff();
    }

    Transform CreateStatsView(PlayerStats stats)
    {
        StatsView view = Instantiate(PF_Character).GetComponent<StatsView>();
        view.Stats = stats;
        return view.transform;
    }
    
    public void Play()
    {
        PlayerStats.Instance = group.ActiveToggles().First().transform.parent.GetComponent<StatsView>().Stats;
        String playerName = IN_name.text;
        if (string.IsNullOrEmpty(playerName))
        {
            playerName = names[Random.Range(0, names.Length)];
        }
        
        PlayerStats.Instance.PlayerName = playerName;
        
        SceneController.Instance.LoadNextScene();
    }

    public void Back()
    {
        SceneController.Instance.LoadLastScene();
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
