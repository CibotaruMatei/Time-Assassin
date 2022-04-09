using System.Collections;
using System.Collections.Generic;

public class Position {
    public int board;
    public int x;
    public int z;

    public Position(int board, int x, int z) {
        this.board = board;
        this.x = x;
        this.z = z;
    }
    
    public Position IncreaseBoard(int board) { 
        Position p = this;
        p.board += board;
        return p;
    }

    public Position IncreaseX(int x) { 
        Position p = this;
        p.x += x;
        return p;
    }

    public Position IncreaseZ(int z) { 
        Position p = this;
        p.z += z;
        return p;
    }
}