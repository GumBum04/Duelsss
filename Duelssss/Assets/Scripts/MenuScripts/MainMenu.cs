using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public OptionsMenu optionsMenu;
    public void PlayLevelSelect()
    {
        SceneManager.LoadScene("LevelSelect");
    }

    public void OpenOptions()
    {
        Debug.Log("Options Clicked // Options Clicked");
        optionsMenu.OpenOptions();
    }
    public void CloseOptions()
    {
        optionsMenu.CloseOptions();
    }

    public void PlayLevel1()
    {
        SceneManager.LoadScene("Level1");
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void BackToLevelSelect()
    {
        SceneManager.LoadScene("LevelSelect");
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit(); 
    }
}
