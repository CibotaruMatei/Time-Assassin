using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State {
    public int depth { get; }
    public bool player { get; }
    public List<Position> aiPieces { get; } = new List<Position>();
    public List<Position> playerPieces { get; } = new List<Position>();
    public int clones { get; }
    public Value[ , , ] tables { get; } = new Value[3, 4, 4];
    public Move move;
    public State nextState;
    public int score;

    public State(int depth, bool player, int clones) {
        this.depth = depth;
        this.player = player;
        this.clones = clones;
    }
    
    public State(int depth, bool player, int clones, List<Position> aiPieces, List<Position> playerPieces, Value[ , , ] tables) {
        this.depth = depth;
        this.player = player;
        this.clones = clones;
        this.tables = tables;
        this.aiPieces = aiPieces;
        this.playerPieces = playerPieces;
    }

    public List<State> GetNewStates()
    {
        List<State> newStates = new List<State>();
        foreach (Move move in GetMoves())
        {
            State newState = MakeMove(move);
            newState.move = move;
            newStates.Add(newState);
        }
        
        return newStates;
    }

    public void GetScore()
    {
        List<Position> pieces = aiPieces;
        List<Position> enemyPieces = playerPieces;
        score = 0;
        foreach (Position piece in pieces)
        {
            switch (piece.board)
            {
                case 0: score += 5;
                    break;
                case 1: score += 7;
                    break;
                case 2: score += 10;
                    break;
            }

            foreach (Position neighbor in piece.Neighbors())
            {
                Value val = tables[neighbor.board, neighbor.x, neighbor.z];
                if ((player && val == Value.Enemy) || (!player && val == Value.Player))
                    score -= piece.IsPushBounded(neighbor) ? 2 : 5;
                
                foreach (Position neighbor2 in neighbor.Neighbors())
                {
                    Value val2 = tables[neighbor2.board, neighbor2.x, neighbor2.z];
                    if ((player && val2 == Value.Enemy) || (!player && val2 == Value.Player))
                        score += 2;
                }
            }
        }
    }
    
    private List<Move> GetMoves() {
        List<Move> moves = new List<Move>();
        List<Position> pieces = player ? playerPieces : aiPieces;
        foreach (Position piece in pieces) {
            Value value = tables[piece.board, piece.x, piece.z];
            List<Position> neighbors = piece.Neighbors();
            foreach (Position neighbor in neighbors)
            {
                Value toValue = tables[neighbor.board, neighbor.x, neighbor.z];
                moves.Add(new Move(piece, neighbor));
            }


            if (piece.board < 2) {
                Position futureBoardPos = piece.FutureBoard();
                Value toValue = tables[piece.board + 1, piece.x, piece.z];
                if (Utilities.IsBounded(futureBoardPos) && toValue == Value.Empty) {
                    moves.Add(new Move(piece, futureBoardPos));
                }
            }

            // check if all clones were used
            if (clones > 0 && piece.board > 0) {
                Position pastBoard = piece.PastBoard();
                Value toValue = tables[piece.board - 1, piece.x, piece.z];
                if (Utilities.IsBounded(pastBoard) && toValue == Value.Empty)
                {
                    moves.Add(new Move(piece, pastBoard));
                }
            }
        }

        return moves;
    }

    private State MakeMove(Move move)
    {
        List<Position> newAIPieces = aiPieces.ConvertAll(piece => piece.Clone());
        List<Position> newPlayerPieces = playerPieces.ConvertAll(piece => piece.Clone());

        Value[ , , ] newTables = (Value[ , , ]) tables.Clone();
        
        if (player)
            newPlayerPieces.Add(move.to);
        else
            newAIPieces.Add(move.to);
        newTables[move.to.board, move.to.x, move.to.z] = player ? Value.Player : Value.Enemy;
        
        if (move.to.board >= move.from.board) {
            if (player)
                newPlayerPieces.Remove(move.from);
            else
                newAIPieces.Remove(move.from);
            newTables[move.from.board, move.from.x, move.from.z] = Value.Empty;
        }

        if (!player)
            return new State(depth - 1, !player, clones - 1, newAIPieces, newPlayerPieces, newTables);
        return new State(depth - 1, !player, clones, newAIPieces, newPlayerPieces, newTables);
    }

    private void AddPiece(Position piece) {
        if (player)
            playerPieces.Add(piece);
        else
            aiPieces.Add(piece);
        tables[piece.board, piece.x, piece.z] = player ? Value.Player : Value.Enemy;
    }

    private void RemovePiece(Position piece) {
        if (player)
            playerPieces.Remove(piece);
        else
            aiPieces.Remove(piece);
        tables[piece.board, piece.x, piece.z] = Value.Empty;
    }
}
