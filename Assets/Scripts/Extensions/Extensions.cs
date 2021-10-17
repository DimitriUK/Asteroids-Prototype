using System;

public static class Extensions
{
    public static T GetNextValue<T>(this T src) where T : struct
    {
        T[] EnumArray = (T[])Enum.GetValues(src.GetType());

        int e = Array.IndexOf<T>(EnumArray, src) + 1;

        return (EnumArray.Length == e) ? EnumArray[0] : EnumArray[e];
    }
}