using System;
using System.Collections.ObjectModel;

namespace PuzzleGame.Model
{    
    public static class GameSettings
    {
        public static readonly int SnapTolerance = 4000;

        public static Collection<Tuple<int, int>> BoardSizes = new Collection<Tuple<int, int>>
        {
            new Tuple<int, int>(2, 2),
            new Tuple<int, int>(3, 3),
            new Tuple<int, int>(4, 4),
            new Tuple<int, int>(5, 5)
        };

        // Default size 3x3
        public static int SelectedBoardSizeIndex = 1;

        //        public static readonly int DropShadowDepth = 3;
        //        public static readonly Color DropShadowColor = Color.FromArgb(1, 50, 50, 50);

        public static bool IsSoundEnabled = true;
    }
}
