// Russell Casad
// CPT-230-W01
// 2023 FA
using System.Diagnostics;

namespace NavigationAtt2
{
    public partial class Form1 : Form
    {

        // initializing variables
        public static string dir = @"C:\C#\Casad\";
        private int randomTime = 20000;
        private string itemToRemove = "";
        private bool levelUp = false;
        private readonly Stopwatch stopwatch = new();
        private Hero Player = new();
        private ItemList itemList = new ItemList();
        DateTime StartTime = new DateTime();
        DateTime EndTime = new DateTime();
        private bool didLoad = false;
        Tile[,] TileMap = new Tile[70, 70];
        char[,] CharMap = new char[70, 70];

        public System.Drawing.Image Direction = Properties.Resources.border;
        public System.Drawing.Image Up = Properties.Resources.border;
        public System.Drawing.Image Down = Properties.Resources.border;
        public System.Drawing.Image Left = Properties.Resources.border;
        public System.Drawing.Image Right = Properties.Resources.border;
        public System.Drawing.Image Animation = Properties.Resources.border;


        public Form1(Hero character, bool load)
        {
            InitializeComponent();
            bx34.Controls.Add(pbxPlayer);
            pbxPlayer.Location = new Point(0, 0);
            pbxPlayer.BackColor = Color.Transparent; // makes the player picturebox have a transparent background so the floor doesn't look weird
            pbxPlayer.Dock = DockStyle.Fill;
            itemList.MakeList();

            Player = character;

            this.Text = "Dungeon Explorer : " + Player.FileName;

            Up = Properties.Resources.player_up_t;
            Down = Properties.Resources.player_down_t;
            Left = Properties.Resources.player_left_t;
            Right = Properties.Resources.player_right_t;
            Animation = Properties.Resources.Yawnnn;

            if (Player.Name == "Salem")
            {
                Up = Properties.Resources.player_2_up;
                Down = Properties.Resources.player_2_down;
                Left = Properties.Resources.player_2_left;
                Right = Properties.Resources.player_2_right;
                Animation = Properties.Resources.player_2_down;
            }

            if (load)
            {
                lbxInventoryBox.Items.Clear();
                for (var i = 0; i < Player.Inventory.Count; i++)
                {
                    lbxInventoryBox.Items.Add(Player.Inventory[i]);
                }
                didLoad = true;
                Player.Inventory.Clear();
                txtLookingAt.Text = "-";
                Direction = Down;
                timer1.Start();
                stopwatch.Start();
                txtLevel.Text = Player.Floor.ToString();
                pbxPlayer.Image = Down;
                pgbPlayerHP.Maximum = Player.MaxHP;
                pgbXP.Maximum = Player.MaxXP;
                TileMap = LoadImageAssign();
                TextBoxOpen(" ", true);
                InfoBoxUpdate();
                Map(Player.X, Player.Y);
            }
            else
            {
                Start();
            }

        }

        private void Start()
        {

            pgbPlayerHP.Value = Player.HP;
            LoadMap();
            TextBoxOpen(" ", true);
            InfoBoxUpdate();
            ResetMap();

            timer1.Start();
            stopwatch.Start();
            Map(Player.X, Player.Y);

        }

        private bool Fight(string itemToGet)
        {
            TextBoxOpen("", true);
            FightScreen fightScreen = new(Player, Right);
            fightScreen.ShowDialog(this);
            fightScreen.Close();
            InfoBoxUpdate();
            AddToInventory(itemToGet);

            if (Player.HP <= 0)
            {
                Player.HP = 0;
                fightScreen.Close();
                return false;
            }
            TextBoxOpen($"You beat the {fightScreen.Opponent.Name}!\r\n" +
                $"They dropped a {itemToGet}!\r\n\r\n" +
                $"[{itemToGet} Added]\r\n" +
                $"[+{fightScreen.Opponent.XPAward} XP]\r\n");
            fightScreen.Close();
            return true;
        }

        private void LevelUp()
        {
            if (Player.XP > Player.MaxXP) Player.XP -= Player.MaxXP;
            else Player.XP = 0;
            Player.Level++;

            Player.MaxXP += 25;
            Player.MaxHP += 25;

            Player.HP = Player.MaxHP;
            pgbXP.Maximum = Player.MaxXP;
            pgbPlayerHP.Maximum = Player.MaxHP;

            MessageBox.Show($"You leveled up to level {Player.Level}!\r\nYour HP has been restored and increased by 25!");
        } // level up function for when xp = max xp

        private int FindItem(string name)// finds item info in the list of items, for referencing their specific data
        {
            for (int i = 0; i < itemList.Items.Count; i++)
            {
                if (itemList.Items[i].Name == name) return i;
            }
            return -1;
        }

        private string ChestItemPool()// outputs a random item from the items list, for randomizing chest contents
        {

            Random random = new Random();

            int chestItem = random.Next(0, itemList.Items.Count);

            return itemList.Items[chestItem].Name;
        }

        private bool ChangeHealth(int amount, string item, string message) // for add health items
        {
            if (amount >= 0)
            {
                if (Player.HP == Player.MaxHP) // keeps item from being used if health is already 100
                {
                    TextBoxOpen("Health already full!");
                    return false;
                }

                Player.HP += amount; // adds HP
                if (Player.HP > Player.MaxHP) // if HP is over 100, sets it to 100 to prevent the progrssbar from crashing
                {
                    Player.HP = Player.MaxHP;
                }
                InfoBoxUpdate();

                TextBoxOpen(message + $"\r\n\r\n[{item} removed]\r\n[+{amount} HP]"); // tells the user that the health was added
                return true;
            }
            else
            {
                Player.HP += amount;

                if (Player.HP < 0) Player.HP = 0;

                InfoBoxUpdate();

                TextBoxOpen($"{message}\r\n\r\n[{amount} HP]");

                return true;
            }
        }

        private void InfoBoxUpdate()// updates the current hp/xp/level box with new info
        {
            if (Player.XP >= Player.MaxHP) LevelUp();
            pgbPlayerHP.Value = Player.HP; // sets the progressbar to show HP 
            pgbXP.Value = Player.XP; // sets the progressbar to show XP 
            boxHP.Text = $"HP: {Player.HP}/{Player.MaxHP} | XP: {Player.XP}/{Player.MaxXP} | Level: {Player.Level}";
            bxGreen.Image = Player.IsGreenChestOpen ? Properties.Resources.green_chest_open : Properties.Resources.green_chest_closed;
            bxYellow.Image = Player.IsYellowChestOpen ? Properties.Resources.yellow_chest_open : Properties.Resources.yellow_chest_closed;
            bxRed.Image = Player.IsRedChestOpen ? Properties.Resources.red_chest_open : Properties.Resources.red_chest_closed;
            bxPurple.Image = Player.IsPurpleChestOpen ? Properties.Resources.purple_chest_open : Properties.Resources.purple_chest_closed;
            //boxInventory.Text = $"Inventory ({lbxInventoryBox.Items.Count}/{inventorySize})";
        }

        // Map stuff

        private static char[,] LevelSelection() //Randomly generates an array to serve as a map
        {
            bool allConditionsMet = false; // sets variables to ensure a map has all needed assets
            char roomChar = 'r';
            int roomCounter = 0;
            int specialType = 0;
            bool greenPedPlaced = false;
            bool yellowPedPlaced = false;
            bool redPedPlaced = false;
            bool purplePedPlaced = false;
            bool endPlaced = false;

            Random random = new Random();

            char[,] randomMap = new char[10, 10];

            int spawnX = random.Next(1, 8);
            int spawnY = random.Next(1, 8);


            for (int i = 0; i < randomMap.GetLength(0); i++)
            {
                for (int j = 0; j < randomMap.GetLength(1); j++)
                {
                    randomMap[i, j] = ' ';
                }
            }

            randomMap[spawnY, spawnX] = 's';
            char lastRoom = randomMap[spawnY, spawnX];


            while (!allConditionsMet) //keeps adding rooms until all conditions are met, a special room is added every 3 rooms so it doesn't load forever
            {
                int nextDirection = random.Next(0, 4);
                if (nextDirection == 0 && spawnY + 1 < randomMap.GetLength(0) - 2)
                {

                    spawnY += 1;
                }
                else if (nextDirection == 1 && spawnY - 1 > 2)
                {
                    spawnY -= 1;
                }
                else if (nextDirection == 2 && spawnX + 1 < randomMap.GetLength(1) - 2)
                {
                    spawnX += 1;
                }
                else if (nextDirection == 3 && spawnX - 1 > 2)
                {
                    spawnX -= 1;
                }
                else continue;

                if (randomMap[spawnY, spawnX] != ' ') continue;


                if (lastRoom == '-') { roomChar = '!'; lastRoom = randomMap[spawnY, spawnX]; }
                else if (lastRoom == '=') { roomChar = '@'; lastRoom = randomMap[spawnY, spawnX]; }
                else if (lastRoom == '[') { roomChar = '#'; lastRoom = randomMap[spawnY, spawnX]; }
                else if (lastRoom == ']') { roomChar = '$'; lastRoom = randomMap[spawnY, spawnX]; }
                else
                {
                    int roomCharInt = random.Next(0, 6);
                    if (roomCharInt == 0 && roomCounter < 2)
                    {
                        roomChar = 'h';
                        roomCounter++;
                        lastRoom = 'h';
                    }
                    else
                    {
                        int roomVarient = random.Next(0, 6);
                        if (roomVarient == 0 && roomCounter < 2)
                        {
                            roomChar = 'r';
                            roomCounter++;
                            lastRoom = 'r';
                        }
                        else
                        {
                            if (specialType == 0 && !greenPedPlaced)
                            {
                                greenPedPlaced = true;
                                specialType += 1;
                                roomCounter = 0;
                                roomChar = '-';
                                lastRoom = '-';
                            }
                            else if (specialType == 1 && !yellowPedPlaced)
                            {
                                yellowPedPlaced = true;
                                specialType += 1;
                                roomCounter = 0;
                                roomChar = '=';
                                lastRoom = '=';
                            }
                            else if (specialType == 2 && !redPedPlaced)
                            {
                                redPedPlaced = true;
                                specialType += 1;
                                roomCounter = 0;
                                roomChar = '[';
                                lastRoom = '[';
                            }
                            else if (specialType == 3 && !purplePedPlaced)
                            {
                                purplePedPlaced = true;
                                specialType += 1;
                                roomCounter = 0;
                                roomChar = ']';
                                lastRoom = ']';
                            }
                            else if (specialType == 4 && !endPlaced)
                            {
                                endPlaced = true;
                                roomCounter = 0;
                                roomChar = 'e';
                                lastRoom = 'e';
                            }
                        }
                    }
                }

                if (greenPedPlaced && yellowPedPlaced && redPedPlaced && purplePedPlaced && endPlaced) allConditionsMet = true;

                randomMap[spawnY, spawnX] = roomChar;

            }

            return randomMap;

        }

        private static char[] RoomStorage(char roomChar, char roomObject, int index, bool up, bool down, bool left, bool right)// this stores all the room prefabs
        {
            char[,] room =
            {
                {' ', ' ', 'd', 'd', 'd', ' ', ' ' }, // each type of room prefab stored in a 2D array
                {' ', '1', '1', '1', '1', '1', ' ' }, // 1 = floor
                {'l', '1', '1', '1', '1', '1', 'r' }, // u, d, l, r = what appears when direction equals up, down, left, or right
                {'l', '1', '1', 'x', '1', '1', 'r' }, // x = if a special item is used in a room, like a pedestal
                {'l', '1', '1', '1', '1', '1', 'r' },
                {' ', '1', '1', '1', '1', '1', ' ' },
                {' ', 't', 'u', 'u', 'u', 't', ' ' }
            };

            char[,] hall =
            {
                {' ', ' ', 'd', 'd', 'd', ' ', ' ' }, // same as room but a hallway
                {' ', ' ', 'd', 'd', 'd', ' ', ' ' },
                {'l', 'l', '1', '1', '1', 'r', 'r' },
                {'l', 'l', '1', '1', '1', 'r', 'r' },
                {'l', 'l', '1', '1', '1', 'r', 'r' },
                {' ', ' ', 'u', 'u', 'u', ' ', ' ' },
                {' ', ' ', 'u', 'u', 'u', ' ', ' ' }
            };


            char[,] chestRoom =
            {
                {' ', ' ', ' ', '8', ' ', ' ', ' ' }, // room with chests
                {' ', ' ', ' ', 'd', ' ', ' ', ' ' }, // 7, 8, 9, 0 are like u, d, l, r but for doors instead of floors
                {' ', ' ', '1', '1', '1', ' ', ' ' },
                {'9', 'l', '1', 'x', '1', 'r', '0' },
                {' ', ' ', '1', '1', '1', ' ', ' ' },
                {' ', ' ', 't', 'u', 't', ' ', ' ' },
                {' ', ' ', ' ', '7', ' ', ' ', ' ' }
            };

            char[,] noRoom =
            {
                {' ', ' ', ' ', ' ', ' ', ' ', ' ' }, // no room
                {' ', ' ', ' ', ' ', ' ', ' ', ' ' },
                {' ', ' ', ' ', ' ', ' ', ' ', ' ' },
                {' ', ' ', ' ', ' ', ' ', ' ', ' ' },
                {' ', ' ', ' ', ' ', ' ', ' ', ' ' },
                {' ', ' ', ' ', ' ', ' ', ' ', ' ' },
                {' ', ' ', ' ', ' ', ' ', ' ', ' ' }
            };


            char[,] returnRoom; // makes a 2D array

            if (roomChar == 'r') // sets the return room to the correct room from the big map to be translated into a map array the player can walk in
            {
                returnRoom = room;
            }
            else if (roomChar == 'h')
            {
                returnRoom = hall;
            }
            else if (roomChar == '!' || roomChar == '@' || roomChar == '#' || roomChar == '$' || roomChar == 'e' || roomChar == 'f')
            {
                returnRoom = chestRoom;
            }
            else
            {
                returnRoom = noRoom;
            }


            char[] returnArray = new char[7]; // makes a 7 wide array which will be returned



            for (int i = 0; i < returnRoom.GetLength(0); i++) // translates every part of the array from big map terms to player map terms
            {

                if (returnRoom[index, i] == 'u' || returnRoom[index, i] == 'd' || returnRoom[index, i] == 'l' || returnRoom[index, i] == 'r' || returnRoom[index, i] == 'x' || returnRoom[index, i] == '7' || returnRoom[index, i] == '8' || returnRoom[index, i] == '9' || returnRoom[index, i] == '0')
                {
                    if (returnRoom[index, i] == 'u' && up) { returnArray[i] = '1'; continue; } // these next 8 if statements replace udlr with floors or no floors if there's a room im that direction or not
                    if (returnRoom[index, i] == 'd' && down) { returnArray[i] = '1'; continue; }
                    if (returnRoom[index, i] == 'l' && left) { returnArray[i] = '1'; continue; }
                    if (returnRoom[index, i] == 'r' && right) { returnArray[i] = '1'; continue; }

                    if (returnRoom[index, i] == 'u' && !up) { returnArray[i] = ' '; continue; }
                    if (returnRoom[index, i] == 'd' && !down) { returnArray[i] = ' '; continue; }
                    if (returnRoom[index, i] == 'l' && !left) { returnArray[i] = ' '; continue; }
                    if (returnRoom[index, i] == 'r' && !right) { returnArray[i] = ' '; continue; }

                    if (returnRoom[index, i] == '7' && up && roomChar == '!') { returnArray[i] = '%'; continue; } // does the same as the last 8 if statements, but for doors
                    else returnArray[i] = ' ';
                    if (returnRoom[index, i] == '8' && down && roomChar == '!') { returnArray[i] = '%'; continue; }
                    else returnArray[i] = ' ';
                    if (returnRoom[index, i] == '9' && left && roomChar == '!') { returnArray[i] = '%'; continue; }
                    else returnArray[i] = ' ';
                    if (returnRoom[index, i] == '0' && right && roomChar == '!') { returnArray[i] = '%'; continue; }
                    else returnArray[i] = ' ';

                    if (returnRoom[index, i] == '7' && up && roomChar == '@') { returnArray[i] = '^'; continue; }
                    else returnArray[i] = ' ';
                    if (returnRoom[index, i] == '8' && down && roomChar == '@') { returnArray[i] = '^'; continue; }
                    else returnArray[i] = ' ';
                    if (returnRoom[index, i] == '9' && left && roomChar == '@') { returnArray[i] = '^'; continue; }
                    else returnArray[i] = ' ';
                    if (returnRoom[index, i] == '0' && right && roomChar == '@') { returnArray[i] = '^'; continue; }
                    else returnArray[i] = ' ';

                    if (returnRoom[index, i] == '7' && up && roomChar == '#') { returnArray[i] = '&'; continue; }
                    else returnArray[i] = ' ';
                    if (returnRoom[index, i] == '8' && down && roomChar == '#') { returnArray[i] = '&'; continue; }
                    else returnArray[i] = ' ';
                    if (returnRoom[index, i] == '9' && left && roomChar == '#') { returnArray[i] = '&'; continue; }
                    else returnArray[i] = ' ';
                    if (returnRoom[index, i] == '0' && right && roomChar == '#') { returnArray[i] = '&'; continue; }
                    else returnArray[i] = ' ';

                    if (returnRoom[index, i] == '7' && up && roomChar == '$') { returnArray[i] = '*'; continue; }
                    else returnArray[i] = ' ';
                    if (returnRoom[index, i] == '8' && down && roomChar == '$') { returnArray[i] = '*'; continue; }
                    else returnArray[i] = ' ';
                    if (returnRoom[index, i] == '9' && left && roomChar == '$') { returnArray[i] = '*'; continue; }
                    else returnArray[i] = ' ';
                    if (returnRoom[index, i] == '0' && right && roomChar == '$') { returnArray[i] = '*'; continue; }
                    else returnArray[i] = ' ';

                    if (returnRoom[index, i] == '7' && up && roomChar == 'e' || roomChar == 'f') { returnArray[i] = '+'; continue; }
                    else returnArray[i] = ' ';
                    if (returnRoom[index, i] == '8' && down && roomChar == 'e' || roomChar == 'f') { returnArray[i] = '+'; continue; }
                    else returnArray[i] = ' ';
                    if (returnRoom[index, i] == '9' && left && roomChar == 'e' || roomChar == 'f') { returnArray[i] = '+'; continue; }
                    else returnArray[i] = ' ';
                    if (returnRoom[index, i] == '0' && right && roomChar == 'e' || roomChar == 'f') { returnArray[i] = '+'; continue; }
                    else returnArray[i] = ' ';

                    if (roomObject != 'h' && roomObject != 'r') // replaces the X with a special item if needbe, otherwise, makes it a floor tile
                    {
                        if (returnRoom[index, i] == 'x') { returnArray[i] = roomObject; continue; }
                    }
                    else { returnArray[i] = '1'; continue; }
                }
                else returnArray[i] = returnRoom[index, i];
            }

            return returnArray; //returns the room array 1 line at a time
        }

        private void CharMapMaker() // this translates the small map into a big map that the player can walk on
        {
            char[,] internalMap = LevelSelection(); // gets the small map array
            char[,] playerMap = new char[internalMap.GetLength(0) * 7, internalMap.GetLength(1) * 7]; // makes an array the size of the player map
            List<char> mapList = new(); // makes a list to store the player map in 1 dimension

            for (var i = 0; i < internalMap.GetLength(0); i++) // runs through every slot in the small map, and adds each room into a big list
            {
                for (var j = 0; j < internalMap.GetLength(1); j++)
                {
                    for (var k = 0; k < 7; k++)
                    {
                        bool up = false, down = false, left = false, right = false;

                        if (i - 1 >= 0 && j - 1 >= 0 && i < internalMap.GetLength(0) && j < internalMap.GetLength(1) && internalMap[i, j] != ' ')
                        {
                            up = internalMap[i + 1, j] != ' ' ? true : false;
                            down = internalMap[i - 1, j] != ' ' ? true : false;
                            left = internalMap[i, j - 1] != ' ' ? true : false;
                            right = internalMap[i, j + 1] != ' ' ? true : false;
                        }
                        char[] toAdd;

                        if (internalMap[i, j] != 'h' && internalMap[i, j] != 'r' && internalMap[i, j] != ' ' && internalMap[i, j] != '!' && internalMap[i, j] != '@' && internalMap[i, j] != '#' && internalMap[i, j] != '$' && internalMap[i, j] != 'e' && internalMap[i, j] != 'f')
                        {
                            toAdd = RoomStorage('r', internalMap[i, j], k, up, down, left, right);
                        }
                        else
                        {
                            toAdd = RoomStorage(internalMap[i, j], internalMap[i, j], k, up, down, left, right);
                        }

                        for (var l = 0; l < toAdd.Length; l++)
                        {
                            mapList.Add(toAdd[l]);
                        }
                    }
                }
            }

            var mapListValue = 0;

            for (var k = 0; k < internalMap.GetLength(0); k++) // runs through the list and translates it into a 2D map
            {
                for (var l = 0; l < internalMap.GetLength(1); l++)
                {
                    for (var m = 0; m < 7; m++)
                    {
                        for (var n = 0; n < 7; n++)
                        {
                            var newK = k * 7;
                            var newL = l * 7;
                            playerMap[newK + m, newL + n] = mapList[mapListValue];

                            if (mapListValue >= mapList.Count) break;
                            else mapListValue++;
                        }
                    }
                }
            }

            Player.SetCharMap(playerMap);
            CharMap = playerMap;
        }

        private void MapMaker(bool load = false)
        {

            if (!load)
            {
                CharMapMaker();
            }
            else
            {
                CharMap = Player.GetCharMap();
            }


            CharMap = Player.GetCharMap();

            Tile[,] map = new Tile[CharMap.GetLength(0), CharMap.GetLength(1)];

            for (var y = 0; y < CharMap.GetLength(0); y++)
            {
                for (var x = 0; x < CharMap.GetLength(1); x++)
                {
                    char tile = CharMap[y, x];

                    if (tile == 's')
                    {
                        Player.X = x;
                        Player.Y = y;

                        map[y, x] = new Tile()
                        {
                            Type = "floor",
                            LookAtText = "-",
                            Image = MapRooms(x, y)
                        };

                        continue;
                    }

                    if (tile == ' ')
                    {
                        map[y, x] = new Tile()
                        {
                            Image = MapRooms(x, y)
                        };
                        continue;
                    }

                    if (tile == '1')
                    {

                        Random random = new Random();
                        var i = random.Next(1, 101);
                        var image = Properties.Resources.floor;
                        var variation = 0;

                        if (i >= 81 && i <= 85)
                        {
                            image = Properties.Resources.floor_1;
                            variation = 1;
                        }
                        else if (i >= 86 && i <= 90)
                        {
                            image = Properties.Resources.floor_2;
                            variation = 2;
                        }
                        else if (i >= 91 && i <= 95)
                        {
                            image = Properties.Resources.floor_3;
                            variation = 3;
                        }
                        else if (i >= 96 && i <= 100)
                        {
                            image = Properties.Resources.floor_4;
                            variation = 4;
                        }


                        map[y, x] = new Tile()
                        {
                            Type = "floor",
                            LookAtText = "-",
                            Image = image,
                            Variation = variation
                        };
                        continue;
                    }

                    if (tile == 't')
                    {
                        map[y, x] = new Tile()
                        {
                            Type = "wall",
                            LookAtText = "Torch",
                            Image = MapRooms(x, y)
                        };
                        continue;
                    }

                    if (tile == '!')
                    {
                        map[y, x] = new Tile()
                        {
                            Type = "chest",
                            LookAtText = "Green Chest",
                            Image = MapRooms(x, y),
                            OpenImage = MapRooms(x, y, true),
                            Chest = new Chest()
                            {
                                Contents = ChestItemPool(),
                                UnlockItem = "Green Key"
                            }
                        };
                        continue;
                    }

                    if (tile == '@')
                    {
                        map[y, x] = new Tile()
                        {
                            Type = "chest",
                            LookAtText = "Yellow Chest",
                            Image = MapRooms(x, y),
                            OpenImage = MapRooms(x, y, true),
                            Chest = new Chest()
                            {
                                Contents = ChestItemPool(),
                                UnlockItem = "Yellow Key"
                            }
                        };
                        continue;
                    }

                    if (tile == '#')
                    {
                        map[y, x] = new Tile()
                        {
                            Type = "chest",
                            LookAtText = "Red Chest",
                            Image = MapRooms(x, y),
                            OpenImage = MapRooms(x, y, true),
                            Chest = new Chest()
                            {
                                Contents = ChestItemPool(),
                                UnlockItem = "Red Key"
                            }
                        };
                        continue;
                    }

                    if (tile == '$')
                    {
                        map[y, x] = new Tile()
                        {
                            Type = "chest",
                            LookAtText = "Purple Chest",
                            Image = MapRooms(x, y),
                            OpenImage = MapRooms(x, y, true),
                            Chest = new Chest()
                            {
                                Contents = ChestItemPool(),
                                UnlockItem = "Purple Key"
                            }
                        };
                        continue;
                    }

                    if (tile == '%')
                    {
                        map[y, x] = new Tile()
                        {
                            Type = "door",
                            LookAtText = "Green Door",
                            Image = MapRooms(x, y),
                            OpenImage = MapRooms(x, y, true),
                            Door = new Door()
                            {
                                UnlockItem = "Green Key"
                            }
                        };
                        continue;
                    }

                    if (tile == '^')
                    {
                        map[y, x] = new Tile()
                        {
                            Type = "door",
                            LookAtText = "Yellow Door",
                            Image = MapRooms(x, y),
                            OpenImage = MapRooms(x, y, true),
                            Door = new Door()
                            {
                                UnlockItem = "Yellow Key"
                            }
                        };
                        continue;
                    }

                    if (tile == '&')
                    {
                        map[y, x] = new Tile()
                        {
                            Type = "door",
                            LookAtText = "Red Door",
                            Image = MapRooms(x, y),
                            OpenImage = MapRooms(x, y, true),
                            Door = new Door()
                            {
                                UnlockItem = "Red Key"
                            }
                        };
                        continue;
                    }

                    if (tile == '*')
                    {
                        map[y, x] = new Tile()
                        {
                            Type = "door",
                            LookAtText = "Purple Door",
                            Image = MapRooms(x, y),
                            OpenImage = MapRooms(x, y, true),
                            Door = new Door()
                            {
                                UnlockItem = "Purple Key"
                            }
                        };
                        continue;
                    }

                    if (tile == '+')
                    {
                        map[y, x] = new Tile()
                        {
                            Type = "finaldoor",
                            LookAtText = "Stairwell Door",
                            Image = MapRooms(x, y),
                            OpenImage = MapRooms(x, y, true),
                            FinalDoor = new FinalDoor()
                            {
                                Item1 = "Green Key",
                                Item2 = "Yellow Key",
                                Item3 = "Red Key",
                                Item4 = "Purple Key"
                            }
                        };
                        continue;
                    }

                    if (tile == '-')
                    {
                        map[y, x] = new Tile()
                        {
                            Type = "pedestal",
                            LookAtText = "Green Pedestal",
                            Image = MapRooms(x, y),
                            Pedestal = new Pedestal()
                            {
                                Color = "Green",
                                Item = "Green Key"
                            }
                        };
                        continue;
                    }

                    if (tile == '=')
                    {
                        map[y, x] = new Tile()
                        {
                            Type = "pedestal",
                            LookAtText = "Yellow Pedestal",
                            Image = MapRooms(x, y),
                            Pedestal = new Pedestal()
                            {
                                Color = "Yellow",
                                Item = "Yellow Key"
                            }
                        };
                        continue;
                    }

                    if (tile == '[')
                    {
                        map[y, x] = new Tile()
                        {
                            Type = "pedestal",
                            LookAtText = "Red Pedestal",
                            Image = MapRooms(x, y),
                            Pedestal = new Pedestal()
                            {
                                Color = "Red",
                                Item = "Red Key"
                            }
                        };
                        continue;
                    }

                    if (tile == ']')
                    {
                        map[y, x] = new Tile()
                        {
                            Type = "pedestal",
                            LookAtText = "Purple Pedestal",
                            Image = MapRooms(x, y),
                            Pedestal = new Pedestal()
                            {
                                Color = "Purple",
                                Item = "Purple Key"
                            }
                        };
                        continue;
                    }
                    else
                    {
                        if (tile == 'e')
                        {
                            Random random = new Random();
                            var i = random.Next(0, 2);
                            var image = i switch
                            {
                                0 => Properties.Resources.down_l,
                                1 => Properties.Resources.down_r,
                                _ => Properties.Resources.down_l
                            };
                            map[y, x] = new Tile()
                            {
                                Image = image,
                                LookAtText = "Stairwell"
                            };
                            continue;
                        }
                    }
                }
            }

            TileMap = map;
            Player.SetTileMap(map);


        } // this translates the big map into objects of tiles that can be more refined to work independently

        private void LoadMap() // loads the map array into a public array, so it doesn't have to translate the small array to big every time a player moves
        {

            //if (Player.Floor > 1)
            //{
            //    EndTime = DateTime.Now;

            //    TimeSpan time = (EndTime - StartTime);

            //    if (didLoad)
            //    {
            //        time += (Player.StartTime - Player.QuitTime);
            //        didLoad = false;
            //    }

            //    TextBoxOpen($"Time to complete last dungeon:\r\n\r\n[{time:mm}:{time:ss}]");
            //}

            TextBoxOpen($"Congratulations!\r\nYou beat floor {Player.Floor - 1}!");
            MapMaker();
            ResetMap();
            txtLevel.Text = Player.Floor.ToString();
            StartTime = DateTime.Now;
            Map(Player.X, Player.Y);
        }

        private bool IsFloor(int X, int Y) // a part of the map maker, returns true if a floor tile is at a given coord, false if not.
        {
            if (CharMap[Y, X] == '1' || CharMap[Y, X] == 's' || CharMap[Y, X] == 'c' || CharMap[Y, X] == '!' || CharMap[Y, X] == '@' || CharMap[Y, X] == '#' || CharMap[Y, X] == '$' || CharMap[Y, X] == '%' || CharMap[Y, X] == '^' || CharMap[Y, X] == '&' || CharMap[Y, X] == '*' || CharMap[Y, X] == '-' || CharMap[Y, X] == '=' || CharMap[Y, X] == '[' || CharMap[Y, X] == ']' || CharMap[Y, X] == '+')
            {
                return true;
            }

            else return false;
        }

        private Tile[,] LoadImageAssign()
        {
            var map = Player.GetTileMap();
            CharMap = Player.GetCharMap();

            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    map[i, j].Image = MapRooms(j, i);

                    if (map[i, j].Type == "floor")
                    {
                        switch (map[i, j].Variation)
                        {
                            case 0: map[i, j].Image = Properties.Resources.floor; break;
                            case 1: map[i, j].Image = Properties.Resources.floor_1; break;
                            case 2: map[i, j].Image = Properties.Resources.floor_2; break;
                            case 3: map[i, j].Image = Properties.Resources.floor_3; break;
                            case 4: map[i, j].Image = Properties.Resources.floor_4; break;
                        }
                    }

                    if (map[i, j].Type == "door" || map[i, j].Type == "finaldoor" || map[i, j].Type == "chest")
                    {
                        map[i, j].OpenImage = MapRooms(j, i, true);

                        if (map[i, j].Door.IsUnlocked || map[i, j].FinalDoor.IsUnlocked || map[i, j].Chest.IsOpen)
                        {
                            map[i, j].Image = MapRooms(j, i, true);
                        }

                    }
                }
            }

            pbxPlayer.Image = Down;

            return map;

        }

        private System.Drawing.Image MapRooms(int X, int Y, bool isOpen = false) // the map maker, runs each space in the map array through a bunch of true/false if statements to see what image to apply where.
        {

            if (X < 0 || X >= CharMap.GetLength(1)) return Properties.Resources.no_wall;
            if (Y < 0 || Y >= CharMap.GetLength(0)) return Properties.Resources.no_wall;


            {


                if (CharMap[Y, X] == ' ')
                {
                    if (Y - 1 < 0 && X - 1 < 0)
                    {
                        if (IsFloor(X + 1, Y + 1)) return Properties.Resources.dot_top_right;
                        else return Properties.Resources.no_wall;
                    }

                    else if (Y - 1 < 0 && X + 1 >= CharMap.GetLength(1))
                    {
                        if (IsFloor(X - 1, Y + 1)) return Properties.Resources.dot_top_left;
                        else return Properties.Resources.no_wall;
                    }

                    else if (Y + 1 >= CharMap.GetLength(0) && X - 1 < 0)
                    {
                        if (IsFloor(X + 1, Y - 1)) return Properties.Resources.wall_right;
                        else return Properties.Resources.no_wall;
                    }

                    else if (Y + 1 >= CharMap.GetLength(0) && X + 1 >= CharMap.GetLength(1))
                    {
                        if (IsFloor(X - 1, Y - 1)) return Properties.Resources.wall_left;
                        else return Properties.Resources.no_wall;
                    }

                    else if (Y - 1 < 0)
                    {
                        if (IsFloor(X, Y + 1) == true) return Properties.Resources.wall_top;
                        else if (IsFloor(X + 1, Y + 1)) return Properties.Resources.dot_top_right;
                        else if (IsFloor(X - 1, Y + 1)) return Properties.Resources.dot_top_left;
                        else return Properties.Resources.no_wall;
                    }

                    else if (Y + 1 >= CharMap.GetLength(0))
                    {
                        if (IsFloor(X, Y - 1) == true) return Properties.Resources.wall_bot;
                        else if (IsFloor(X - 1, Y - 1) == true) return Properties.Resources.wall_left;
                        else if (IsFloor(X + 1, Y - 1) == true) return Properties.Resources.wall_right;
                        else return Properties.Resources.no_wall;
                    }

                    else if (X + 1 >= CharMap.GetLength(1))
                    {
                        if (IsFloor(X - 1, Y + 1) == true && IsFloor(X - 1, Y) == false && IsFloor(X, Y + 1) == false) return Properties.Resources.dot_top_left;
                        else if (IsFloor(X, Y + 1) == false && IsFloor(X, Y - 1) == false && (IsFloor(X - 1, Y) == true || IsFloor(X - 1, Y - 1) == true)) return Properties.Resources.wall_left;
                        else return Properties.Resources.no_wall;
                    }

                    else if (X - 1 < 0)
                    {
                        if (IsFloor(X + 1, Y + 1) && !IsFloor(X + 1, Y) && IsFloor(X + 1, Y - 1)) return Properties.Resources.wall_right;
                        else if (IsFloor(X + 1, Y + 1) == true && IsFloor(X + 1, Y) == false && IsFloor(X, Y + 1) == false) return Properties.Resources.dot_top_right;
                        else if (IsFloor(X, Y + 1) == false && IsFloor(X, Y - 1) == false && (IsFloor(X + 1, Y) == true || IsFloor(X + 1, Y - 1) == true)) return Properties.Resources.wall_right;
                        else return Properties.Resources.no_wall;
                    }

                    else if (IsFloor(X - 1, Y + 1) && !IsFloor(X - 1, Y - 1) && !IsFloor(X - 1, Y) && !IsFloor(X, Y + 1) && !IsFloor(X, Y - 1) && IsFloor(X + 1, Y + 1) && !IsFloor(X + 1, Y - 1) && !IsFloor(X + 1, Y)) return Properties.Resources.wall_top;
                    else if (!IsFloor(X - 1, Y + 1) && IsFloor(X - 1, Y - 1) && !IsFloor(X - 1, Y) && !IsFloor(X, Y + 1) && !IsFloor(X, Y - 1) && !IsFloor(X + 1, Y + 1) && IsFloor(X + 1, Y - 1) && !IsFloor(X + 1, Y)) return Properties.Resources.wall_bot;
                    else if (!IsFloor(X + 1, Y) && !IsFloor(X - 1, Y) && IsFloor(X, Y + 1) && IsFloor(X, Y - 1)) return Properties.Resources.wall_bot;
                    else if (!IsFloor(X, Y + 1) && IsFloor(X, Y - 1) && IsFloor(X + 1, Y + 1) && !IsFloor(X + 1, Y) && IsFloor(X - 1, Y) && IsFloor(X - 1, Y - 1)) return Properties.Resources.wall_bot;
                    else if (!IsFloor(X, Y + 1) && IsFloor(X, Y - 1) && IsFloor(X - 1, Y + 1) && !IsFloor(X - 1, Y) && IsFloor(X + 1, Y) && IsFloor(X + 1, Y - 1)) return Properties.Resources.wall_bot;
                    else if (IsFloor(X - 1, Y + 1) && IsFloor(X, Y - 1) && IsFloor(X + 1, Y + 1)) return Properties.Resources.wall_bot;
                    else if (IsFloor(X + 1, Y + 1) == true && IsFloor(X + 1, Y) == true && IsFloor(X, Y + 1) == true && IsFloor(X, Y - 1) == false && IsFloor(X, Y - 1) == false) return Properties.Resources.wall_corner_top_right;
                    else if (IsFloor(X - 1, Y + 1) == true && IsFloor(X - 1, Y) == true && IsFloor(X, Y + 1) == true && IsFloor(X, Y - 1) == false && IsFloor(X, Y - 1) == false) return Properties.Resources.wall_corner_top_left;
                    else if (IsFloor(X + 1, Y + 1) == true && IsFloor(X + 1, Y) == false && IsFloor(X, Y + 1) == false) return Properties.Resources.dot_top_right;
                    else if (IsFloor(X - 1, Y + 1) == true && IsFloor(X - 1, Y) == false && IsFloor(X, Y + 1) == false) return Properties.Resources.dot_top_left;
                    else if (IsFloor(X, Y + 1) == true && IsFloor(X - 1, Y) == false && IsFloor(X + 1, Y) == false) return Properties.Resources.wall_top;
                    else if (IsFloor(X, Y - 1) == true) return Properties.Resources.wall_bot;
                    else if ((IsFloor(X + 1, Y) == true || IsFloor(X + 1, Y - 1)) && IsFloor(X, Y + 1) == false && IsFloor(X, Y - 1) == false) return Properties.Resources.wall_right;
                    else if ((IsFloor(X - 1, Y) == true || IsFloor(X - 1, Y - 1)) && IsFloor(X, Y + 1) == false && IsFloor(X, Y - 1) == false) return Properties.Resources.wall_left;
                    return Properties.Resources.no_wall;
                }

                else if (CharMap[Y, X] == '1' || CharMap[Y, X] == 's') return Properties.Resources.floor;
                else if (isOpen && CharMap[Y, X] == '!') return Properties.Resources.green_chest_open; // chests & doors use special chars
                else if (CharMap[Y, X] == '!') return Properties.Resources.green_chest_closed;
                else if (isOpen && CharMap[Y, X] == '@') return Properties.Resources.yellow_chest_open;
                else if (CharMap[Y, X] == '@') return Properties.Resources.yellow_chest_closed;
                else if (isOpen && CharMap[Y, X] == '#') return Properties.Resources.red_chest_open;
                else if (CharMap[Y, X] == '#') return Properties.Resources.red_chest_closed;
                else if (isOpen && CharMap[Y, X] == '$') return Properties.Resources.purple_chest_open;
                else if (CharMap[Y, X] == '$') return Properties.Resources.purple_chest_closed;


                else if (CharMap[Y, X] == '%' && IsFloor(X, Y + 1) && !IsFloor(X - 1, Y) && !IsFloor(X - 1, Y + 1) && !IsFloor(X + 1, Y) && !IsFloor(X + 1, Y + 1))
                {
                    if (isOpen) return Properties.Resources.green_door_open_up;
                    else return Properties.Resources.green_door_closed_up;
                }

                else if (CharMap[Y, X] == '%' && IsFloor(X, Y - 1) && !IsFloor(X - 1, Y) && !IsFloor(X - 1, Y - 1) && !IsFloor(X + 1, Y) && !IsFloor(X + 1, Y - 1))
                {
                    if (isOpen) return Properties.Resources.green_door_open_up;
                    else return Properties.Resources.green_door_closed_up;
                }

                else if (CharMap[Y, X] == '%' && IsFloor(X - 1, Y) && !IsFloor(X - 1, Y + 1) && !IsFloor(X - 1, Y - 1) && !IsFloor(X, Y + 1) && !IsFloor(X, Y - 1))
                {
                    if (isOpen) return Properties.Resources.green_door_open_right;
                    else return Properties.Resources.green_door_closed_right;
                }

                else if (CharMap[Y, X] == '%' && IsFloor(X + 1, Y) && !IsFloor(X + 1, Y + 1) && !IsFloor(X + 1, Y - 1) && !IsFloor(X, Y + 1) && !IsFloor(X, Y - 1))
                {
                    if (isOpen) return Properties.Resources.green_door_open_left;
                    else return Properties.Resources.green_door_closed_left;
                }


                else if (CharMap[Y, X] == '^' && IsFloor(X, Y + 1) && !IsFloor(X - 1, Y) && !IsFloor(X - 1, Y + 1) && !IsFloor(X + 1, Y) && !IsFloor(X + 1, Y + 1))
                {
                    if (isOpen) return Properties.Resources.yellow_door_open_up;
                    else return Properties.Resources.yellow_door_closed_up;
                }

                else if (CharMap[Y, X] == '^' && IsFloor(X, Y - 1) && !IsFloor(X - 1, Y) && !IsFloor(X - 1, Y - 1) && !IsFloor(X + 1, Y) && !IsFloor(X + 1, Y - 1))
                {
                    if (isOpen) return Properties.Resources.yellow_door_open_up;
                    else return Properties.Resources.yellow_door_closed_up;
                }

                else if (CharMap[Y, X] == '^' && IsFloor(X - 1, Y) && !IsFloor(X - 1, Y + 1) && !IsFloor(X - 1, Y - 1) && !IsFloor(X, Y + 1) && !IsFloor(X, Y - 1))
                {
                    if (isOpen) return Properties.Resources.yellow_door_open_right;
                    else return Properties.Resources.yellow_door_closed_right;
                }

                else if (CharMap[Y, X] == '^' && IsFloor(X + 1, Y) && !IsFloor(X + 1, Y + 1) && !IsFloor(X + 1, Y - 1) && !IsFloor(X, Y + 1) && !IsFloor(X, Y - 1))
                {
                    if (isOpen) return Properties.Resources.yellow_door_open_left;
                    else return Properties.Resources.yellow_door_closed_left;
                }



                else if (CharMap[Y, X] == '&' && IsFloor(X, Y + 1) && !IsFloor(X - 1, Y) && !IsFloor(X - 1, Y + 1) && !IsFloor(X + 1, Y) && !IsFloor(X + 1, Y + 1))
                {
                    if (isOpen) return Properties.Resources.red_door_open_up;
                    else return Properties.Resources.red_door_closed_up;
                }

                else if (CharMap[Y, X] == '&' && IsFloor(X, Y - 1) && !IsFloor(X - 1, Y) && !IsFloor(X - 1, Y - 1) && !IsFloor(X + 1, Y) && !IsFloor(X + 1, Y - 1))
                {
                    if (isOpen) return Properties.Resources.red_door_open_up;
                    else return Properties.Resources.red_door_closed_up;
                }

                else if (CharMap[Y, X] == '&' && IsFloor(X - 1, Y) && !IsFloor(X - 1, Y + 1) && !IsFloor(X - 1, Y - 1) && !IsFloor(X, Y + 1) && !IsFloor(X, Y - 1))
                {
                    if (isOpen) return Properties.Resources.red_door_open_right;
                    else return Properties.Resources.red_door_closed_right;
                }

                else if (CharMap[Y, X] == '&' && IsFloor(X + 1, Y) && !IsFloor(X + 1, Y + 1) && !IsFloor(X + 1, Y - 1) && !IsFloor(X, Y + 1) && !IsFloor(X, Y - 1))
                {
                    if (isOpen) return Properties.Resources.red_door_open_left;
                    else return Properties.Resources.red_door_closed_left;
                }


                else if (CharMap[Y, X] == '*' && IsFloor(X, Y + 1) && !IsFloor(X - 1, Y) && !IsFloor(X - 1, Y + 1) && !IsFloor(X + 1, Y) && !IsFloor(X + 1, Y + 1))
                {
                    if (isOpen) return Properties.Resources.purple_door_open_up;
                    else return Properties.Resources.purple_door_closed_up;
                }

                else if (CharMap[Y, X] == '*' && IsFloor(X, Y - 1) && !IsFloor(X - 1, Y) && !IsFloor(X - 1, Y - 1) && !IsFloor(X + 1, Y) && !IsFloor(X + 1, Y - 1))
                {
                    if (isOpen) return Properties.Resources.purple_door_open_up;
                    else return Properties.Resources.purple_door_closed_up;
                }

                else if (CharMap[Y, X] == '*' && IsFloor(X - 1, Y) && !IsFloor(X - 1, Y + 1) && !IsFloor(X - 1, Y - 1) && !IsFloor(X, Y + 1) && !IsFloor(X, Y - 1))
                {
                    if (isOpen) return Properties.Resources.purple_door_open_right;
                    else return Properties.Resources.purple_door_closed_right;
                }

                else if (CharMap[Y, X] == '*' && IsFloor(X + 1, Y) && !IsFloor(X + 1, Y + 1) && !IsFloor(X + 1, Y - 1) && !IsFloor(X, Y + 1) && !IsFloor(X, Y - 1))
                {
                    if (isOpen) return Properties.Resources.purple_door_open_left;
                    else return Properties.Resources.purple_door_closed_left;
                }


                else if (CharMap[Y, X] == '+' && IsFloor(X, Y + 1) && !IsFloor(X - 1, Y) && !IsFloor(X - 1, Y + 1) && !IsFloor(X + 1, Y) && !IsFloor(X + 1, Y + 1))
                {
                    if (isOpen) return Properties.Resources.brown_door_open_up;
                    else return Properties.Resources.brown_door_closed_up;
                }

                else if (CharMap[Y, X] == '+' && IsFloor(X, Y - 1) && !IsFloor(X - 1, Y) && !IsFloor(X - 1, Y - 1) && !IsFloor(X + 1, Y) && !IsFloor(X + 1, Y - 1))
                {
                    if (isOpen) return Properties.Resources.brown_door_open_up;
                    else return Properties.Resources.brown_door_closed_up;
                }

                else if (CharMap[Y, X] == '+' && IsFloor(X - 1, Y) && !IsFloor(X - 1, Y + 1) && !IsFloor(X - 1, Y - 1) && !IsFloor(X, Y + 1) && !IsFloor(X, Y - 1))
                {
                    if (isOpen) return Properties.Resources.brown_door_open_right;
                    else return Properties.Resources.brown_door_closed_right;
                }

                else if (CharMap[Y, X] == '+' && IsFloor(X + 1, Y) && !IsFloor(X + 1, Y + 1) && !IsFloor(X + 1, Y - 1) && !IsFloor(X, Y + 1) && !IsFloor(X, Y - 1))
                {
                    if (isOpen) return Properties.Resources.brown_door_open_left;
                    else return Properties.Resources.brown_door_closed_left;
                }


                else if (CharMap[Y, X] == '-') return Properties.Resources.green_ped;
                else if (CharMap[Y, X] == '=') return Properties.Resources.Yellow_ped;
                else if (CharMap[Y, X] == '[') return Properties.Resources.red_ped;
                else if (CharMap[Y, X] == ']') return Properties.Resources.purple_ped;

                else if (CharMap[Y, X] == 'c') return Properties.Resources.cactus;
                else if (CharMap[Y, X] == 't') return Properties.Resources.torch_wall;
                else if (CharMap[Y, X] == 'e') return Properties.Resources.down_l;
                else if (CharMap[Y, X] == 'f') return Properties.Resources.down_r;

                else return Properties.Resources.no_wall;// if char not found, it returns this to prevent crashes
            }
        }

        private System.Drawing.Image LoadImage(int X, int Y)
        {
            if (X < 0 || X >= CharMap.GetLength(1)) return Properties.Resources.no_wall;
            if (Y < 0 || Y >= CharMap.GetLength(0)) return Properties.Resources.no_wall;

            return TileMap[Y, X].Image;
        }

        private void Map(int X, int Y) // The map updater, this is what calls the map maker for each square on the map
        {
            bx00.Image = LoadImage(X - 4, Y - 3); // sets each photobox to the proper image for it using x&y coords
            bx01.Image = LoadImage(X - 3, Y - 3);
            bx02.Image = LoadImage(X - 2, Y - 3);
            bx03.Image = LoadImage(X - 1, Y - 3);
            bx04.Image = LoadImage(X + 0, Y - 3);
            bx05.Image = LoadImage(X + 1, Y - 3);
            bx06.Image = LoadImage(X + 2, Y - 3);
            bx07.Image = LoadImage(X + 3, Y - 3);
            bx08.Image = LoadImage(X + 4, Y - 3);

            bx10.Image = LoadImage(X - 4, Y - 2);
            bx11.Image = LoadImage(X - 3, Y - 2);
            bx12.Image = LoadImage(X - 2, Y - 2);
            bx13.Image = LoadImage(X - 1, Y - 2);
            bx14.Image = LoadImage(X + 0, Y - 2);
            bx15.Image = LoadImage(X + 1, Y - 2);
            bx16.Image = LoadImage(X + 2, Y - 2);
            bx17.Image = LoadImage(X + 3, Y - 2);
            bx18.Image = LoadImage(X + 4, Y - 2);

            bx20.Image = LoadImage(X - 4, Y - 1);
            bx21.Image = LoadImage(X - 3, Y - 1);
            bx22.Image = LoadImage(X - 2, Y - 1);
            bx23.Image = LoadImage(X - 1, Y - 1);
            bx24.Image = LoadImage(X + 0, Y - 1);
            bx25.Image = LoadImage(X + 1, Y - 1);
            bx26.Image = LoadImage(X + 2, Y - 1);
            bx27.Image = LoadImage(X + 3, Y - 1);
            bx28.Image = LoadImage(X + 4, Y - 1);

            bx30.Image = LoadImage(X - 4, Y - 0);
            bx31.Image = LoadImage(X - 3, Y - 0);
            bx32.Image = LoadImage(X - 2, Y - 0);
            bx33.Image = LoadImage(X - 1, Y - 0);
            bx34.Image = LoadImage(X, Y);
            bx35.Image = LoadImage(X + 1, Y - 0);
            bx36.Image = LoadImage(X + 2, Y - 0);
            bx37.Image = LoadImage(X + 3, Y - 0);
            bx38.Image = LoadImage(X + 4, Y - 0);

            bx40.Image = LoadImage(X - 4, Y + 1);
            bx41.Image = LoadImage(X - 3, Y + 1);
            bx42.Image = LoadImage(X - 2, Y + 1);
            bx43.Image = LoadImage(X - 1, Y + 1);
            bx44.Image = LoadImage(X + 0, Y + 1);
            bx45.Image = LoadImage(X + 1, Y + 1);
            bx46.Image = LoadImage(X + 2, Y + 1);
            bx47.Image = LoadImage(X + 3, Y + 1);
            bx48.Image = LoadImage(X + 4, Y + 1);

            bx50.Image = LoadImage(X - 4, Y + 2);
            bx51.Image = LoadImage(X - 3, Y + 2);
            bx52.Image = LoadImage(X - 2, Y + 2);
            bx53.Image = LoadImage(X - 1, Y + 2);
            bx54.Image = LoadImage(X + 0, Y + 2);
            bx55.Image = LoadImage(X + 1, Y + 2);
            bx56.Image = LoadImage(X + 2, Y + 2);
            bx57.Image = LoadImage(X + 3, Y + 2);
            bx58.Image = LoadImage(X + 4, Y + 2);

            bx60.Image = LoadImage(X - 4, Y + 3);
            bx61.Image = LoadImage(X - 3, Y + 3);
            bx62.Image = LoadImage(X - 2, Y + 3);
            bx63.Image = LoadImage(X - 1, Y + 3);
            bx64.Image = LoadImage(X + 0, Y + 3);
            bx65.Image = LoadImage(X + 1, Y + 3);
            bx66.Image = LoadImage(X + 2, Y + 3);
            bx67.Image = LoadImage(X + 3, Y + 3);
            bx68.Image = LoadImage(X + 4, Y + 3);

            pbxPlayer.Image = Direction; ;
        }

        private void TextBoxOpen(string text = "", bool hide = false)
        {
            if (hide == false) // shows TextBox
            {
                txtOutput.Clear(); // ensures TextBox is empty
                btnUp.Enabled = false;
                btnDown.Enabled = false;
                btnLeft.Enabled = false;
                btnRight.Enabled = false; //disables direction buttons
                txtOutput.Enabled = true; // makes text box visible
                btnAction.Enabled = false;

                txtOutput.Text = text;

                if (Player.HP > 0)
                {
                    btnBack.Enabled = true;
                }
                else btnBack.Focus();
            }
            else if (hide == true) // hides TextBoxOpen
            {
                btnUp.Enabled = true;
                btnDown.Enabled = true;
                btnLeft.Enabled = true;
                btnRight.Enabled = true;
                btnAction.Enabled = true;
                btnBack.Enabled = false;
                txtOutput.Enabled = false;
                txtOutput.Clear();
                Map(Player.X, Player.Y);
            }

        } // makes a Text Box with any given text, hide is called by the close button only

        private void Back()
        {
            TextBoxOpen("", true);
        } // back button pushed

        //inventory stuff

        private void AddToInventory(string item)
        {
            string toAdd = "- " + item;
            lbxInventoryBox.Items.Add(toAdd);
        } // adds an item to the inventory -- going to add inventory size later and this will keep track of that

        private void RemoveFromInventory(string item = "", bool removeFront = true)
        {
            if (removeFront)
            {
                itemToRemove = item[(item.IndexOf('-') + 2)..];
                lbxInventoryBox.Items.Remove(itemToRemove);
                txtItemInfo.Text = string.Empty;
            }
            else
            {
                itemToRemove = "- " + item;
                lbxInventoryBox.Items.Remove(itemToRemove);
                txtItemInfo.Text = string.Empty;
            }

        }// removes an item from the inventory

        private void ActionEvents()
        {

            var contents = string.Empty;

            if (lbxInventoryBox.SelectedIndex != -1)
            {
                contents = lbxInventoryBox.Items[lbxInventoryBox.SelectedIndex].ToString() ?? "null";
                if (contents != "null")
                {
                    contents = contents.Substring(contents.IndexOf('-') + 2);
                }

                var itemNumber = FindItem(contents);

                if (itemNumber != -1)
                {
                    var itemHealth = itemList.Items[itemNumber].HealthChange;
                    var didAddHealth = ChangeHealth(itemHealth, contents, $"You {itemList.Items[itemNumber].EatOrDrink} the {itemList.Items[itemNumber].Name}. {itemList.Items[itemNumber].EatText}");
                    if (didAddHealth) RemoveFromInventory(contents, false);
                    lbxInventoryBox.SelectedItems.Clear();

                    return;
                }

                if (rbnToss.Checked)
                {
                    if (contents == "Green Key" || contents == "Yellow Key" || contents == "Red Key" || contents == "Purple Key")
                    {
                        TextBoxOpen("You cannot toss a required item!");
                        return;
                    }
                    else
                    {
                        RemoveFromInventory(contents);
                        TextBoxOpen($"{contents} Tossed");
                        return;
                    }
                }
            }

            if (TileMap[Player.LookingAtY, Player.LookingAtX].Type == "door")
            {
                var door = TileMap[Player.LookingAtY, Player.LookingAtX].Door;

                if (contents == door.UnlockItem)
                {
                    if (door.IsUnlocked)
                    {
                        TextBoxOpen($"An open door.");
                        lbxInventoryBox.ClearSelected();
                        return;
                    }
                    else
                    {
                        TextBoxOpen($"You open the {TileMap[Player.LookingAtY, Player.LookingAtX].LookAtText}");
                        door.IsUnlocked = true;
                        TileMap[Player.LookingAtY, Player.LookingAtX].Image = TileMap[Player.LookingAtY, Player.LookingAtX].OpenImage;
                        TileMap[Player.LookingAtY, Player.LookingAtX].LookAtText = "-";
                        Map(Player.X, Player.Y);
                        lbxInventoryBox.ClearSelected();
                        return;
                    }
                }

                if (door.IsUnlocked) TextBoxOpen($"An open door.");
                else TextBoxOpen($"A Locked {TileMap[Player.LookingAtY, Player.LookingAtX].LookAtText}");
                Map(Player.X, Player.Y);
                return;
            }

            if (TileMap[Player.LookingAtY, Player.LookingAtX].Type == "finaldoor")
            {
                var door = TileMap[Player.LookingAtY, Player.LookingAtX].FinalDoor;

                if (door.IsUnlocked)
                {
                    TextBoxOpen($"An open door.");
                    lbxInventoryBox.ClearSelected();
                    return;
                }
                else if (lbxInventoryBox.Items.Contains("- " + door.Item1) && lbxInventoryBox.Items.Contains("- " + door.Item2) &&
                    lbxInventoryBox.Items.Contains("- " + door.Item3) && lbxInventoryBox.Items.Contains("- " + door.Item4))
                {
                    var answer = MessageBox.Show("Are you sure?\r\nYou will lose all your keys!", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (answer == DialogResult.Yes)
                    {
                        door.IsUnlocked = true;
                        TileMap[Player.LookingAtY, Player.LookingAtX].Image = TileMap[Player.LookingAtY, Player.LookingAtX].OpenImage;
                        TileMap[Player.LookingAtY, Player.LookingAtX].LookAtText = "-";
                        lbxInventoryBox.Items.Remove("- " + door.Item1);
                        lbxInventoryBox.Items.Remove("- " + door.Item2);
                        lbxInventoryBox.Items.Remove("- " + door.Item3);
                        lbxInventoryBox.Items.Remove("- " + door.Item4);
                        Map(Player.X, Player.Y);
                    }

                }
                else
                {
                    TextBoxOpen($"The door requires all 4 keys to open.");
                }


                return;
            }


            if (TileMap[Player.LookingAtY, Player.LookingAtX].Type == "chest")
            {
                var chest = TileMap[Player.LookingAtY, Player.LookingAtX].Chest;

                if (contents == chest.UnlockItem)
                {
                    if (chest.IsOpen)
                    {
                        TextBoxOpen($"An Open {TileMap[Player.LookingAtY, Player.LookingAtX].LookAtText}");
                        lbxInventoryBox.ClearSelected();
                        return;
                    }
                    else
                    {
                        TextBoxOpen($"You open the {TileMap[Player.LookingAtY, Player.LookingAtX].LookAtText} and get a {chest.Contents}\r\n\r\n[{chest.Contents} added.]");
                        AddToInventory(chest.Contents);
                        chest.IsOpen = true;
                        TileMap[Player.LookingAtY, Player.LookingAtX].Image = TileMap[Player.LookingAtY, Player.LookingAtX].OpenImage;
                        Map(Player.X, Player.Y);
                        lbxInventoryBox.ClearSelected();

                        if (TileMap[Player.LookingAtY, Player.LookingAtX].LookAtText == "Green Chest")
                        {
                            Player.IsGreenChestOpen = true;
                            InfoBoxUpdate();
                        }
                        else if (TileMap[Player.LookingAtY, Player.LookingAtX].LookAtText == "Yellow Chest")
                        {
                            Player.IsYellowChestOpen = true;
                            InfoBoxUpdate();
                        }
                        else if (TileMap[Player.LookingAtY, Player.LookingAtX].LookAtText == "Red Chest")
                        {
                            Player.IsRedChestOpen = true;
                            InfoBoxUpdate();
                        }
                        else if (TileMap[Player.LookingAtY, Player.LookingAtX].LookAtText == "Purple Chest")
                        {
                            Player.IsPurpleChestOpen = true;
                            InfoBoxUpdate();
                        }

                        return;
                    }

                }

                if (chest.IsOpen) TextBoxOpen($"An Open {TileMap[Player.LookingAtY, Player.LookingAtX].LookAtText}");
                else TextBoxOpen($"A Locked {TileMap[Player.LookingAtY, Player.LookingAtX].LookAtText}");
                Map(Player.X, Player.Y);
                return;
            }


            if (TileMap[Player.LookingAtY, Player.LookingAtX].Type == "pedestal")
            {
                var pedestal = TileMap[Player.LookingAtY, Player.LookingAtX].Pedestal;

                if (pedestal.ConditionMet) TextBoxOpen($"Floor {Player.Floor} {pedestal.Color} Challenge completed");
                else
                {
                    var answer = MessageBox.Show("A Challenge awaits... Do you accept?", $"{pedestal.Color} Challenge", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (answer == DialogResult.Yes)
                    {
                        var didSurvive = Fight(pedestal.Item);
                        if (!didSurvive) { Die(); }
                        else
                        {
                            pedestal.HasGivenItem = true;
                            pedestal.ConditionMet = true;
                        }
                        return;
                    }
                }
            }

            if (txtLookingAt.Text == "Stairwell")
            {
                Player.Floor++;
                LoadMap();
            }

            if (txtLookingAt.Text == "Wall")
            {
                TextBoxOpen("It's a wall.");
            }

            if (txtLookingAt.Text == "cactus")
            {
                ChangeHealth(-50, "", "The cactus stabs you.");
            }

            if (txtLookingAt.Text == "Torch")
            {
                ChangeHealth(-3, "", "You see a torch and go wide-eyed. You touch it immediately.");
            }


        } // events when the action button is pressed

        private void ButtonPushed()
        {
            stopwatch.Restart();
            txtLookingAt.Text = TileMap[Player.LookingAtY, Player.LookingAtX].LookAtText;
            Map(Player.X, Player.Y);
        }

        private void Die()
        {
            timer1.Stop();
            stopwatch.Stop();

            pbxPlayer.Image = Properties.Resources.border;
            Direction = Properties.Resources.border;

            bx00.Image = Properties.Resources.border;
            bx01.Image = Properties.Resources.border;
            bx02.Image = Properties.Resources.border;
            bx03.Image = Properties.Resources.border;
            bx04.Image = Properties.Resources.border;
            bx05.Image = Properties.Resources.border;
            bx06.Image = Properties.Resources.border;
            bx07.Image = Properties.Resources.border;
            bx08.Image = Properties.Resources.border;

            bx10.Image = Properties.Resources.border;
            bx11.Image = Properties.Resources.border;
            bx12.Image = Properties.Resources.border;
            bx13.Image = Properties.Resources.border;
            bx14.Image = Properties.Resources.border;
            bx15.Image = Properties.Resources.border;
            bx16.Image = Properties.Resources.border;
            bx17.Image = Properties.Resources.border;
            bx18.Image = Properties.Resources.border;

            bx20.Image = Properties.Resources.border;
            bx21.Image = Properties.Resources.border;
            bx22.Image = Properties.Resources.border;
            bx23.Image = Properties.Resources.border;
            bx24.Image = Properties.Resources.border;
            bx25.Image = Properties.Resources.border;
            bx26.Image = Properties.Resources.border;
            bx27.Image = Properties.Resources.border;
            bx28.Image = Properties.Resources.border;

            bx30.Image = Properties.Resources.border;
            bx31.Image = Properties.Resources.border;
            bx32.Image = Properties.Resources.border;
            bx33.Image = Properties.Resources.border;
            bx34.Image = Properties.Resources.border;
            bx35.Image = Properties.Resources.border;
            bx36.Image = Properties.Resources.border;
            bx37.Image = Properties.Resources.border;
            bx38.Image = Properties.Resources.border;

            bx40.Image = Properties.Resources.border;
            bx41.Image = Properties.Resources.border;
            bx42.Image = Properties.Resources.border;
            bx43.Image = Properties.Resources.border;
            bx44.Image = Properties.Resources.border;
            bx45.Image = Properties.Resources.border;
            bx46.Image = Properties.Resources.border;
            bx47.Image = Properties.Resources.border;
            bx48.Image = Properties.Resources.border;

            bx50.Image = Properties.Resources.border;
            bx51.Image = Properties.Resources.border;
            bx52.Image = Properties.Resources.border;
            bx53.Image = Properties.Resources.border;
            bx54.Image = Properties.Resources.border;
            bx55.Image = Properties.Resources.border;
            bx56.Image = Properties.Resources.border;
            bx57.Image = Properties.Resources.border;
            bx58.Image = Properties.Resources.border;

            bx60.Image = Properties.Resources.border;
            bx61.Image = Properties.Resources.border;
            bx62.Image = Properties.Resources.border;
            bx63.Image = Properties.Resources.border;
            bx64.Image = Properties.Resources.border;
            bx65.Image = Properties.Resources.border;
            bx66.Image = Properties.Resources.border;
            bx67.Image = Properties.Resources.border;
            bx68.Image = Properties.Resources.border;

            btnUp.Enabled = false;
            btnDown.Enabled = false;
            btnLeft.Enabled = false;
            btnRight.Enabled = false;
            btnAction.Enabled = false;
            btnBack.Enabled = false;

            var answer = MessageBox.Show("You died! Restart?", "You died!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (answer == DialogResult.Yes)
            {
                this.Close();
            }
            else
            {
                System.Windows.Forms.Application.Exit();
            }
        }

        //Event handlers

        private void btnRestart_Click(object sender, EventArgs e) // resets all variables to default state -- also changes button text the first time for better UX
        {
            var answer = MessageBox.Show("Are you sure you want to restart?", "Restart?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (answer == DialogResult.Yes)
            {
                this.Close();
            }
            else
            {
                System.Windows.Forms.Application.Exit();
            }
        }

        private void ResetMap()
        {
            txtLookingAt.Text = "-";
            Direction = Down;
            Player.IsGreenChestOpen = false;
            Player.IsYellowChestOpen = false;
            Player.IsRedChestOpen = false;
            Player.IsPurpleChestOpen = false;
            InfoBoxUpdate();
        }

        private string IsFacing(int X, int Y)
        {
            return TileMap[Y, X].LookAtText;
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            Direction = Up; // Changes player direction
            Player.LookingAtX = Player.X + 0;
            Player.LookingAtY = Player.Y + 1;
            lbxInventoryBox.ClearSelected();
            if (IsFacing(Player.X, Player.Y + 1) == "-")
            {
                Player.Y++;
                Player.LookingAtX = Player.X + 0;
                Player.LookingAtY = Player.Y + 1;
            }
            ButtonPushed();
        } // all direction buttons: changes player direction & detects 

        private void btnDown_Click(object sender, EventArgs e)
        {
            Direction = Down;
            Player.LookingAtX = Player.X + 0;
            Player.LookingAtY = Player.Y - 1;
            lbxInventoryBox.ClearSelected();
            if (IsFacing(Player.X, Player.Y - 1) == "-")
            {
                Player.Y--;
                Player.LookingAtX = Player.X + 0;
                Player.LookingAtY = Player.Y - 1;
            }
            ButtonPushed();
        }

        private void btnLeft_Click(object sender, EventArgs e)
        {
            Direction = Left;
            Player.LookingAtX = Player.X - 1;
            Player.LookingAtY = Player.Y + 0;
            lbxInventoryBox.ClearSelected();
            if (IsFacing(Player.X - 1, Player.Y) == "-")
            {
                Player.X--;
                Player.LookingAtX = Player.X - 1;
                Player.LookingAtY = Player.Y + 0;
            }
            ButtonPushed();
        }

        private void btnRight_Click(object sender, EventArgs e)
        {
            Direction = Right;
            Player.LookingAtX = Player.X + 1;
            Player.LookingAtY = Player.Y + 0;
            lbxInventoryBox.ClearSelected();
            if (IsFacing(Player.X + 1, Player.Y) == "-")
            {
                Player.X++;
                Player.LookingAtX = Player.X + 1;
                Player.LookingAtY = Player.Y + 0;
            }
            ButtonPushed();
        }

        private void btnAction_Click(object sender, EventArgs e)
        {
            ButtonPushed();
            ActionEvents();

        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            ButtonPushed();
            Back();
        }

        private void timer1_Tick(object sender, EventArgs e) // a timer that ticks every half seccond, used for timed events, right now mostly for just making the fox yawn
        {

            Random random = new Random();


            if (stopwatch.ElapsedMilliseconds > randomTime)
            {
                pbxPlayer.Image = Animation;
                randomTime = random.Next(15000, 30001);
                stopwatch.Restart();

            }

            if (stopwatch.ElapsedMilliseconds < 20000 && stopwatch.ElapsedMilliseconds > 1500)
            {
                pbxPlayer.Image = Direction;
            }


            if (Player.HP == 0)
            {

                Die();
            }

        }

        private void Form1_KeyDown(object sender, KeyEventArgs e) // adds keyboard controls to the application, making navigation more accessible
        {
            if (e.KeyCode == Keys.W)
            {

                btnUp.PerformClick();
            }

            if (e.KeyCode == Keys.S)
            {

                btnDown.PerformClick();
            }

            if (e.KeyCode == Keys.A)
            {

                btnLeft.PerformClick();
            }

            if (e.KeyCode == Keys.D)
            {

                btnRight.PerformClick();
            }

            if (e.KeyCode == Keys.E)
            {
                if (btnBack.Enabled) btnBack.PerformClick();
                else btnAction.PerformClick();
            }

            if (e.KeyCode == Keys.Escape)
            {
                btnBack.PerformClick();
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            for (var i = 0; i < lbxInventoryBox.Items.Count; i++)
            {
                Player.Inventory.Add(lbxInventoryBox.Items[i].ToString() ?? "null");
            }

            if (Player.HP > 0)
            {
                var answer = MessageBox.Show($"Would you like to save {Player.FileName}?", "Save??", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (answer == DialogResult.Yes)
                {
                    Player.StartTime = StartTime;
                    Player.QuitTime = DateTime.Now;

                    SaveLoad.Save(Player);
                }
            }
            else
            {
                File.Delete(dir + Player.FileName);
            }
            PlayerSelect newGame = new PlayerSelect();
            newGame.Show();
        }

        private void btnDeselectInventory_Click(object sender, EventArgs e)
        {
            txtItemInfo.Text = string.Empty;
            lbxInventoryBox.SelectedItems.Clear();
        }

        private void lbxInventoryBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string holding;
            if (lbxInventoryBox.SelectedIndex == -1) holding = "null";
            else
            {
                holding = lbxInventoryBox.Items[lbxInventoryBox.SelectedIndex].ToString() ?? "null";

                var contents = holding.Substring(holding.IndexOf('-') + 2);
                var item = FindItem(contents);
                if (item == -1)
                {
                    txtItemInfo.Text = string.Empty;
                    return;
                }

                if (itemList.Items[item].HealthChange >= 0)
                {
                    txtItemInfo.Text = $"Adds {itemList.Items[item].HealthChange} HP";
                }
                else
                {
                    txtItemInfo.Text = $"Removes {itemList.Items[item].HealthChange} HP";
                }


            }
        }
    }
}