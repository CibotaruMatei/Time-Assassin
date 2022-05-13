using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public void ChangeFocus(bool player) {
        if(player) playerFocus = !playerFocus;
        else enemyFocus = !enemyFocus;

        Material material = highlight.GetComponent<Material>();
        if(playerFocus) {
            if(enemyFocus) {
                material.color = Color.magenta;
            } else {
                material.color = Color.blue;
            }
        } else {
            material.color = Color.red;
        }

        if(playerFocus || enemyFocus) highlight.SetActive(true);
        if(!playerFocus && !enemyFocus) highlight.SetActive(false);
    }

    bool playerFocus = false, enemyFocus = false;
    public GameObject highlight;
    public List<PieceController> playerPieces = new List<PieceController>(), enemyPieces = new List<PieceController>();
}
