using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace PACMAN {
    /// <summary>
    /// This class provides global access to important game constants; what fruits appear on what levels,
    /// relative speeds of everything, timers, etc. Centralizing this data here makes it easy to change
    /// the game settings. It also relieves the data-heavy ghost and gameloop classes from a LOT of definitions.
    /// </summary>
    static class Constants {

        // Dispersion tiles for each ghost
        public static readonly List<Point> ScatterTilesBlinky =  new List<Point> {   new Point(21, 1),   new Point(26, 1),
                                                                            new Point(26, 5),   new Point(21, 5)    
        };
        public static readonly List<Point> ScatterTilesPinky = new List<Point> {   new Point(1, 1),    new Point(6, 1),
                                                                            new Point(6, 5),    new Point(1, 5)     
        };
        public static readonly List<Point> ScatterTilesClyde = new List<Point> {   new Point(6, 23),   new Point(9, 23),
                                                                            new Point(9, 26),   new Point(12, 26),  
                                                                            new Point(12, 29),  new Point(1, 29),
                                                                            new Point(1, 26),   new Point(6, 26)    
        };
        public static readonly List<Point> ScatterTilesInky = new List<Point> {   new Point(18, 23),  new Point(21, 23),
                                                                            new Point(21, 26),  new Point(26, 26),
                                                                            new Point(26, 29),  new Point(15, 29),
                                                                            new Point(15, 26),  new Point(18, 26)
        };

        public static List<Point> ScatterTiles(Ghosts identity) {
            switch (identity) {
                case Ghosts.Blinky:
                    return ScatterTilesBlinky;
                case Ghosts.Clyde:
                    return ScatterTilesClyde;
                case Ghosts.Inky:
                    return ScatterTilesInky;
                case Ghosts.Pinky:
                    return ScatterTilesPinky;
                default:
                    throw new ArgumentException();
            }
        }

        public static readonly Position StartPositionBlinky = new Position { Tile = new Point(13, 11), DeltaPixel = new Point(8, 0) };
        public static readonly Position StartPositionPinky = new Position { Tile = new Point(13, 14), DeltaPixel = new Point(8, 8) };
        public static readonly Position StartPositionInky = new Position { Tile = new Point(11, 13), DeltaPixel = new Point(8, 8) };
        public static readonly Position StartPositionClyde = new Position { Tile = new Point(15, 13), DeltaPixel = new Point(8, 8) };
        public static Position StartPosition(Ghosts identity) {
            switch (identity) {
                case Ghosts.Blinky:
                    return StartPositionBlinky;
                case Ghosts.Pinky:
                    return StartPositionPinky;
                case Ghosts.Clyde:
                    return StartPositionClyde;
                case Ghosts.Inky:
                    return StartPositionInky;
                default:
                    throw new ArgumentException();

            }
        }

        public static int Level = 0;
        public static int InitialJumps(Ghosts ghost, bool newLevel)
        {
            if (newLevel) {
                switch (ghost) {
                    case Ghosts.Inky:
                        return (int)MathHelper.Clamp((20 - Level) / 2, 0, 10);
                    case Ghosts.Clyde:
                        return InitialJumps(Ghosts.Inky, true) + 2;
                    default:
                        return 0;
                }
            }
            switch (ghost) {
                case Ghosts.Inky:
                    return 1;
                case Ghosts.Clyde:
                    return 2;
                default:
                    return 0;
            }
        }

        private static readonly int[] CruiseElroyTimers = { 20, 30, 40, 40, 40, 50, 50, 50, 60, 60 };
        public static int CruiseElroyTimer()
        {
            if (Level >= 10) {
                return CruiseElroyTimers[9];
            }
            return CruiseElroyTimers[Level - 1];
        }

        public static Color Colors(Ghosts identity) {
            switch (identity) {
                case Ghosts.Blinky:
                    return Color.Red;
                case Ghosts.Clyde:
                    return Color.Orange;
                case Ghosts.Inky:
                    return Color.LightSkyBlue;
                case Ghosts.Pinky:
                    return Color.LightPink;
                default:
                    throw new ArgumentException();
            }
        }

        private static readonly int[] BlueTimes = { 6, 6, 4, 3, 2, 6, 2, 2, 1, 5, 2, 1, 1, 3, 1, 1, 0, 1, 0, 0, 0 };
        public static int BlueTime() {
            return Level > BlueTimes.Length - 2 ? 0 : BlueTimes[Level - 1];
        }

        private static readonly int[] bonusScores_ = { 100, 300, 500, 700, 700, 1000, 1000, 2000, 2000, 3000, 3000, 5000, 5000, 5000 };
        public static int BonusScores() {
            return Level > bonusScores_.Length - 2 ? 5000 : bonusScores_[Level - 1];
        }

        private static readonly string[] BonusSprites = { "Cherry", "Strawberry", "Apple", "Bell", "Orange", "Pear", "Pretzel", "Bell", "Banana", "Key", "Key" };
        public static string BonusSprite() {
            return Level > BonusSprites.Length - 2 ? "Key" : BonusSprites[Level - 1];
        }

        private static readonly int[] pacManSpeed_ = { 7, 9, 8, 8, 9 };
        public static int PacManSpeed()
        {
            if (5 <= Level && Level <= 20) {
                return pacManSpeed_[4];
            }
            if (5 > Level) {
                return pacManSpeed_[Level - 1];
            }
            return 10;
        }

        private static readonly int[] ghostSpeed_ = { 13, 11, 12, 12 };
        public static int GhostSpeed() {
            return Level > 4 ? 11 : ghostSpeed_[Level - 1];
        }
    }
}
