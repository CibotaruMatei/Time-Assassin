using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceController : MonoBehaviour
{    
    GameManager gm;
    AIManager aim;
    public BoardManager bm;
    public Position position;
    public bool player = false; 
    public bool enemy = false;
    public bool target = false;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        aim = GameObject.Find("Enemy").GetComponent<AIManager>();
    }

    public void Init(BoardManager board) {
        bm = board;
        if(player) bm.playerPieces.Add(this);
        else bm.enemyPieces.Add(this);
    }

    void OnDisable() {
        if(player) bm.playerPieces.Remove(this);
        else bm.enemyPieces.Remove(this);
    }

    public void MovePiece(Position other) {
        // Starea e invalida doar atunci cand o piesa e impinsa de pe tabla
        if(!Utilities.IsBounded(other)) {
            // Delete piece
            gm.DeletePiece(position, this);
            return;
        }

        PieceController possiblePiece = gm.GetPiece(other);
        
        if(possiblePiece != null) {
            if(player == possiblePiece.player) {
                // Delete both
                gm.DeletePiece(other, possiblePiece);
                gm.DeletePiece(position, this);
                return;
            }
            // noua locatie: 2other - position
            possiblePiece.MovePiece(other + other - position);
        }

        // add new piece in game
        gm.AddPiece(other, player);
        // delete current piece and remove it only when moving into the future
        if (other.board >= position.board) {
            gm.DeletePiece(position, this);
            return;
        }
        // decrement possible remainingClones
        if (player)
            gm.player.remainingClones--;
        else
            aim.remainingClones--;
    }

    public List<Position> GetMoves() {
        List<Position> moves = new List<Position>();
        // check positions on same board
        for(int i = -1; i < 2; i += 2) {
            Position neighborXpos = position.IncreaseX(i), neighborZpos = position.IncreaseZ(i);

            if(Utilities.IsBounded(neighborXpos)) {
                PieceController neighborX = gm.GetPiece(neighborXpos);
                if(neighborX == null || !neighborX.player || !neighborX.enemy) {
                    moves.Add(neighborXpos); // left/right
                }
            } 
            if(Utilities.IsBounded(neighborZpos)) {
                PieceController neighborZ = gm.GetPiece(neighborZpos);
                if(neighborZ == null || !neighborZ.player || !neighborZ.enemy) {
                    moves.Add(neighborZpos); // down/up
                }
            }
        }

        //check same position on other boards
        Position pastBoardPos = position.IncreaseBoard(-1);
        if(Utilities.IsBounded(pastBoardPos)) {
            PieceController pc = gm.GetPiece(pastBoardPos);
            //going back requires the player to have clones available
            if(pc == null && (player ? gm.player.remainingClones : gm.enemy.remainingClones) > 0) {
                moves.Add(pastBoardPos);
            }
        }
        Position futureBoardPos = position.IncreaseBoard(1);
        if(Utilities.IsBounded(futureBoardPos)) {
            PieceController pc = gm.GetPiece(futureBoardPos);
            if(pc == null) {
                moves.Add(futureBoardPos);
            }
        }

        return moves;
    }

}
