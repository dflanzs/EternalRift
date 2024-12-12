using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public GameObject PasueMenu;

    // Start is called before the first frame update

    public void pause()
    {
        // Show Credits Menu
        Time.timeScale = 0f;
        PasueMenu.SetActive(true);
    }

    public void MainGameButton()
    {
        // Show Main Menu
        Time.timeScale = 1f;
        PasueMenu.SetActive(false);
    }
}
