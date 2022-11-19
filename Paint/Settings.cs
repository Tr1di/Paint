using System;
using System.Data;
using System.Drawing;

namespace Paint
{
    public struct Theme
    {
        public static Theme Black => new Theme(Color.FromArgb(20, 20, 20), Color.White);
        public static Theme White => new Theme(Color.White, Color.Black);

        public Color Color { get; set; }
        public Color FontColor { get; set; }

        public Theme(Color color, Color fontColor)
        {
            Color = color;
            FontColor = fontColor;
        }
    }

    public static class Settings
    {
        public static string RU = "ru-RU";
        public static string EN = "en-US";

        public static Theme Theme { get; set; } = Theme.White;
        public static string Language { get; set; } = RU;
    }
}
