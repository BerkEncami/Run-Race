﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
   
    public InputField playerName;

    public Text currentLevelText,nextLevelText;
    private void Awake()
    {
        
    }
    void Start()
    {
        currentLevelText.text = PlayerPrefs.GetInt("Level", 1).ToString();
        nextLevelText.text = PlayerPrefs.GetInt("Level",1) + 1 + "";

        if (PlayerPrefs.GetInt("FirstTime", 0) == 0)
        {
            PlayerPrefs.GetInt("FirstTime", 1);
        }
        else
        {
            playerName.text = PlayerPrefs.GetString("PlayerName");
        }

    
    }

    
   public void StartGame()
    {
        if (playerName.text == "")
            PlayerPrefs.SetString("PlayerName", "Player");
        else
            PlayerPrefs.SetString("PlayerName", playerName.text);

        SceneManager.LoadScene(PlayerPrefs.GetInt("Level",1));
    }
}
