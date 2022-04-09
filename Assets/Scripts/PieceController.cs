using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void MovePiece(Position other) {
        
    }

    public List<Position> GetMoves() {
        List<Position> moves = new List<Position>();
        // up
        if(Utilities.IsBounded(position.IncreaseZ(1))) moves.Add(position.IncreaseZ(1));
        // down
        if(Utilities.IsBounded(position.IncreaseZ(-1))) moves.Add(position.IncreaseZ(-1));
        // left
        if(Utilities.IsBounded(position.IncreaseX(-1))) moves.Add(position.IncreaseX(-1));
        // right
        if(Utilities.IsBounded(position.IncreaseX(1))) moves.Add(position.IncreaseX(1));
        // past
        if(Utilities.IsBounded(position.IncreaseBoard(-1))) moves.Add(position.IncreaseBoard(-1));
        // future
        if(Utilities.IsBounded(position.IncreaseBoard(1))) moves.Add(position.IncreaseBoard(1));
        return moves;
    }

    GameManager gm;
    public Position position;
    public bool player;
}
