public static class Utilities {
    public static bool IsBounded(int value, int min, int max) {
        return value >= min && value <= max;
    }

    public static bool IsBounded(Position pos) {
        return IsBounded(pos.board, 0, 2) && IsBounded(pos.x, 0, 3) && IsBounded(pos.z, 0, 3);
    }

    public static int Bound(int value, int min, int max) {
        return value < min ? min : (value > max ? max : value);
    }
}