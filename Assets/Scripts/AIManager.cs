using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class AIManager
{
    private GameManager gm;
    public AIManager(GameManager gm)
    {
        this.gm = gm;
    }
    public Move AiMove(int clones)
    {
        PieceController[,,] gamePieces = gm.pieces;
        State state = new State(3, false, clones);
        for (int board = 0; board < 3; board++)
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++) {
                    PieceController piece = gamePieces[board, i, j];
                    if (piece == null) {
                        state.tables[board, i, j] = Value.Empty;
                        continue;
                    }

                    if (piece.player) {
                        state.tables[board, i, j] = Value.Player;
                        state.playerPieces.Add(new Position(board, i, j));
                    }
                    else {
                        state.tables[board, i, j] = Value.Enemy;
                        state.aiPieces.Add(new Position(board, i, j));
                    }
                }

        MinMax(state);
        return state.move;
    }

    private State MinMax(State state) {
        if (state.depth == 0)
        {
            state.score = state.GetScore();
            return state;
        }
        
        List<State> moves = state.GetNewStates();

        if (state.player)
            state.nextState = Utilities.MaxValue(moves, x => x.score);
        else
            state.nextState = Utilities.MinValue(moves, x => x.score);
        state.score = state.nextState.score;
        
        return state;
    }
}