using System;
using System.Windows.Media;
using Color = System.Windows.Media.Color;

public static class ColorHelper
{
    private static readonly Random Random = new Random();

    public static Color GetRandomColor()
    {
        return Color.FromRgb(
            (byte)Random.Next(256),
            (byte)Random.Next(256),
            (byte)Random.Next(256)
        );
    }
} 