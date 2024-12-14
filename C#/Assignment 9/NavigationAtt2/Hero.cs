using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavigationAtt2
{
    public class Hero
    {
        public int Difficulty { get; set; }
        public Attack Attack1 { get; set; } = new Attack();
        public Attack Attack2 { get; set; } = new Attack();
        public Attack Attack3 { get; set; } = new Attack();
        public Attack Attack4 { get; set; } = new Attack();
        public int Floor {  get; set; }
        public int XP { get; set; }
        public int MaxXP { get; set; }
        public int Level { get; set; }
        public int MaxHP { get; set; }
        public int HP { get; set; }
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
