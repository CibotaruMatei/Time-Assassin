using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Value
{
    Empty = 0,
    Player = 1,
    Enemy = 2
}
public class Move
{
    public Position from;
    public Position to;

    public Move(int board, int x, int z) {
        this.from.board = board;
        this.from.x = x;
        this.from.z = z;
    }

    public Move(Position from, Position to) {
        this.from = from;
        this.to = to;
    }
}
