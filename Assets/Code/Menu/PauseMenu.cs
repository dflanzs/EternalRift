using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{

    public GameObject OptionMenu;
    public GameObject PasueMenu;
    public GameObject LeaveMenu;

    public void MainMenuButton()
    {
        // Play Now Button has been pressed, here you can initialize your game (For example Load a Scene called GameLevel etc.)
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene("MenuPrincipal");

    }

    public void OptionButton()
    {
        // Show Credits Menu
        PasueMenu.SetActive(false);
        OptionMenu.SetActive(true);
    }

    public void PauseMenuButton()
    {
        // Show Main Menu
        PasueMenu.SetActive(true);
        OptionMenu.SetActive(false);
    }

    public void LeaveMenuButton()
    {
        // Show Main Menu
        LeaveMenu.SetActive(true);
        PasueMenu.SetActive(false);
    }

    public void PauseMenuButton2()
    {
        // Show Main Menu
        PasueMenu.SetActive(true);
        LeaveMenu.SetActive(false);
    }

}
