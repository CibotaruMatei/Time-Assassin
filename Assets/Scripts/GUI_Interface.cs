using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUI_Interface : MonoBehaviour
{
    
	private bool isActive;

    void playerTurnMessage() {
        int x = Camera.main.pixelWidth / 2 - 45;
        int y = 0;
        int sy = 30;
        int sx = 90;
        GUI.contentColor = Color.white;
        if(GameObject.Find("GameManager").GetComponent<GameManager>().playerTurn) {
             GUI.Label(new Rect(x, y, sx, sy), "White Turn!");
        }
        else {
             GUI.Label(new Rect(x, y, sx, sy), "Black Turn!");
        }
    }


    void tiltToggle() {
        bool toggle =  GameObject.Find("GameManager").GetComponent<GameManager>().tiltOption;
        bool result = (GUI.Toggle(new Rect(0, 30, 125, 30), toggle, "Toggle Tilt Floor") );
        GameObject.Find("GameManager").GetComponent<GameManager>().tiltOption = result;
    }

    void OnGUI() {

        bool gameFinished = GameObject.Find("GameManager").GetComponent<GameManager>().gameFinished;
        if(!gameFinished) {
            SkyBoxMenu();
            playerTurnMessage();
            tiltToggle();

        }
        
	}

    void SkyBoxMenu() {
        int x = Camera.main.pixelWidth - 300;
        int y = 100;
        int dy = 40;
        int cnt = 0;
        int sx = 300;
        int sy = 30;
        var s = GUI.Button(new Rect(0, 0, 100, 30), "Skybox Menu");
        if (s) {
            if(s) {
                isActive = !isActive;
                // Debug.LogError(Camera.main.pixelWidth);
            }

            if(isActive) {
                if (GUI.Button(new Rect(x, y+dy*cnt++, sx, sy), "Skybox 1 - hubble deep field")) {
                    RenderSettings.skybox = (Material)Resources.Load("Skybox1");
                }
                if (GUI.Button(new Rect(x, y+dy*cnt++, sx, sy), "Skybox 2 - Semi-close galaxy")) {
                    RenderSettings.skybox = (Material)Resources.Load("Skybox2");
                }
                if (GUI.Button(new Rect(x, y+dy*cnt++, sx, sy), "Skybox 3 - Large blueish galaxy")) {
                    RenderSettings.skybox = (Material)Resources.Load("Skybox3");
                }
                if (GUI.Button(new Rect(x, y+dy*cnt++, sx, sy), "Skybox 4 - center large blue galaxy")) {
                    RenderSettings.skybox = (Material)Resources.Load("Skybox4");
                }
                if (GUI.Button(new Rect(x, y+dy*cnt++, sx, sy), "Skybox 5 - Neutral galaxies")) {
                    RenderSettings.skybox = (Material)Resources.Load("Skybox5");
                }
                if (GUI.Button(new Rect(x, y+dy*cnt++, sx, sy), "Skybox 6 - red galaxy")) {
                    RenderSettings.skybox = (Material)Resources.Load("Skybox6");
                }
            }
        }
    }
  
}
