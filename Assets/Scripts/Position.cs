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

    public Position(Position o) {
        this.board = o.board;
        this.x = o.x;
        this.z = o.z;
    }
    
    public Position IncreaseBoard(int board) { 
        Position p = new Position(this);
        p.board += board;
        return p;
    }

    public Position IncreaseX(int x) { 
        Position p = new Position(this);
        p.x += x;
        return p;
    }

    public Position IncreaseZ(int z) { 
        Position p = new Position(this);
        p.z += z;
        return p;
    }

    public static Position operator +(Position a, Position b) {
        return new Position(a.board + b.board, a.x + b.x, a.z + b.z);
    }

    public static Position operator -(Position a, Position b) {
        return new Position(a.board - b.board, a.x - b.x, a.z - b.z);
    }
}