using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject optionsScreen;

    public void StartGame()
    {
        SceneManager.LoadScene("gameScene");
    }

    public void OpenOptions()
    {
        optionsScreen.SetActive(true);
    }


    public void CloseOption()
    {
        optionsScreen.SetActive(false);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
