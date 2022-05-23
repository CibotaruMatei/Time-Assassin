using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PauseMenu : MonoBehaviour
{

     GameManager gm;


    public static bool GameIsPaused  = false;

    public GameObject PauseMenuUI;
    public GameObject SimulatorMenuUI;

    // Start is called before the first frame update
    void Start()
    {
        PauseMenuUI.SetActive(false);
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            if(GameIsPaused) {
                Resume();
            }
            else {
                Pause();
            }
        }
    }

    public void Resume() {
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = !GameIsPaused;
    }

    public void BackMenu() {
        PauseMenuUI.SetActive(true);
        SimulatorMenuUI.SetActive(false);
    }

    public void Pause () {
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = !GameIsPaused;
    }

    public void QuitToMainMenu() {
        Time.timeScale = 1f;
        GameIsPaused = !GameIsPaused;
        gm.InitBoards();
        SceneManager.LoadScene("mainMenu");
        
        
    }

    public void OpenSimulatorMenu() {
        PauseMenuUI.SetActive(false);
        SimulatorMenuUI.SetActive(true);
    }

    public void setComputerDifficulty(float dif) { 
        gm.computerDifficulty = (int)dif;
    }

    public void setPlanePowerTilt(float power) {
        gm.tiltPower = (int)power;
    }

    public void sandsOfTimeToggle(float toggle) {
        gm.SandsOfTime = (int)toggle;
    }

    public void StartNewGame() {
        Time.timeScale = 1f;
        GameIsPaused = !GameIsPaused;
        gm.InitBoards();
        SceneManager.LoadScene("gameScene");
    }



    public void TogglePostProcessing(bool toggle) {
        gm.mainCamera.GetComponent<UniversalAdditionalCameraData>().renderPostProcessing = toggle;
    }



}
