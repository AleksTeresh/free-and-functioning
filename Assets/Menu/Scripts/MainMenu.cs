﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;
using UnityEngine.SceneManagement;

public class MainMenu : Menu
{

    void OnLevelWasLoaded()
    {
        Cursor.visible = true;
        if (PlayerManager.GetPlayerName() == "")
        {
            //no player yet selected so enable SetPlayerMenu
            GetComponent<MainMenu>().enabled = false;
            GetComponent<SelectPlayerMenu>().enabled = true;
        }
        else
        {
            //player selected so enable MainMenu
            GetComponent<MainMenu>().enabled = true;
            GetComponent<SelectPlayerMenu>().enabled = false;
        }
    }

    protected override void SetButtons()
    {
        buttons = new string[] { "New Game", "Load Game", "Change Player", "Quit Game" };
    }

    protected override void HandleButton(string text)
    {
        base.HandleButton(text);

        switch (text)
        {
            case "New Game": NewGame(); break;
            case "Load Game": LoadGame(); break;
            case "Change Player": ChangePlayer(); break;
            case "Quit Game": ExitGame(); break;
            default: break;
        }
    }

    protected override void HideCurrentMenu()
    {
        GetComponent<MainMenu>().enabled = false;
    }

    private void NewGame()
    {
        ResourceManager.MenuOpen = false;
        SceneManager.LoadScene("MiniGame");
        //makes sure that the loaded level runs at normal speed
        Time.timeScale = 1.0f;
    }

    private void ChangePlayer()
    {
        GetComponent<MainMenu>().enabled = false;
        GetComponent<SelectPlayerMenu>().enabled = true;
        SelectionList.LoadEntries(PlayerManager.GetPlayerNames());
    }
}