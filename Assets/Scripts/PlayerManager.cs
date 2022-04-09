using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void SwitchFocus(int board) {
        focusBoard = board;
    }
    
    GameManager gm;
    public List<PieceController> pieces = new List<PieceController>();
    int remainingClones = 4;
    public int focusBoard = 0;
}
