namespace NavigationAtt2
{
    public class Hero : Character
    {
        public string FileName { get; set; } = string.Empty;
        public int Difficulty { get; set; }
        public int Floor { get; set; }
        public int XP { get; set; }
        public int MaxXP { get; set; }
        public int Level { get; set; }
        public int MaxHP { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int LookingAtX { get; set; }
        public int LookingAtY { get; set; }
        public bool IsGreenChestOpen { get; set; } = false;
        public bool IsYellowChestOpen { get; set; } = false;
        public bool IsRedChestOpen { get; set; } = false;
        public bool IsPurpleChestOpen { get; set; } = false;
        public List<string> Inventory { get; set; } = new List<string>();
        public char[] CharMap { get; set; } = Array.Empty<char>();
        public Tile[] TileMap { get; set; } = Array.Empty<Tile>();
        public DateTime StartTime { get; set; } = new();
        public DateTime QuitTime { get; set; } = new();
        public char[,] GetCharMap()
        {
            char[,] map = new char[70, 70];

            for (int j = 0; j < 70; j++)
            {
                for (int i = 0; i < 70; i++)
                {
                    map[i, j] = CharMap[j * 70 + i];

                }
            }

            return map;

        }

        public void SetCharMap(char[,] inputMap)
        {
            var width = inputMap.GetLength(0);
            var height = inputMap.GetLength(1);

            char[] oneDimensionMap = new char[4900];

            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    oneDimensionMap[j * width + i] = inputMap[i, j];
                }
            }

            CharMap = oneDimensionMap;
        }

        public Tile[,] GetTileMap()
        {
            Tile[,] map = new Tile[70, 70];

            for (int j = 0; j < 70; j++)
            {
                for (int i = 0; i < 70; i++)
                {
                    map[i, j] = TileMap[j * 70 + i];

                }
            }

            return map;

        }

        public void SetTileMap(Tile[,] inputMap)
        {
            var width = inputMap.GetLength(0);
            var height = inputMap.GetLength(1);

            Tile[] oneDimensionMap = new Tile[4900];

            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    oneDimensionMap[j * width + i] = inputMap[i, j];
                }
            }

            TileMap = oneDimensionMap;
        }

    }
}
