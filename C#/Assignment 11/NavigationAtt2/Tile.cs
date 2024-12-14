using System.Text.Json.Serialization;

namespace NavigationAtt2
{
    public class Tile
    {
        public string Type { get; set; } = "wall";
        public string LookAtText { get; set; } = "Wall";
        public int Variation { get; set; } = 0;
        public Pedestal Pedestal { get; set; } = new();
        public Door Door { get; set; } = new();
        public FinalDoor FinalDoor { get; set; } = new();
        public Chest Chest { get; set; } = new();
        [JsonIgnore] public System.Drawing.Image OpenImage { get; set; } = Properties.Resources.no_wall;
        [JsonIgnore] public System.Drawing.Image Image { get; set; } = Properties.Resources.no_wall;
    }
}
