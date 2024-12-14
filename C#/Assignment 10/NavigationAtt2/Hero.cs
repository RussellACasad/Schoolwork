using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavigationAtt2
{
    public class Hero : Character
    {
        public int Difficulty { get; set; }
        public int Floor {  get; set; }
        public int XP { get; set; }
        public int MaxXP { get; set; }
        public int Level { get; set; }
        public int MaxHP { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public bool DidLastWolfFight { get; set; } = true;
        public int ChestsOpened { get; set; }
        public int LookingAtX { get; set; }
        public int LookingAtY { get; set; }
        public Tile[,] LevelMap { get; set; } = { };
        public char[,] CharMap { get; set; } = { };
        public System.Drawing.Image Direction { get; set; } = Properties.Resources.border;
        public System.Drawing.Image Up { get; set; } = Properties.Resources.border;
        public System.Drawing.Image Down { get; set; } = Properties.Resources.border;
        public System.Drawing.Image Left { get; set; } = Properties.Resources.border;
        public System.Drawing.Image Right { get; set; } = Properties.Resources.border;
        public System.Drawing.Image Animation { get; set; } = Properties.Resources.border;

    }
}
