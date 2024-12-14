using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavigationAtt2
{
    public class Tile
    {
        public string Type { get; set; } = "wall";
        public System.Drawing.Image Image { get; set; } = Properties.Resources.no_wall;
        public string LookAtText = "Wall";
    }

    public class ChestTile : Tile
    {
        public Chest Chest { get; set; } = new();
        public System.Drawing.Image OpenImage { get; set; } = Properties.Resources.no_wall;
    }

    public class PedestalTile : Tile
    {
        public Pedestal Pedestal { get; set; } = new();
    }

    public class DoorTile : Tile
    {
        public Door Door { get; set; } = new();
        public System.Drawing.Image OpenImage { get; set; } = Properties.Resources.no_wall;
    }

    public class FinalDoorTile : Tile
    {
        public FinalDoor Door { get; set; } = new();
        public System.Drawing.Image OpenImage { get; set; } = Properties.Resources.no_wall;
    }
}
