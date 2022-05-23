using System.Collections;
using System.Collections.Generic;
using System;
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
    
    public static T MaxValue<T>(List<T> list, Converter<T, int> projection)
    {
        int maxValue = int.MinValue;
        T result = list[0];
        foreach (T item in list)
        {
            int value = projection(item);
            if (value > maxValue)
            {
                maxValue = value;
                result = item;
            }
        }
        return result;
    }
    
    public static T MinValue<T>(List<T> list, Converter<T, int> projection)
    {
        int minValue = int.MaxValue;
        T result = list[0];
        foreach (T item in list)
        {
            int value = projection(item);
            if (value > minValue)
            {
                minValue = value;
                result = item;
            }
        }
        return result;
    }

}