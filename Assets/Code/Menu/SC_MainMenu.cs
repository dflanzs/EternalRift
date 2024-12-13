using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_MainMenu : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject OptionMenu;
    [SerializeField] private PlayerData data;

    // Start is called before the first frame update
    void Start()
    {
        MainMenuButton();
        data.currentCharge = 0;
        GameManager.Instance.hasWeapon = false;
    }

    public void PlayNowButton()
    {
        // Play Now Button has been pressed, here you can initialize your game (For example Load a Scene called GameLevel etc.)
        UnityEngine.SceneManagement.SceneManager.LoadScene("TutorialScene");
    }

    public void OptionButton()
    {
        // Show Credits Menu
        MainMenu.SetActive(false);
        OptionMenu.SetActive(true);
    }

    public void MainMenuButton()
    {
        // Show Main Menu
        MainMenu.SetActive(true);
        OptionMenu.SetActive(false);
    }

    public void QuitButton()
    {
        // Quit Game
        Application.Quit();
    }
}