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
using static System.Windows.Forms.LinkLabel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace NavigationAtt2
{
    public partial class Form1 : Form
    {

        // initializing variables to detect when things are done
        public System.Drawing.Image playerDirection = Properties.Resources.border;
        private int spawnX = 0;
        private int spawnY = 0;
        private int playerX = 0;
        private int playerY = 0;
        private int randomTime = 20000;
        private int xpToAdd = 0;
        private string itemToRemove = "";
        private bool isTextBoxOpenShowing = false;
        private bool levelUp = false;
        private readonly Stopwatch stopwatch = new Stopwatch();
        private char[,] levelMap = { };
        private Hero Player = new();
        private Chest GreenChest = new();
        private Chest YellowChest = new();
        private Chest RedChest = new();
        private Chest PurpleChest = new();
        private Door GreenDoor = new();
        private Door YellowDoor = new();
        private Door RedDoor = new();
        private Door PurpleDoor = new();
        private Door EndDoor = new();
        private Pedestal GreenPed = new();
        private Pedestal YellowPed = new();
        private Pedestal RedPed = new();
        private Pedestal PurplePed = new();
        private ItemList itemList = new ItemList();
        DateTime startTime = new DateTime();
        DateTime endTime = new DateTime();


        public Form1(Hero character, bool didLoad, int fileNum)
        {
            InitializeComponent();
            pbx42.Controls.Add(pbxPlayer);
            pbxPlayer.Location = new Point(0, 0);
            txtOutput.Location = new Point(12, 292);
            pbxPlayer.BackColor = Color.Transparent; // makes the player picturebox have a transparent background so the floor doesn't look weird
            pbxPlayer.Size = pbx42.Size;
            boxInventory.Text = "Inventory";
            btnUnlock.Visible = false;
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
            Map(playerX, playerY);
            timer1.Start();
            stopwatch.Start();

        }

        private bool Fight(string itemToGet)
        {
            TextBoxOpen("", true);
            FightScreen fightScreen = new(Player);
            fightScreen.ShowDialog(this);
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
                $"[+{fightScreen.Opponent.XPAward} XP]");
            fightScreen.Close();
            return true;
        }

        private void LevelUp()
        {
            Player.Level++;
            TextBoxOpen($"LEVEL UP!\r\n\r\nCongratulations on level {Player.Level}!");
            Player.MaxXP += 25;
            pgbXP.Maximum = Player.MaxXP;
            xpToAdd += 5;
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
                if (Player.HP == 100) // keeps item from being used if health is already 100
                {
                    TextBoxOpen("Health already full!");
                    return false;
                }

                Player.HP += amount; // adds HP
                if (Player.HP > 100) // if HP is over 100, sets it to 100 to prevent the progrssbar from crashing
                {
                    Player.HP = 100;
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
            pgbPlayerHP.Value = Player.HP; // sets the progressbar to show HP 
            pgbXP.Value = Player.XP; // sets the progressbar to show XP 
            boxHP.Text = $"HP: {Player.HP} | XP: {Player.XP}/{Player.MaxXP} | Level {Player.Level}";
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
                    if (roomCharInt == 0 && roomCounter < 2) { roomChar = 'h'; roomCounter++; lastRoom = randomMap[spawnY, spawnX]; }
                    else
                    {
                        int roomVarient = random.Next(0, 6);
                        if (roomVarient == 0 && roomCounter < 2) { roomChar = 'r'; roomCounter++; lastRoom = randomMap[spawnY, spawnX]; }
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
                                int endDirection = random.Next(0, 1);
                                endPlaced = true;
                                roomCounter = 0;
                                if (endDirection == 0)
                                {
                                    roomChar = 'e';
                                    lastRoom = 'e';
                                }
                                else
                                {
                                    roomChar = 'f';
                                    lastRoom = 'f';
                                }
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

        private static char[,] MapMaker() // this translates the small map into a big map that the player can walk on
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

        private void LoadMap() // loads the map array into a public array, so it doesn't have to translate the small array to big every time a player moves
        {

            if (Player.Floor > 1)
            {
                endTime = DateTime.Now;

                TimeSpan time = endTime - startTime;

                TextBoxOpen($"Time to complete last dungeon:\r\n\r\n[{time:mm}:{time:ss}]");
            }

            levelMap = MapMaker();

            ResetMap();

            for (var i = 0; i < levelMap.GetLength(0); i++)
            {
                for (var j = 0; j < levelMap.GetLength(1); j++)
                {
                    if (levelMap[i, j] == 's')
                    {
                        spawnX = j;
                        spawnY = i;
                    }
                }
            }

            playerX = spawnX;
            playerY = spawnY;
            txtLevel.Text = Player.Floor.ToString();
            startTime = DateTime.Now;
            Map(playerX, playerY);
        }

        private bool IsFloor(int X, int Y) // a part of the map maker, returns true if a floor tile is at a given coord, false if not.
        {
            if (levelMap[Y, X] == '1' || levelMap[Y, X] == 's' || levelMap[Y, X] == 'c' || levelMap[Y, X] == '!' || levelMap[Y, X] == '@' || levelMap[Y, X] == '#' || levelMap[Y, X] == '$' || levelMap[Y, X] == '%' || levelMap[Y, X] == '^' || levelMap[Y, X] == '&' || levelMap[Y, X] == '*' || levelMap[Y, X] == '-' || levelMap[Y, X] == '=' || levelMap[Y, X] == '[' || levelMap[Y, X] == ']' || levelMap[Y, X] == '+')
            {
                return true;
            }

            else return false;
        }

        private System.Drawing.Image MapRooms(int X, int Y) // the map maker, runs each space in the map array through a bunch of true/false if statements to see what image to apply where.
        {

            if (X < 0 || X >= levelMap.GetLength(1)) return Properties.Resources.no_wall;
            if (Y < 0 || Y >= levelMap.GetLength(0)) return Properties.Resources.no_wall;


            {


                if (levelMap[Y, X] == ' ')
                {
                    if (Y - 1 < 0 && X - 1 < 0)
                    {
                        if (IsFloor(X + 1, Y + 1)) return Properties.Resources.dot_top_right;
                        else return Properties.Resources.no_wall;
                    }

                    else if (Y - 1 < 0 && X + 1 >= levelMap.GetLength(1))
                    {
                        if (IsFloor(X - 1, Y + 1)) return Properties.Resources.dot_top_left;
                        else return Properties.Resources.no_wall;
                    }

                    else if (Y + 1 >= levelMap.GetLength(0) && X - 1 < 0)
                    {
                        if (IsFloor(X + 1, Y - 1)) return Properties.Resources.wall_right;
                        else return Properties.Resources.no_wall;
                    }

                    else if (Y + 1 >= levelMap.GetLength(0) && X + 1 >= levelMap.GetLength(1))
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

                    else if (Y + 1 >= levelMap.GetLength(0))
                    {
                        if (IsFloor(X, Y - 1) == true) return Properties.Resources.wall_bot;
                        else if (IsFloor(X - 1, Y - 1) == true) return Properties.Resources.wall_left;
                        else if (IsFloor(X + 1, Y - 1) == true) return Properties.Resources.wall_right;
                        else return Properties.Resources.no_wall;
                    }

                    else if (X + 1 >= levelMap.GetLength(1))
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
                    else if (IsFloor(X + 1, Y) && IsFloor(X - 1, Y) && !IsFloor(X, Y + 1) && !IsFloor(X, Y - 1)) return Properties.Resources.Wall_L_R;
                    else if (!IsFloor(X + 1, Y) && !IsFloor(X - 1, Y) && IsFloor(X, Y + 1) && IsFloor(X, Y - 1)) return Properties.Resources.wall_bot;
                    else if (!IsFloor(X, Y + 1) && IsFloor(X, Y - 1) && IsFloor(X + 1, Y + 1) && !IsFloor(X + 1, Y) && IsFloor(X - 1, Y) && IsFloor(X - 1, Y - 1)) return Properties.Resources.wall_bot;
                    else if (!IsFloor(X, Y + 1) && IsFloor(X, Y - 1) && IsFloor(X - 1, Y + 1) && !IsFloor(X - 1, Y) && IsFloor(X + 1, Y) && IsFloor(X + 1, Y - 1)) return Properties.Resources.wall_bot;
                    else if (!IsFloor(X, Y - 1) && IsFloor(X, Y + 1) && IsFloor(X + 1, Y - 1) && !IsFloor(X + 1, Y) && IsFloor(X - 1, Y) && IsFloor(X - 1, Y + 1)) return Properties.Resources.wall_back_left;
                    else if (!IsFloor(X, Y - 1) && IsFloor(X, Y + 1) && IsFloor(X - 1, Y - 1) && !IsFloor(X - 1, Y) && IsFloor(X + 1, Y) && IsFloor(X + 1, Y + 1)) return Properties.Resources.wall_back_right;
                    else if (IsFloor(X + 1, Y) && IsFloor(X - 1, Y) && IsFloor(X, Y + 1) && !IsFloor(X, Y - 1)) return Properties.Resources.Wall_T_L_R;
                    else if (IsFloor(X - 1, Y + 1) && IsFloor(X, Y - 1) && IsFloor(X + 1, Y + 1)) return Properties.Resources.wall_bot;
                    else if (IsFloor(X - 1, Y - 1) && IsFloor(X, Y + 1) && IsFloor(X + 1, Y - 1)) return Properties.Resources.wall_back_left_right;
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

                else if (levelMap[Y, X] == '1' || levelMap[Y, X] == 's') return Properties.Resources.floor;
                else if (GreenChest.IsOpen && levelMap[Y, X] == '!') return Properties.Resources.green_chest_open; // chests & doors use special chars
                else if (levelMap[Y, X] == '!') return Properties.Resources.green_chest_closed;
                else if (YellowChest.IsOpen && levelMap[Y, X] == '@') return Properties.Resources.yellow_chest_open;
                else if (levelMap[Y, X] == '@') return Properties.Resources.yellow_chest_closed;
                else if (RedChest.IsOpen && levelMap[Y, X] == '#') return Properties.Resources.red_chest_open;
                else if (levelMap[Y, X] == '#') return Properties.Resources.red_chest_closed;
                else if (PurpleChest.IsOpen && levelMap[Y, X] == '$') return Properties.Resources.purple_chest_open;
                else if (levelMap[Y, X] == '$') return Properties.Resources.purple_chest_closed;


                else if (levelMap[Y, X] == '%' && IsFloor(X, Y + 1) && !IsFloor(X - 1, Y) && !IsFloor(X - 1, Y + 1) && !IsFloor(X + 1, Y) && !IsFloor(X + 1, Y + 1))
                {
                    if (GreenDoor.isUnlocked) return Properties.Resources.green_door_open_up;
                    else return Properties.Resources.green_door_closed_up;
                }

                else if (levelMap[Y, X] == '%' && IsFloor(X, Y - 1) && !IsFloor(X - 1, Y) && !IsFloor(X - 1, Y - 1) && !IsFloor(X + 1, Y) && !IsFloor(X + 1, Y - 1))
                {
                    if (GreenDoor.isUnlocked) return Properties.Resources.green_door_open_up;
                    else return Properties.Resources.green_door_closed_up;
                }

                else if (levelMap[Y, X] == '%' && IsFloor(X - 1, Y) && !IsFloor(X - 1, Y + 1) && !IsFloor(X - 1, Y - 1) && !IsFloor(X, Y + 1) && !IsFloor(X, Y - 1))
                {
                    if (GreenDoor.isUnlocked) return Properties.Resources.green_door_open_right;
                    else return Properties.Resources.green_door_closed_right;
                }

                else if (levelMap[Y, X] == '%' && IsFloor(X + 1, Y) && !IsFloor(X + 1, Y + 1) && !IsFloor(X + 1, Y - 1) && !IsFloor(X, Y + 1) && !IsFloor(X, Y - 1))
                {
                    if (GreenDoor.isUnlocked) return Properties.Resources.green_door_open_left;
                    else return Properties.Resources.green_door_closed_left;
                }


                else if (levelMap[Y, X] == '^' && IsFloor(X, Y + 1) && !IsFloor(X - 1, Y) && !IsFloor(X - 1, Y + 1) && !IsFloor(X + 1, Y) && !IsFloor(X + 1, Y + 1))
                {
                    if (YellowDoor.isUnlocked) return Properties.Resources.yellow_door_open_up;
                    else return Properties.Resources.yellow_door_closed_up;
                }

                else if (levelMap[Y, X] == '^' && IsFloor(X, Y - 1) && !IsFloor(X - 1, Y) && !IsFloor(X - 1, Y - 1) && !IsFloor(X + 1, Y) && !IsFloor(X + 1, Y - 1))
                {
                    if (YellowDoor.isUnlocked) return Properties.Resources.yellow_door_open_up;
                    else return Properties.Resources.yellow_door_closed_up;
                }

                else if (levelMap[Y, X] == '^' && IsFloor(X - 1, Y) && !IsFloor(X - 1, Y + 1) && !IsFloor(X - 1, Y - 1) && !IsFloor(X, Y + 1) && !IsFloor(X, Y - 1))
                {
                    if (YellowDoor.isUnlocked) return Properties.Resources.yellow_door_open_right;
                    else return Properties.Resources.yellow_door_closed_right;
                }

                else if (levelMap[Y, X] == '^' && IsFloor(X + 1, Y) && !IsFloor(X + 1, Y + 1) && !IsFloor(X + 1, Y - 1) && !IsFloor(X, Y + 1) && !IsFloor(X, Y - 1))
                {
                    if (YellowDoor.isUnlocked) return Properties.Resources.yellow_door_open_left;
                    else return Properties.Resources.yellow_door_closed_left;
                }



                else if (levelMap[Y, X] == '&' && IsFloor(X, Y + 1) && !IsFloor(X - 1, Y) && !IsFloor(X - 1, Y + 1) && !IsFloor(X + 1, Y) && !IsFloor(X + 1, Y + 1))
                {
                    if (RedDoor.isUnlocked) return Properties.Resources.red_door_open_up;
                    else return Properties.Resources.red_door_closed_up;
                }

                else if (levelMap[Y, X] == '&' && IsFloor(X, Y - 1) && !IsFloor(X - 1, Y) && !IsFloor(X - 1, Y - 1) && !IsFloor(X + 1, Y) && !IsFloor(X + 1, Y - 1))
                {
                    if (RedDoor.isUnlocked) return Properties.Resources.red_door_open_up;
                    else return Properties.Resources.red_door_closed_up;
                }

                else if (levelMap[Y, X] == '&' && IsFloor(X - 1, Y) && !IsFloor(X - 1, Y + 1) && !IsFloor(X - 1, Y - 1) && !IsFloor(X, Y + 1) && !IsFloor(X, Y - 1))
                {
                    if (RedDoor.isUnlocked) return Properties.Resources.red_door_open_right;
                    else return Properties.Resources.red_door_closed_right;
                }

                else if (levelMap[Y, X] == '&' && IsFloor(X + 1, Y) && !IsFloor(X + 1, Y + 1) && !IsFloor(X + 1, Y - 1) && !IsFloor(X, Y + 1) && !IsFloor(X, Y - 1))
                {
                    if (RedDoor.isUnlocked) return Properties.Resources.red_door_open_left;
                    else return Properties.Resources.red_door_closed_left;
                }


                else if (levelMap[Y, X] == '*' && IsFloor(X, Y + 1) && !IsFloor(X - 1, Y) && !IsFloor(X - 1, Y + 1) && !IsFloor(X + 1, Y) && !IsFloor(X + 1, Y + 1))
                {
                    if (PurpleDoor.isUnlocked) return Properties.Resources.purple_door_open_up;
                    else return Properties.Resources.purple_door_closed_up;
                }

                else if (levelMap[Y, X] == '*' && IsFloor(X, Y - 1) && !IsFloor(X - 1, Y) && !IsFloor(X - 1, Y - 1) && !IsFloor(X + 1, Y) && !IsFloor(X + 1, Y - 1))
                {
                    if (PurpleDoor.isUnlocked) return Properties.Resources.purple_door_open_up;
                    else return Properties.Resources.purple_door_closed_up;
                }

                else if (levelMap[Y, X] == '*' && IsFloor(X - 1, Y) && !IsFloor(X - 1, Y + 1) && !IsFloor(X - 1, Y - 1) && !IsFloor(X, Y + 1) && !IsFloor(X, Y - 1))
                {
                    if (PurpleDoor.isUnlocked) return Properties.Resources.purple_door_open_right;
                    else return Properties.Resources.purple_door_closed_right;
                }

                else if (levelMap[Y, X] == '*' && IsFloor(X + 1, Y) && !IsFloor(X + 1, Y + 1) && !IsFloor(X + 1, Y - 1) && !IsFloor(X, Y + 1) && !IsFloor(X, Y - 1))
                {
                    if (PurpleDoor.isUnlocked) return Properties.Resources.purple_door_open_left;
                    else return Properties.Resources.purple_door_closed_left;
                }


                else if (levelMap[Y, X] == '+' && IsFloor(X, Y + 1) && !IsFloor(X - 1, Y) && !IsFloor(X - 1, Y + 1) && !IsFloor(X + 1, Y) && !IsFloor(X + 1, Y + 1))
                {
                    if (EndDoor.isUnlocked) return Properties.Resources.brown_door_open_up;
                    else return Properties.Resources.brown_door_closed_up;
                }

                else if (levelMap[Y, X] == '+' && IsFloor(X, Y - 1) && !IsFloor(X - 1, Y) && !IsFloor(X - 1, Y - 1) && !IsFloor(X + 1, Y) && !IsFloor(X + 1, Y - 1))
                {
                    if (EndDoor.isUnlocked) return Properties.Resources.brown_door_open_up;
                    else return Properties.Resources.brown_door_closed_up;
                }

                else if (levelMap[Y, X] == '+' && IsFloor(X - 1, Y) && !IsFloor(X - 1, Y + 1) && !IsFloor(X - 1, Y - 1) && !IsFloor(X, Y + 1) && !IsFloor(X, Y - 1))
                {
                    if (EndDoor.isUnlocked) return Properties.Resources.brown_door_open_right;
                    else return Properties.Resources.brown_door_closed_right;
                }

                else if (levelMap[Y, X] == '+' && IsFloor(X + 1, Y) && !IsFloor(X + 1, Y + 1) && !IsFloor(X + 1, Y - 1) && !IsFloor(X, Y + 1) && !IsFloor(X, Y - 1))
                {
                    if (EndDoor.isUnlocked) return Properties.Resources.brown_door_open_left;
                    else return Properties.Resources.brown_door_closed_left;
                }


                else if (levelMap[Y, X] == '-') return Properties.Resources.green_ped;
                else if (levelMap[Y, X] == '=') return Properties.Resources.Yellow_ped;
                else if (levelMap[Y, X] == '[') return Properties.Resources.red_ped;
                else if (levelMap[Y, X] == ']') return Properties.Resources.purple_ped;

                else if (levelMap[Y, X] == 'c') return Properties.Resources.cactus;
                else if (levelMap[Y, X] == 't') return Properties.Resources.torch_wall;
                else if (levelMap[Y, X] == 'e') return Properties.Resources.down_l;
                else if (levelMap[Y, X] == 'f') return Properties.Resources.down_r;

                else return Properties.Resources.no_wall;// if char not found, it returns this to prevent crashes
            }
        }

        private void Map(int X, int Y) // The map updater, this is what calls the map maker for each square on the map
        {
            pbx0n1.Image = MapRooms(X - 4, Y - 3); // sets each photobox to the proper image for it using x&y coords
            pbx1n1.Image = MapRooms(X - 3, Y - 3);
            pbx2n1.Image = MapRooms(X - 2, Y - 3);
            pbx3n1.Image = MapRooms(X - 1, Y - 3);
            pbx4n1.Image = MapRooms(X + 0, Y - 3);
            pbx5n1.Image = MapRooms(X + 1, Y - 3);
            pbx6n1.Image = MapRooms(X + 2, Y - 3);
            pbx7n1.Image = MapRooms(X + 3, Y - 3);
            pbx8n1.Image = MapRooms(X + 4, Y - 3);

            pbx00.Image = MapRooms(X - 4, Y - 2);
            pbx10.Image = MapRooms(X - 3, Y - 2);
            pbx20.Image = MapRooms(X - 2, Y - 2);
            pbx30.Image = MapRooms(X - 1, Y - 2);
            pbx40.Image = MapRooms(X + 0, Y - 2);
            pbx50.Image = MapRooms(X + 1, Y - 2);
            pbx60.Image = MapRooms(X + 2, Y - 2);
            pbx70.Image = MapRooms(X + 3, Y - 2);
            pbx80.Image = MapRooms(X + 4, Y - 2);

            pbx01.Image = MapRooms(X - 4, Y - 1);
            pbx11.Image = MapRooms(X - 3, Y - 1);
            pbx21.Image = MapRooms(X - 2, Y - 1);
            pbx31.Image = MapRooms(X - 1, Y - 1);
            pbx41.Image = MapRooms(X + 0, Y - 1);
            pbx51.Image = MapRooms(X + 1, Y - 1);
            pbx61.Image = MapRooms(X + 2, Y - 1);
            pbx71.Image = MapRooms(X + 3, Y - 1);
            pbx81.Image = MapRooms(X + 4, Y - 1);

            pbx02.Image = MapRooms(X - 4, Y - 0);
            pbx12.Image = MapRooms(X - 3, Y - 0);
            pbx22.Image = MapRooms(X - 2, Y - 0);
            pbx32.Image = MapRooms(X - 1, Y - 0);
            pbx42.Image = MapRooms(X, Y);
            pbx52.Image = MapRooms(X + 1, Y - 0);
            pbx62.Image = MapRooms(X + 2, Y - 0);
            pbx72.Image = MapRooms(X + 3, Y - 0);
            pbx82.Image = MapRooms(X + 4, Y - 0);

            pbx03.Image = MapRooms(X - 4, Y + 1);
            pbx13.Image = MapRooms(X - 3, Y + 1);
            pbx23.Image = MapRooms(X - 2, Y + 1);
            pbx33.Image = MapRooms(X - 1, Y + 1);
            pbx43.Image = MapRooms(X + 0, Y + 1);
            pbx53.Image = MapRooms(X + 1, Y + 1);
            pbx63.Image = MapRooms(X + 2, Y + 1);
            pbx73.Image = MapRooms(X + 3, Y + 1);
            pbx83.Image = MapRooms(X + 4, Y + 1);

            pbx04.Image = MapRooms(X - 4, Y + 2);
            pbx14.Image = MapRooms(X - 3, Y + 2);
            pbx24.Image = MapRooms(X - 2, Y + 2);
            pbx34.Image = MapRooms(X - 1, Y + 2);
            pbx44.Image = MapRooms(X + 0, Y + 2);
            pbx54.Image = MapRooms(X + 1, Y + 2);
            pbx64.Image = MapRooms(X + 2, Y + 2);
            pbx74.Image = MapRooms(X + 3, Y + 2);
            pbx84.Image = MapRooms(X + 4, Y + 2);

            pbx05.Image = MapRooms(X - 4, Y + 3);
            pbx15.Image = MapRooms(X - 3, Y + 3);
            pbx25.Image = MapRooms(X - 2, Y + 3);
            pbx35.Image = MapRooms(X - 1, Y + 3);
            pbx45.Image = MapRooms(X + 0, Y + 3);
            pbx55.Image = MapRooms(X + 1, Y + 3);
            pbx65.Image = MapRooms(X + 2, Y + 3);
            pbx75.Image = MapRooms(X + 3, Y + 3);
            pbx85.Image = MapRooms(X + 4, Y + 3);

            pbxPlayer.Image = playerDirection;

            pbxGreenChestInd.Image = GreenChest.IsOpen ? Properties.Resources.green_chest_open : Properties.Resources.green_chest_closed;
            pbxYellowChestInd.Image = YellowChest.IsOpen ? Properties.Resources.yellow_chest_open : Properties.Resources.yellow_chest_closed;
            pbxRedChestInd.Image = RedChest.IsOpen ? Properties.Resources.red_chest_open : Properties.Resources.red_chest_closed;
            pbxPurpleChestInd.Image = PurpleChest.IsOpen ? Properties.Resources.purple_chest_open : Properties.Resources.purple_chest_closed;
        }

        private string IsFacing(int X, int Y)
        {
            if (Y > levelMap.GetLength(0) - 1 || Y < 0)
            {
                return "Wall";
            }
            else if (X > levelMap.GetLength(1) - 1 || X < 0)
            {
                return "Wall";
            }
            else if (levelMap[Y, X] == ' ')
            {
                return "Wall";
            }
            else if (levelMap[Y, X] == '!')
            {
                return "Green Chest";
            }
            else if (levelMap[Y, X] == '@')
            {
                return "Yellow Chest";
            }
            else if (levelMap[Y, X] == '#')
            {
                return "Red Chest";
            }
            else if (levelMap[Y, X] == '$')
            {
                return "Purple Chest";
            }
            else if (levelMap[Y, X] == '%')
            {
                if (GreenDoor.isUnlocked) return "null";
                else return "Green Door";
            }
            else if (levelMap[Y, X] == '^')
            {
                if (YellowDoor.isUnlocked) return "null";
                else return "Yellow Door";
            }
            else if (levelMap[Y, X] == '&')
            {
                if (RedDoor.isUnlocked) return "null";
                else return "Red Door";
            }
            else if (levelMap[Y, X] == '*')
            {
                if (PurpleDoor.isUnlocked) return "null";
                else return "Purple Door";
            }
            else if (levelMap[Y, X] == '+')
            {
                if (EndDoor.isUnlocked) return "null";
                else return "Stairwell Door";
            }
            else if (levelMap[Y, X] == 'e')
            {
                return "Stairwell";
            }
            else if (levelMap[Y, X] == '-')
            {
                return "Green Pedestal";
            }
            else if (levelMap[Y, X] == '=')
            {
                return "Yellow Pedestal";
            }
            else if (levelMap[Y, X] == '[')
            {
                return "Red Pedestal";
            }
            else if (levelMap[Y, X] == ']')
            {
                return "Purple Pedestal";
            }
            else if (levelMap[Y, X] == 'c')
            {
                return "cactus";
            }
            else if (levelMap[Y, X] == 't')
            {
                return "Torch";
            }
            else
            {
                return "null";
            }
        } // gives a text description of any X,Y coord, used wenever you want to know what the player is facing

        private void TextBoxOpen(string text = "", bool hide = false)
        {
            if (hide == false) // shows TextBox
            {
                txtOutput.Text = ""; // ensures TextBox is empty
                btnUp.Enabled = false;
                btnDown.Enabled = false;
                btnLeft.Enabled = false;
                btnRight.Enabled = false; //disables direction buttons
                txtOutput.Visible = true; // makes text box visible
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
                txtOutput.Visible = false;
                Map(playerX, playerY);
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
            bool useInventory = InventoryEvents();

            if (useInventory) { return; }

            if (txtLookingAt.Text == "Green Door")
            {
                if (GreenDoor.isUnlocked == true) TextBoxOpen("An open green door.");
                else TextBoxOpen("A locked green door.");
            } // viewing doors

            if (txtLookingAt.Text == "Yellow Door")
            {
                if (YellowDoor.isUnlocked == true) TextBoxOpen("An open yellow door.");
                else TextBoxOpen("A locked yellow door.");
            }

            if (txtLookingAt.Text == "Red Door")
            {
                if (RedDoor.isUnlocked == true) TextBoxOpen("An open red door.");
                else TextBoxOpen("A locked red door.");
            }

            if (txtLookingAt.Text == "Purple Door")
            {
                if (PurpleDoor.isUnlocked == true) TextBoxOpen("An open purple door.");
                else TextBoxOpen("A locked purple door.");
            }

            if (txtLookingAt.Text == "Stairwell Door")
            {
                if (GreenChest.IsOpen && YellowChest.IsOpen && RedChest.IsOpen && PurpleChest.IsOpen && !isTextBoxOpenShowing)
                {
                    TextBoxOpen("The dungeon can tell you've done everything on this floor... Congradulations. \r\n\r\n [Stairwell Door Unlocked]");
                    EndDoor.isUnlocked = true;
                }
                else TextBoxOpen("A locked stairwell door, to open it you must unlock all 4 chests. Come back when this is complete...");
            }

            if (txtLookingAt.Text == "Green Chest")
            {
                if (GreenChest.IsOpen == true) TextBoxOpen("An open green chest.");
                else TextBoxOpen("A locked green chest.");
            } // viewing chests

            if (txtLookingAt.Text == "Yellow Chest")
            {
                if (YellowChest.IsOpen == true) TextBoxOpen("An open yellow chest.");
                else TextBoxOpen("A locked yellow chest.");
            }

            if (txtLookingAt.Text == "Red Chest")
            {
                if (RedChest.IsOpen == true) TextBoxOpen("An open red chest.");
                else TextBoxOpen("A locked red chest.");
            }

            if (txtLookingAt.Text == "Purple Chest")
            {
                if (PurpleChest.IsOpen == true) TextBoxOpen("An open purple chest.");
                else TextBoxOpen("A locked purple chest.");
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

            if (txtLookingAt.Text == "Green Pedestal")
            {
                if (!GreenPed.HasGivenItem)
                {
                    var answer = MessageBox.Show("A Challenge awaits... Do you accept?", "Green Challenge", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (answer == DialogResult.Yes)
                    {
                        var didSurvive = Fight("Green Key");
                        if (!didSurvive) { Die(); }
                        else GreenPed.HasGivenItem = true;
                        return;
                    }
                }
                else
                {
                    TextBoxOpen($"Floor {Player.Floor} Green Challenge completed");
                }
            }// green ped action

            if (txtLookingAt.Text == "Yellow Pedestal")
            {
                if (!YellowPed.HasGivenItem)
                {
                    var answer = MessageBox.Show("A Challenge awaits... Do you accept?", "Yellow Challenge", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (answer == DialogResult.Yes)
                    {
                        var didSurvive = Fight("Yellow Key");
                        if (!didSurvive) { Die(); }
                        else YellowPed.HasGivenItem = true;
                        return;
                    }
                }
                else
                {
                    TextBoxOpen($"Floor {Player.Floor} Yellow Challenge completed");
                }
            }// yellow ped action

            if (txtLookingAt.Text == "Red Pedestal")
            {
                if (!RedPed.HasGivenItem)
                {
                    var answer = MessageBox.Show("A Challenge awaits... Do you accept?", "Red Challenge", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (answer == DialogResult.Yes)
                    {
                        var didSurvive = Fight("Red Key");
                        if (!didSurvive) { Die(); }
                        else RedPed.HasGivenItem = true;
                        return;
                    }
                }
                else
                {
                    TextBoxOpen($"Floor {Player.Floor} Red Challenge completed");
                }
            }// red ped action

            if (txtLookingAt.Text == "Purple Pedestal")
            {
                if (!PurplePed.HasGivenItem)
                {
                    var answer = MessageBox.Show("A Challenge awaits... Do you accept?", "Purple Challenge", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (answer == DialogResult.Yes)
                    {
                        var didSurvive = Fight("Purple Key");
                        if (!didSurvive) { Die(); }
                        else PurplePed.HasGivenItem = true;
                        return;
                    }
                }
                else
                {
                    TextBoxOpen($"Floor {Player.Floor} Purple Challenge completed");
                }
            }// purple ped action


        } // events when the action button is pressed


        private bool InventoryEvents()
        {

            if (lbxInventoryBox.SelectedIndex == -1)
            {
                return false;
            }

            string contents = lbxInventoryBox.Items[lbxInventoryBox.SelectedIndex].ToString() ?? "null";

            if (contents == "null") { return false; }

            contents = contents.Substring(contents.IndexOf('-') + 2);

            if (rbnToss.Checked)
            {
                if (contents == "Green Key" || contents == "Yellow Key" || contents == "Red Key" || contents == "Purple Key")
                {
                    TextBoxOpen("You cannot toss a required item!");
                    return false;
                }
                else
                {
                    RemoveFromInventory(contents);
                    TextBoxOpen($"{contents} Tossed");
                    return true;
                }
            }

            if (txtLookingAt.Text == "Green Door" && contents == "Green Key")
            {
                GreenDoor.isUnlocked = true;
                TextBoxOpen("You put the key into the green door and turn it. You feel all green doors in the dungeon opening...\r\n\r\n [Green doors unlocked]");
                lbxInventoryBox.SelectedItems.Clear();

                return true;
            } // if trying to use the green key on the green door

            if (txtLookingAt.Text == "Yellow Door" && contents == "Yellow Key")
            {
                YellowDoor.isUnlocked = true;
                TextBoxOpen("You put the key into the yellow door and turn it. You feel all yellow doors in the dungeon opening...\r\n\r\n [Yellow doors unlocked]");
                lbxInventoryBox.SelectedItems.Clear();

                return true;
            }// if trying to use the yellow key on the yellow door

            if (txtLookingAt.Text == "Red Door" && contents == "Red Key")
            {
                RedDoor.isUnlocked = true;
                TextBoxOpen("You put the key into the red door and turn it. You feel all red doors in the dungeon opening...\r\n\r\n [Red doors unlocked]");
                lbxInventoryBox.SelectedItems.Clear();

                return true;
            }// if trying to use the red key on the red door

            if (txtLookingAt.Text == "Purple Door" && contents == "Purple Key")
            {
                PurpleDoor.isUnlocked = true;
                TextBoxOpen("You put the key into the purple door and turn it. You feel all purple doors in the dungeon opening...\r\n\r\n [Purple doors unlocked]");
                lbxInventoryBox.SelectedItems.Clear();

                return true;
            }// if trying to use the purple key on the purple door

            if (txtLookingAt.Text == "Green Chest" && contents == "Green Key")
            {
                if (!GreenChest.IsOpen)
                {
                    var itemID = FindItem(GreenChest.Contents);
                    TextBoxOpen($"You open the green chest, and you get a {GreenChest.Contents}! {itemList.Items[itemID].GetText}\r\n" +
                        "The key gets stuck in the chest.\r\n\r\n" +
                        $"[Green Key Removed.]\r\n[{GreenChest.Contents} added.]");
                    GreenChest.IsOpen = true;
                    RemoveFromInventory("Green Key", false);
                    AddToInventory(GreenChest.Contents);
                    lbxInventoryBox.SelectedItems.Clear();

                    return true;
                }
            }// if trying to use the green key on the green chest

            if (txtLookingAt.Text == "Yellow Chest" && contents == "Yellow Key")
            {
                if (!YellowChest.IsOpen)
                {
                    var itemID = FindItem(YellowChest.Contents);
                    TextBoxOpen($"You open the yellow chest, and you get a {YellowChest.Contents}! {itemList.Items[itemID].GetText}\r\n" +
                        "The key gets stuck in the chest.\r\n\r\n" +
                        $"[Yellow Key Removed.]\r\n[{YellowChest.Contents} added.]");
                    YellowChest.IsOpen = true;
                    RemoveFromInventory("Yellow Key", false);
                    AddToInventory(YellowChest.Contents);
                    lbxInventoryBox.SelectedItems.Clear();

                    return true;
                }
            }// if trying to use the red key on the red chest

            if (txtLookingAt.Text == "Red Chest" && contents == "Red Key")
            {
                if (!RedChest.IsOpen)
                {
                    var itemID = FindItem(RedChest.Contents);
                    TextBoxOpen($"You open the red chest, and you get a {RedChest.Contents}! {itemList.Items[itemID].GetText}\r\n" +
                        "The key gets stuck in the chest.\r\n\r\n" +
                        $"[Red Key Removed.]\r\n[{RedChest.Contents} added.]");
                    RedChest.IsOpen = true;
                    RemoveFromInventory("Red Key", false);
                    AddToInventory(RedChest.Contents);
                    lbxInventoryBox.SelectedItems.Clear();

                    return true;
                }
            }// if trying to use the yellow key on the yellow chest

            if (txtLookingAt.Text == "Purple Chest" && contents == "Purple Key")
            {
                if (!PurpleChest.IsOpen)
                {
                    var itemID = FindItem(PurpleChest.Contents);
                    TextBoxOpen($"You open the purple chest, and you get a {PurpleChest.Contents}! {itemList.Items[itemID].GetText}\r\n" +
                        "The key gets stuck in the chest.\r\n\r\n" +
                        $"[Purple Key Removed.]\r\n[{PurpleChest.Contents} added.]");
                    PurpleChest.IsOpen = true;
                    RemoveFromInventory("Purple Key", false);
                    AddToInventory(PurpleChest.Contents);
                    lbxInventoryBox.SelectedItems.Clear();

                    return true;
                }
            }// if trying to use the purple key on the purple chest

            if (contents != "Green Key" && contents != "Yellow Key" && contents != "Red Key" && contents != "Purple Key")
            {
                var itemNumber = FindItem(contents);
                var itemHealth = itemList.Items[itemNumber].HealthChange;
                var didAddHealth = ChangeHealth(itemHealth, contents, $"You {itemList.Items[itemNumber].EatOrDrink} the {itemList.Items[itemNumber].Name}. {itemList.Items[itemNumber].EatText}");
                if (didAddHealth) RemoveFromInventory(contents, false);
                lbxInventoryBox.SelectedItems.Clear();

                return true;
            }

            return false;
        } // events when action button is pressed that involve the inventory -- returns 'true' if a funtion is found

        private void ButtonPushed()
        {
            stopwatch.Restart();
            Map(playerX, playerY);
        } // functions that call whenever a button is pushed -- will be used to initiate fights in the future

        private void Die()
        {
            timer1.Stop();
            stopwatch.Stop();

            pbxPlayer.Image = Properties.Resources.border;
            playerDirection = Properties.Resources.border;

            pbx0n1.Image = Properties.Resources.border;
            pbx1n1.Image = Properties.Resources.border;
            pbx2n1.Image = Properties.Resources.border;
            pbx3n1.Image = Properties.Resources.border;
            pbx4n1.Image = Properties.Resources.border;
            pbx5n1.Image = Properties.Resources.border;
            pbx6n1.Image = Properties.Resources.border;
            pbx7n1.Image = Properties.Resources.border;
            pbx8n1.Image = Properties.Resources.border;

            pbx00.Image = Properties.Resources.border;
            pbx10.Image = Properties.Resources.border;
            pbx20.Image = Properties.Resources.border;
            pbx30.Image = Properties.Resources.border;
            pbx40.Image = Properties.Resources.border;
            pbx50.Image = Properties.Resources.border;
            pbx60.Image = Properties.Resources.border;
            pbx70.Image = Properties.Resources.border;
            pbx80.Image = Properties.Resources.border;

            pbx01.Image = Properties.Resources.border;
            pbx11.Image = Properties.Resources.border;
            pbx21.Image = Properties.Resources.border;
            pbx31.Image = Properties.Resources.border;
            pbx41.Image = Properties.Resources.border;
            pbx51.Image = Properties.Resources.border;
            pbx61.Image = Properties.Resources.border;
            pbx71.Image = Properties.Resources.border;
            pbx81.Image = Properties.Resources.border;

            pbx02.Image = Properties.Resources.border;
            pbx12.Image = Properties.Resources.border;
            pbx22.Image = Properties.Resources.border;
            pbx32.Image = Properties.Resources.border;
            pbx42.Image = Properties.Resources.border;
            pbx52.Image = Properties.Resources.border;
            pbx62.Image = Properties.Resources.border;
            pbx72.Image = Properties.Resources.border;
            pbx82.Image = Properties.Resources.border;

            pbx03.Image = Properties.Resources.border;
            pbx13.Image = Properties.Resources.border;
            pbx23.Image = Properties.Resources.border;
            pbx33.Image = Properties.Resources.border;
            pbx43.Image = Properties.Resources.border;
            pbx53.Image = Properties.Resources.border;
            pbx63.Image = Properties.Resources.border;
            pbx73.Image = Properties.Resources.border;
            pbx83.Image = Properties.Resources.border;

            pbx04.Image = Properties.Resources.border;
            pbx14.Image = Properties.Resources.border;
            pbx24.Image = Properties.Resources.border;
            pbx34.Image = Properties.Resources.border;
            pbx44.Image = Properties.Resources.border;
            pbx54.Image = Properties.Resources.border;
            pbx64.Image = Properties.Resources.border;
            pbx74.Image = Properties.Resources.border;
            pbx84.Image = Properties.Resources.border;

            pbx05.Image = Properties.Resources.border;
            pbx15.Image = Properties.Resources.border;
            pbx25.Image = Properties.Resources.border;
            pbx35.Image = Properties.Resources.border;
            pbx45.Image = Properties.Resources.border;
            pbx55.Image = Properties.Resources.border;
            pbx65.Image = Properties.Resources.border;
            pbx75.Image = Properties.Resources.border;
            pbx85.Image = Properties.Resources.border;


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
            GreenChest.IsOpen = false;
            YellowChest.IsOpen = false;
            RedChest.IsOpen = false;
            PurpleChest.IsOpen = false;
            GreenDoor.isUnlocked = false;
            YellowDoor.isUnlocked = false;
            RedDoor.isUnlocked = false;
            PurpleDoor.isUnlocked = false;
            EndDoor.isUnlocked = false;
            GreenPed.HasGivenItem = false;
            GreenPed.ConditionMet = false;
            YellowPed.HasGivenItem = false;
            YellowPed.ConditionMet = false;
            RedPed.HasGivenItem = false;
            RedPed.ConditionMet = false;
            PurplePed.HasGivenItem = false;
            PurplePed.ConditionMet = false;
            GreenChest.Contents = ChestItemPool();
            YellowChest.Contents = ChestItemPool();
            RedChest.Contents = ChestItemPool();
            PurpleChest.Contents = ChestItemPool();
            txtLookingAt.Text = "-";
            playerDirection = Player.Down;
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            playerDirection = Player.Up; // Changes player direction

            if (IsFacing(playerX, playerY + 1) == "null")
            {
                playerY++;
            }

            if (IsFacing(playerX, playerY + 1) == "null") txtLookingAt.Text = " - "; else txtLookingAt.Text = IsFacing(playerX, playerY + 1);

            ButtonPushed();
        } // all direction buttons: changes player direction & detects 

        private void btnDown_Click(object sender, EventArgs e)
        {
            playerDirection = Player.Down;

            if (IsFacing(playerX, playerY - 1) == "null")
            {
                playerY--;
            }

            if (IsFacing(playerX, playerY - 1) == "null") txtLookingAt.Text = " - "; else txtLookingAt.Text = IsFacing(playerX, playerY - 1);

            ButtonPushed();
        }

        private void btnLeft_Click(object sender, EventArgs e)
        {
            playerDirection = Player.Left;

            if (IsFacing(playerX - 1, playerY) == "null")
            {
                playerX--;
            }

            if (IsFacing(playerX - 1, playerY) == "null") txtLookingAt.Text = " - "; else txtLookingAt.Text = IsFacing(playerX - 1, playerY);

            ButtonPushed();
        }

        private void btnRight_Click(object sender, EventArgs e)
        {
            playerDirection = Player.Right;
            if (IsFacing(playerX + 1, playerY) == "null")
            {
                playerX++;
            }

            if (IsFacing(playerX + 1, playerY) == "null") txtLookingAt.Text = " - "; else txtLookingAt.Text = IsFacing(playerX + 1, playerY); ;

            ButtonPushed();
        }

        private void btnAction_Click(object sender, EventArgs e)
        {
            //changes the action button to pushed, all others to false. All others are changed to false on all button presses, inclusing movement
            //      to prevent misunderstandings for both the program and the end user.
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


            if (levelUp && !txtOutput.Visible) { LevelUp(); InfoBoxUpdate(); levelUp = false; }

            Random random = new Random();

            pbxPlayer.Size = pbx42.Size;


            if (stopwatch.ElapsedMilliseconds > randomTime)
            {
                pbxPlayer.Image = Player.Animation;
                randomTime = random.Next(15000, 30001);
                stopwatch.Restart();

            }

            if (stopwatch.ElapsedMilliseconds < 20000 && stopwatch.ElapsedMilliseconds > 1500)
            {
                pbxPlayer.Image = playerDirection;
            }


            if (Player.HP == 0 && !txtOutput.Visible)
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

        private void btnUnlock_Click(object sender, EventArgs e)
        {
            EndDoor.isUnlocked = true;
            GreenDoor.isUnlocked = true;
            YellowDoor.isUnlocked = true;
            RedDoor.isUnlocked = true;
            PurpleDoor.isUnlocked = true;
            Map(playerX, playerY);
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

            if (lbxInventoryBox.SelectedIndex == -1) txtHolding.Text = "null";
            else
            {
                txtHolding.Text = lbxInventoryBox.Items[lbxInventoryBox.SelectedIndex].ToString() ?? "null";

                var contents = txtHolding.Text.Substring(txtHolding.Text.IndexOf('-') + 2);
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