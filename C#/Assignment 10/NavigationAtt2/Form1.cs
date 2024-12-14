// Russell Casad
// CPT-230-W01
// 2023 FA
using Microsoft.VisualBasic;
using NavigationAtt2.Properties;
using System;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.IO.Pipes;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using System.Text;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.LinkLabel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace NavigationAtt2
{
    public partial class Form1 : Form
    {

        // initializing variables to detect when things are done

        private int randomTime = 20000;
        private string itemToRemove = "";
        private bool levelUp = false;
        private readonly Stopwatch stopwatch = new();
        private Hero Player = new();
        private ItemList itemList = new ItemList();
        DateTime startTime = new DateTime();
        DateTime endTime = new DateTime();


        public Form1(Hero character, bool didLoad, int fileNum)
        {
            InitializeComponent();
            bx34.Controls.Add(pbxPlayer);
            pbxPlayer.Location = new Point(0, 0);
            //bx34.Controls.Add(txtOutput);
            //txtOutput.Location = new Point(0, 0);
            pbxPlayer.BackColor = Color.Transparent; // makes the player picturebox have a transparent background so the floor doesn't look weird
            pbxPlayer.Dock = DockStyle.Fill;
            itemList.MakeList();
            Player = character;
            Start();
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
            string levelUpText;
            TextBoxOpen("", true);
            FightScreen fightScreen = new(Player);
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

        private static char[,] CharMapMaker() // this translates the small map into a big map that the player can walk on
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

            return playerMap;
        }

        private Tile[,] MapMaker()
        {
            Player.CharMap = CharMapMaker();
            Tile[,] map = new Tile[Player.CharMap.GetLength(0), Player.CharMap.GetLength(1)];

            for (var y = 0; y < Player.CharMap.GetLength(0); y++)
            {
                for (var x = 0; x < Player.CharMap.GetLength(1); x++)
                {
                    char tile = Player.CharMap[y, x];

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

                        if (i >= 81 && i <= 85)
                        {
                            image = Properties.Resources.floor_1;
                        }
                        else if (i >= 86 && i <= 90)
                        {
                            image = Properties.Resources.floor_2;
                        }
                        else if (i >= 91 && i <= 95)
                        {
                            image = Properties.Resources.floor_3;
                        }
                        else if (i >= 96 && i <= 100)
                        {
                            image = Properties.Resources.floor_4;
                        }


                        map[y, x] = new Tile()
                        {
                            Type = "floor",
                            LookAtText = "-",
                            Image = image
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
                        map[y, x] = new ChestTile()
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
                        map[y, x] = new ChestTile()
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
                        map[y, x] = new ChestTile()
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
                        map[y, x] = new ChestTile()
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
                        map[y, x] = new DoorTile()
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
                        map[y, x] = new DoorTile()
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
                        map[y, x] = new DoorTile()
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
                        map[y, x] = new DoorTile()
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
                        map[y, x] = new FinalDoorTile()
                        {
                            Type = "door",
                            LookAtText = "Stairwell Door",
                            Image = MapRooms(x, y),
                            OpenImage = MapRooms(x, y, true),
                            Door = new FinalDoor()
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
                        map[y, x] = new PedestalTile()
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
                        map[y, x] = new PedestalTile()
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
                        map[y, x] = new PedestalTile()
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
                        map[y, x] = new PedestalTile()
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

            return map;

        } // this translates the big map into objects of tiles that can be more refined to work independently

        private void LoadMap() // loads the map array into a public array, so it doesn't have to translate the small array to big every time a player moves
        {

            if (Player.Floor > 1)
            {
                endTime = DateTime.Now;

                TimeSpan time = endTime - startTime;

                TextBoxOpen($"Time to complete last dungeon:\r\n\r\n[{time:mm}:{time:ss}]");
            }

            Player.LevelMap = MapMaker();

            ResetMap();
            txtLevel.Text = Player.Floor.ToString();
            startTime = DateTime.Now;
            Map(Player.X, Player.Y);
        }

        private bool IsFloor(int X, int Y) // a part of the map maker, returns true if a floor tile is at a given coord, false if not.
        {
            if (Player.CharMap[Y, X] == '1' || Player.CharMap[Y, X] == 's' || Player.CharMap[Y, X] == 'c' || Player.CharMap[Y, X] == '!' || Player.CharMap[Y, X] == '@' || Player.CharMap[Y, X] == '#' || Player.CharMap[Y, X] == '$' || Player.CharMap[Y, X] == '%' || Player.CharMap[Y, X] == '^' || Player.CharMap[Y, X] == '&' || Player.CharMap[Y, X] == '*' || Player.CharMap[Y, X] == '-' || Player.CharMap[Y, X] == '=' || Player.CharMap[Y, X] == '[' || Player.CharMap[Y, X] == ']' || Player.CharMap[Y, X] == '+')
            {
                return true;
            }

            else return false;
        }

        private System.Drawing.Image MapRooms(int X, int Y, bool isOpen = false) // the map maker, runs each space in the map array through a bunch of true/false if statements to see what image to apply where.
        {

            if (X < 0 || X >= Player.CharMap.GetLength(1)) return Properties.Resources.no_wall;
            if (Y < 0 || Y >= Player.CharMap.GetLength(0)) return Properties.Resources.no_wall;


            {


                if (Player.CharMap[Y, X] == ' ')
                {
                    if (Y - 1 < 0 && X - 1 < 0)
                    {
                        if (IsFloor(X + 1, Y + 1)) return Properties.Resources.dot_top_right;
                        else return Properties.Resources.no_wall;
                    }

                    else if (Y - 1 < 0 && X + 1 >= Player.CharMap.GetLength(1))
                    {
                        if (IsFloor(X - 1, Y + 1)) return Properties.Resources.dot_top_left;
                        else return Properties.Resources.no_wall;
                    }

                    else if (Y + 1 >= Player.CharMap.GetLength(0) && X - 1 < 0)
                    {
                        if (IsFloor(X + 1, Y - 1)) return Properties.Resources.wall_right;
                        else return Properties.Resources.no_wall;
                    }

                    else if (Y + 1 >= Player.CharMap.GetLength(0) && X + 1 >= Player.CharMap.GetLength(1))
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

                    else if (Y + 1 >= Player.CharMap.GetLength(0))
                    {
                        if (IsFloor(X, Y - 1) == true) return Properties.Resources.wall_bot;
                        else if (IsFloor(X - 1, Y - 1) == true) return Properties.Resources.wall_left;
                        else if (IsFloor(X + 1, Y - 1) == true) return Properties.Resources.wall_right;
                        else return Properties.Resources.no_wall;
                    }

                    else if (X + 1 >= Player.CharMap.GetLength(1))
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

                else if (Player.CharMap[Y, X] == '1' || Player.CharMap[Y, X] == 's') return Properties.Resources.floor;
                else if (isOpen && Player.CharMap[Y, X] == '!') return Properties.Resources.green_chest_open; // chests & doors use special chars
                else if (Player.CharMap[Y, X] == '!') return Properties.Resources.green_chest_closed;
                else if (isOpen && Player.CharMap[Y, X] == '@') return Properties.Resources.yellow_chest_open;
                else if (Player.CharMap[Y, X] == '@') return Properties.Resources.yellow_chest_closed;
                else if (isOpen && Player.CharMap[Y, X] == '#') return Properties.Resources.red_chest_open;
                else if (Player.CharMap[Y, X] == '#') return Properties.Resources.red_chest_closed;
                else if (isOpen && Player.CharMap[Y, X] == '$') return Properties.Resources.purple_chest_open;
                else if (Player.CharMap[Y, X] == '$') return Properties.Resources.purple_chest_closed;


                else if (Player.CharMap[Y, X] == '%' && IsFloor(X, Y + 1) && !IsFloor(X - 1, Y) && !IsFloor(X - 1, Y + 1) && !IsFloor(X + 1, Y) && !IsFloor(X + 1, Y + 1))
                {
                    if (isOpen) return Properties.Resources.green_door_open_up;
                    else return Properties.Resources.green_door_closed_up;
                }

                else if (Player.CharMap[Y, X] == '%' && IsFloor(X, Y - 1) && !IsFloor(X - 1, Y) && !IsFloor(X - 1, Y - 1) && !IsFloor(X + 1, Y) && !IsFloor(X + 1, Y - 1))
                {
                    if (isOpen) return Properties.Resources.green_door_open_up;
                    else return Properties.Resources.green_door_closed_up;
                }

                else if (Player.CharMap[Y, X] == '%' && IsFloor(X - 1, Y) && !IsFloor(X - 1, Y + 1) && !IsFloor(X - 1, Y - 1) && !IsFloor(X, Y + 1) && !IsFloor(X, Y - 1))
                {
                    if (isOpen) return Properties.Resources.green_door_open_right;
                    else return Properties.Resources.green_door_closed_right;
                }

                else if (Player.CharMap[Y, X] == '%' && IsFloor(X + 1, Y) && !IsFloor(X + 1, Y + 1) && !IsFloor(X + 1, Y - 1) && !IsFloor(X, Y + 1) && !IsFloor(X, Y - 1))
                {
                    if (isOpen) return Properties.Resources.green_door_open_left;
                    else return Properties.Resources.green_door_closed_left;
                }


                else if (Player.CharMap[Y, X] == '^' && IsFloor(X, Y + 1) && !IsFloor(X - 1, Y) && !IsFloor(X - 1, Y + 1) && !IsFloor(X + 1, Y) && !IsFloor(X + 1, Y + 1))
                {
                    if (isOpen) return Properties.Resources.yellow_door_open_up;
                    else return Properties.Resources.yellow_door_closed_up;
                }

                else if (Player.CharMap[Y, X] == '^' && IsFloor(X, Y - 1) && !IsFloor(X - 1, Y) && !IsFloor(X - 1, Y - 1) && !IsFloor(X + 1, Y) && !IsFloor(X + 1, Y - 1))
                {
                    if (isOpen) return Properties.Resources.yellow_door_open_up;
                    else return Properties.Resources.yellow_door_closed_up;
                }

                else if (Player.CharMap[Y, X] == '^' && IsFloor(X - 1, Y) && !IsFloor(X - 1, Y + 1) && !IsFloor(X - 1, Y - 1) && !IsFloor(X, Y + 1) && !IsFloor(X, Y - 1))
                {
                    if (isOpen) return Properties.Resources.yellow_door_open_right;
                    else return Properties.Resources.yellow_door_closed_right;
                }

                else if (Player.CharMap[Y, X] == '^' && IsFloor(X + 1, Y) && !IsFloor(X + 1, Y + 1) && !IsFloor(X + 1, Y - 1) && !IsFloor(X, Y + 1) && !IsFloor(X, Y - 1))
                {
                    if (isOpen) return Properties.Resources.yellow_door_open_left;
                    else return Properties.Resources.yellow_door_closed_left;
                }



                else if (Player.CharMap[Y, X] == '&' && IsFloor(X, Y + 1) && !IsFloor(X - 1, Y) && !IsFloor(X - 1, Y + 1) && !IsFloor(X + 1, Y) && !IsFloor(X + 1, Y + 1))
                {
                    if (isOpen) return Properties.Resources.red_door_open_up;
                    else return Properties.Resources.red_door_closed_up;
                }

                else if (Player.CharMap[Y, X] == '&' && IsFloor(X, Y - 1) && !IsFloor(X - 1, Y) && !IsFloor(X - 1, Y - 1) && !IsFloor(X + 1, Y) && !IsFloor(X + 1, Y - 1))
                {
                    if (isOpen) return Properties.Resources.red_door_open_up;
                    else return Properties.Resources.red_door_closed_up;
                }

                else if (Player.CharMap[Y, X] == '&' && IsFloor(X - 1, Y) && !IsFloor(X - 1, Y + 1) && !IsFloor(X - 1, Y - 1) && !IsFloor(X, Y + 1) && !IsFloor(X, Y - 1))
                {
                    if (isOpen) return Properties.Resources.red_door_open_right;
                    else return Properties.Resources.red_door_closed_right;
                }

                else if (Player.CharMap[Y, X] == '&' && IsFloor(X + 1, Y) && !IsFloor(X + 1, Y + 1) && !IsFloor(X + 1, Y - 1) && !IsFloor(X, Y + 1) && !IsFloor(X, Y - 1))
                {
                    if (isOpen) return Properties.Resources.red_door_open_left;
                    else return Properties.Resources.red_door_closed_left;
                }


                else if (Player.CharMap[Y, X] == '*' && IsFloor(X, Y + 1) && !IsFloor(X - 1, Y) && !IsFloor(X - 1, Y + 1) && !IsFloor(X + 1, Y) && !IsFloor(X + 1, Y + 1))
                {
                    if (isOpen) return Properties.Resources.purple_door_open_up;
                    else return Properties.Resources.purple_door_closed_up;
                }

                else if (Player.CharMap[Y, X] == '*' && IsFloor(X, Y - 1) && !IsFloor(X - 1, Y) && !IsFloor(X - 1, Y - 1) && !IsFloor(X + 1, Y) && !IsFloor(X + 1, Y - 1))
                {
                    if (isOpen) return Properties.Resources.purple_door_open_up;
                    else return Properties.Resources.purple_door_closed_up;
                }

                else if (Player.CharMap[Y, X] == '*' && IsFloor(X - 1, Y) && !IsFloor(X - 1, Y + 1) && !IsFloor(X - 1, Y - 1) && !IsFloor(X, Y + 1) && !IsFloor(X, Y - 1))
                {
                    if (isOpen) return Properties.Resources.purple_door_open_right;
                    else return Properties.Resources.purple_door_closed_right;
                }

                else if (Player.CharMap[Y, X] == '*' && IsFloor(X + 1, Y) && !IsFloor(X + 1, Y + 1) && !IsFloor(X + 1, Y - 1) && !IsFloor(X, Y + 1) && !IsFloor(X, Y - 1))
                {
                    if (isOpen) return Properties.Resources.purple_door_open_left;
                    else return Properties.Resources.purple_door_closed_left;
                }


                else if (Player.CharMap[Y, X] == '+' && IsFloor(X, Y + 1) && !IsFloor(X - 1, Y) && !IsFloor(X - 1, Y + 1) && !IsFloor(X + 1, Y) && !IsFloor(X + 1, Y + 1))
                {
                    if (isOpen) return Properties.Resources.brown_door_open_up;
                    else return Properties.Resources.brown_door_closed_up;
                }

                else if (Player.CharMap[Y, X] == '+' && IsFloor(X, Y - 1) && !IsFloor(X - 1, Y) && !IsFloor(X - 1, Y - 1) && !IsFloor(X + 1, Y) && !IsFloor(X + 1, Y - 1))
                {
                    if (isOpen) return Properties.Resources.brown_door_open_up;
                    else return Properties.Resources.brown_door_closed_up;
                }

                else if (Player.CharMap[Y, X] == '+' && IsFloor(X - 1, Y) && !IsFloor(X - 1, Y + 1) && !IsFloor(X - 1, Y - 1) && !IsFloor(X, Y + 1) && !IsFloor(X, Y - 1))
                {
                    if (isOpen) return Properties.Resources.brown_door_open_right;
                    else return Properties.Resources.brown_door_closed_right;
                }

                else if (Player.CharMap[Y, X] == '+' && IsFloor(X + 1, Y) && !IsFloor(X + 1, Y + 1) && !IsFloor(X + 1, Y - 1) && !IsFloor(X, Y + 1) && !IsFloor(X, Y - 1))
                {
                    if (isOpen) return Properties.Resources.brown_door_open_left;
                    else return Properties.Resources.brown_door_closed_left;
                }


                else if (Player.CharMap[Y, X] == '-') return Properties.Resources.green_ped;
                else if (Player.CharMap[Y, X] == '=') return Properties.Resources.Yellow_ped;
                else if (Player.CharMap[Y, X] == '[') return Properties.Resources.red_ped;
                else if (Player.CharMap[Y, X] == ']') return Properties.Resources.purple_ped;

                else if (Player.CharMap[Y, X] == 'c') return Properties.Resources.cactus;
                else if (Player.CharMap[Y, X] == 't') return Properties.Resources.torch_wall;
                else if (Player.CharMap[Y, X] == 'e') return Properties.Resources.down_l;
                else if (Player.CharMap[Y, X] == 'f') return Properties.Resources.down_r;

                else return Properties.Resources.no_wall;// if char not found, it returns this to prevent crashes
            }
        }

        private System.Drawing.Image LoadImage(int X, int Y)
        {
            if (X < 0 || X >= Player.CharMap.GetLength(1)) return Properties.Resources.no_wall;
            if (Y < 0 || Y >= Player.CharMap.GetLength(0)) return Properties.Resources.no_wall;

            return Player.LevelMap[Y, X].Image;
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

            pbxPlayer.Image = Player.Direction;
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

            if (Player.LevelMap[Player.LookingAtY, Player.LookingAtX] is DoorTile doorTile)
            {
                var door = doorTile.Door;

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
                        TextBoxOpen($"You open the {doorTile.LookAtText}");
                        door.IsUnlocked = true;
                        doorTile.Image = doorTile.OpenImage;
                        doorTile.LookAtText = "-";
                        Map(Player.X, Player.Y);
                        lbxInventoryBox.ClearSelected();
                        return;
                    }
                }

                if (door.IsUnlocked) TextBoxOpen($"An open door.");
                else TextBoxOpen($"A Locked {doorTile.LookAtText}");
                Map(Player.X, Player.Y);
                return;
            }

            if (Player.LevelMap[Player.LookingAtY, Player.LookingAtX] is FinalDoorTile finalDoorTile)
            {
                var door = finalDoorTile.Door;

                if (!door.GivenItem1)
                {
                    if (contents == door.Item1)
                    {
                        var answer = MessageBox.Show($"Are you sure?\r\nYou will lose your {contents}!", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (answer == DialogResult.Yes)
                        {
                            TextBoxOpen($"{door.Item1} Accepted.");
                            RemoveFromInventory(contents, false);
                            door.GivenItem1 = true;
                        }
                    }
                    else
                    {
                        TextBoxOpen($"The door requires 4 keys.\r\nPlease give the {door.Item1}");
                    }
                }
                else if (!door.GivenItem2)
                {
                    if (contents == door.Item2)
                    {
                        var answer = MessageBox.Show($"Are you sure?\r\nYou will lose your {contents}!", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (answer == DialogResult.Yes)
                        {
                            TextBoxOpen($"{door.Item2} Accepted.");
                            RemoveFromInventory(contents, false);
                            door.GivenItem2 = true;
                        }
                    }
                    else
                    {
                        TextBoxOpen($"The door requires 4 keys.\r\nPlease give the {door.Item2}");
                    }
                }
                else if (!door.GivenItem3)
                {
                    if (contents == door.Item3)
                    {
                        var answer = MessageBox.Show($"Are you sure?\r\nYou will lose your {contents}!", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (answer == DialogResult.Yes)
                        {
                            TextBoxOpen($"{door.Item3} Accepted.");
                            RemoveFromInventory(contents, false);
                            door.GivenItem3 = true;
                        }
                    }
                    else
                    {
                        TextBoxOpen($"The door requires 4 keys.\r\nPlease give the {door.Item3}");
                    }
                }
                else if (!door.GivenItem4)
                {
                    if (contents == door.Item4)
                    {
                        var answer = MessageBox.Show($"Are you sure?\r\nYou will lose your {contents}!", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (answer == DialogResult.Yes)
                        {
                            TextBoxOpen($"{door.Item4} Accepted.");
                            RemoveFromInventory(contents, false);
                            door.GivenItem4 = true;
                        }
                    }
                    else
                    {
                        TextBoxOpen($"The door requires 4 keys.\r\nPlease give the {door.Item4}");
                    }
                }

                if (door.GivenItem1 && door.GivenItem2 && door.GivenItem3 && door.GivenItem4)
                {
                    door.IsUnlocked = true;
                    finalDoorTile.Image = finalDoorTile.OpenImage;
                    finalDoorTile.LookAtText = "-";
                }
                return;
            }


            if (Player.LevelMap[Player.LookingAtY, Player.LookingAtX] is ChestTile chestTile)
            {
                var chest = chestTile.Chest;

                if (contents == chest.UnlockItem)
                {
                    if (chest.IsOpen)
                    {
                        TextBoxOpen($"An Open {chestTile.LookAtText}");
                        lbxInventoryBox.ClearSelected();
                        return;
                    }
                    else
                    {
                        TextBoxOpen($"You open the {chestTile.LookAtText} and get a {chest.Contents}\r\n\r\n[{chest.Contents} added.]");
                        AddToInventory(chest.Contents);
                        chest.IsOpen = true;
                        chestTile.Image = chestTile.OpenImage;
                        Map(Player.X, Player.Y);
                        lbxInventoryBox.ClearSelected();

                        if (chestTile.LookAtText == "Green Chest")
                        {
                            bxGreen.Image = chestTile.OpenImage;
                        }
                        else if (chestTile.LookAtText == "Yellow Chest")
                        {
                            bxYellow.Image = chestTile.OpenImage;
                        }
                        else if (chestTile.LookAtText == "Red Chest")
                        {
                            bxRed.Image = chestTile.OpenImage;
                        }
                        else if (chestTile.LookAtText == "Purple Chest")
                        {
                            bxPurple.Image = chestTile.OpenImage;
                        }

                        return;
                    }

                }

                if (chest.IsOpen) TextBoxOpen($"An Open {chestTile.LookAtText}");
                else TextBoxOpen($"A Locked {chestTile.LookAtText}");
                Map(Player.X, Player.Y);
                return;
            }


            if (Player.LevelMap[Player.LookingAtY, Player.LookingAtX] is PedestalTile pedestalTile)
            {
                var pedestal = pedestalTile.Pedestal;

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
            txtLookingAt.Text = Player.LevelMap[Player.LookingAtY, Player.LookingAtX].LookAtText;
            Map(Player.X, Player.Y);
        }

        private void Die()
        {
            timer1.Stop();
            stopwatch.Stop();

            pbxPlayer.Image = Properties.Resources.border;
            Player.Direction = Properties.Resources.border;

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
            var answer = MessageBox.Show("Are you sure you want to restart? This will lose all your progress!", "Restart?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

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
            Player.Direction = Player.Down;
            bxGreen.Image = Properties.Resources.green_chest_closed;
            bxYellow.Image = Properties.Resources.yellow_chest_closed;
            bxRed.Image = Properties.Resources.red_chest_closed;
            bxPurple.Image = Properties.Resources.purple_chest_closed;
        }

        private string IsFacing(int X, int Y)
        {
            return Player.LevelMap[Y, X].LookAtText;
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            Player.Direction = Player.Up; // Changes player direction
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
            Player.Direction = Player.Down;
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
            Player.Direction = Player.Left;
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
            Player.Direction = Player.Right;
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


            if (levelUp && !txtOutput.Enabled) { LevelUp(); InfoBoxUpdate(); levelUp = false; }

            Random random = new Random();


            if (stopwatch.ElapsedMilliseconds > randomTime)
            {
                pbxPlayer.Image = Player.Animation;
                randomTime = random.Next(15000, 30001);
                stopwatch.Restart();

            }

            if (stopwatch.ElapsedMilliseconds < 20000 && stopwatch.ElapsedMilliseconds > 1500)
            {
                pbxPlayer.Image = Player.Direction;
            }


            if (Player.HP == 0 && !txtOutput.Enabled)
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