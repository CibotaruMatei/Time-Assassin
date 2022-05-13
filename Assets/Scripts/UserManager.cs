using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void SwitchFocus(BoardManager board) {
        // TODO: Calling this function to pick the board for the next turn after making your turn
        focusBoard = board;
        board.ChangeFocus(player);
    }
    
    [SerializeField]
    bool player = true;
    GameManager gm;
    public int remainingClones = 4;
    public BoardManager focusBoard;
}
