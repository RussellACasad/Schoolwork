// Russell Casad
// CPT-230-W01
// 2023 FA
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
        private System.Drawing.Image playerDirection = Properties.Resources.player_up_t;
        private int spawnX = 0;
        private int spawnY = 0;
        private int playerX = 0;
        private int playerY = 0;
        private readonly int spawnMaxXP = 100;
        private readonly int spawnHP = 100;
        private readonly int spawnPlayerLevel = 1;
        private readonly int spawnPlayerXP = 0;
        private readonly int spawnLevel = 1;
        private int level = 0;
        private int playerXP = 0;
        private int playerLevel = 0;
        private int playerHP = 0;
        private int maxXP = 0;
        private int randomTime = 20000;
        private int spawnXPToAdd = 25;
        private int xpToAdd = 0;
        private string greenItem = "";
        private string yellowItem = "";
        private string redItem = "";
        private string purpleItem = "";
        private string itemToRemove = "";
        private string greenQuestion = "";
        private string redQuestion = "";
        private string yellowQuestion = "";
        private string purpleQuestion = "";
        private string greenAnswer = "";
        private string yellowAnswer = "";
        private string redAnswer = "";
        private string purpleAnswer = "";
        private bool buttonAPressed = false;
        private bool buttonBPressed = false;
        private bool buttonSPressed = false;
        private bool inv1Pressed = false;
        private bool inv2Pressed = false;
        private bool inv3Pressed = false;
        private bool inv4Pressed = false;
        private bool inv5Pressed = false;
        private bool inv6Pressed = false;
        private bool isGreenChestOpen = false;
        private bool isYellowChestOpen = false;
        private bool isRedChestOpen = false;
        private bool isPurpleChestOpen = false;
        private bool isGreenDoorUnlocked = false;
        private bool isYellowDoorUnlocked = false;
        private bool isRedDoorUnlocked = false;
        private bool isPurpleDoorUnlocked = false;
        private bool isEndDoorUnlocked = false;
        private bool isTextBoxShowing = false;
        private bool hasGivenGreenKey = false;
        private bool hasGivenYellowKey = false;
        private bool hasGivenRedKey = false;
        private bool hasGivenPurpleKey = false;
        private bool greenConditionMet = false;
        private bool yellowConditionMet = false;
        private bool redConditionMet = false;
        private bool purpleConditionMet = false;
        private bool howToPlayShown = false;
        private bool healthFullBox = false;
        private bool levelUp = false;
        private readonly Stopwatch stopwatch = new Stopwatch();
        private string[] inventory = { "null", "null", "null", "null", "null", "null" };
        private char[,] levelMap;


        DateTime startTime = new DateTime();
        DateTime endTime = new DateTime();

        string[,] Items = // string of all items that can be in a chest
            {
                // sorted {"name", "health", "eat text", "get text", "drink/eat"} -- itemMaker app included makes making these easier
                {"Health Potion", "25", "Tastes like grapes!", "Why is it purple...?", "drink"},
                {"Green Apple", "15", "Still Crunchy!", "How is this still fresh..?", "eat"},
                {"Lime", "15", "Squishy, but refreshing!", "Not even a lemon...", "eat" },
                {"Lemon", "15", "Ewww! Sour!", "Not even a lime...", "eat"},
                {"Unknown Gameboy Game", "15", "The junk on the cartrige fills you up", "If only it had a label...", "eat" },
                {"Milkshake", "15", "Still cold! Not sure if that's concerning...", "Somehow not milk soup!", "drink" },
                {"Carton of McDonald's Fries", "15", "Rubbery!", "They're cold.", "eat" },
                {"Bruised Banana", "15", "It tastes as bad as it looks...", "Might not taste very good...", "eat" },
                {"Purple GTC Promotional Bracelet", "5", "The promotional aspect gets to you!", "Very promotional.", "eat" },
                {"Pringles Can", "10", "The crumbs taste good too!", "There are crumbs in it", "eat" },
                {"Rock", "1", "Wait, did you just...", "Its a rock", "eat"},
                {"Half a brick", "5", "Crunky", "You're so hungry you consider eating it...", "eat"},
                {"Dr. Pepper", "15", "It's flat.", "Fizzy!", "drink"},
                {"Chug Jug", "100", "We can be pro Fortnite gamers!", "I really want to Chug Jug with you...", "drink"},
                {"Handful of Spiders", "15", "Don't worry, they like it. ", "That's a lot of spiders for one chest...", "eat"},
                {"3.39oz of bottled spoiled milk", "10", "At least its airline complient?", "Theres little good to be said", "eat"},
                {"A book of sheet music", "15", "Tasted like paper", "What is this even good for..?", "eat"},
                {"Box of Kraft Mac and Cheese", "15", "Crunchy!", "Nothing to cook it with though...", "eat"},
                {"1 vegan marshmallow", "5", "Firm but still tastes good", "At least it's vegan!", "eat"},
                {"A bundle of carrot sticks", "15", "Crunchy!", "They're a bit old...", "eat"},
                {"Chick-Fil-A Dipping sauce bottle", "15", "Tastes like dressing.", "Not even any nuggets!", "drink"},
                {"Cooked Chicken Chunks", "15", "They're cold...", "The breading is only slightly falling off", "eat"},
                {"can of Liquid Death Sparkling Water", "15", "Refreshing!", "Sustainable, recyclable water packaging! Cool! (Not Sponsored)", "drink"},
                {"Container of Blue Diamond Almonds", "15", "Almonds are still gross...", "Almonds are gross...", "eat"},
                {"Single-Serve bag of M&Ms", "15", "The green ones taste the best!", "I hope there's green ones...", "eat"},
            };

        public Form1()
        {
            InitializeComponent();
            pbx42.Controls.Add(pbxPlayer);
            pbxPlayer.Location = new Point(0, 0);
            txtOutput.Location = new Point(12, 292);
            pbxPlayer.BackColor = Color.Transparent; // makes the player picturebox have a transparent background so the floor doesn't look weird
            pbxPlayer.Size = pbx42.Size;
        }

        private void addXP(int toAdd)
        {
            playerXP += toAdd;

            if (playerXP >= maxXP)
            {
                playerXP = playerXP - maxXP;
                levelUp = true;
            }

            infoBoxUpdate();
        }

        private void LevelUp()
        {
            playerLevel++;
            textBox($"LEVEL UP!\r\n\r\nCongratulations on level {playerLevel}!");
            maxXP += 25;
            xpToAdd += 5;
        }

        private int findItem(string name)// finds item info in the list of items, for referencing their specific data
        {
            for (int i = 0; i < Items.GetLength(0); i++)
            {
                if (Items[i, 0] == name) return i;
            }
            return -1;
        }

        private string chestItemPool()// outputs a random item from the items list, for randomizing chest contents
        {

            Random random = new Random();

            int chestItem = random.Next(0, Items.GetLength(0));

            return Items[chestItem, 0];
        }

        private bool AddHealth(int amount, string item, string message) // for add health items
        {
            if (playerHP == 100) // keeps item from being used if health is already 100
            {
                healthFullBox = true;
                textBox("Health already full!\r\nToss the item?\r\n\r\n[Y - Yes | N - No]", false, true);
                itemToRemove = item;
                return false;
            }

            playerHP += amount; // adds HP
            if (playerHP > 100) // if HP is over 100, sets it to 100 to prevent the progrssbar from crashing
            {
                playerHP = 100;
            }
            infoBoxUpdate();

            textBox(message + $"\r\n\r\n[{item} removed]\r\n[+{amount} HP]"); // tells the user that the health was added
            return true;
        }

        private void RemoveHealth(int amount) // for removing health -- also deals with death
        {
            playerHP -= amount;

            if (playerHP < 0) playerHP = 0;

            infoBoxUpdate();

        }

        private void questionMaker()// randomly makes a math problem based on what floor the player is on
        {
            Random random = new Random();

            string[] questionStrings = new string[4];
            string[] answerStrings = new string[4];

            for (int i = 0; i < 4; i++)
            {
                int num1 = random.Next(1 + level, 10 + level * 2);
                int num2 = random.Next(1 + level, 5 + level * 2);

                questionStrings[i] = $"{num1} * {num2}";
                answerStrings[i] = (num1 * num2).ToString();
            }



            greenQuestion = questionStrings[0];
            yellowQuestion = questionStrings[1];
            redQuestion = questionStrings[2];
            purpleQuestion = questionStrings[3];

            greenAnswer = answerStrings[0];
            yellowAnswer = answerStrings[1];
            redAnswer = answerStrings[2];
            purpleAnswer = answerStrings[3];
        }

        private void infoBoxUpdate()// updates the current hp/xp/level box with new info
        {
            pgbPlayerHP.Value = playerHP; // sets the progressbar to show HP 
            pgbXP.Value = playerXP;// sets the progressbar to show XP
            boxHP.Text = $"HP: {playerHP} | XP: {playerXP}/{maxXP} | Level {playerLevel}";
        }


        // Map stuff
        private char[,] LevelSelection() //Randomly generates an array to serve as a map
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

        private char[] RoomStorage(char roomChar, char roomObject, int index, bool up, bool down, bool left, bool right)// this stores all the room prefabs
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

                    if (returnRoom[index, i] == '7' && up && roomChar == 'e') { returnArray[i] = '+'; continue; }
                    else returnArray[i] = ' ';
                    if (returnRoom[index, i] == '8' && down && roomChar == 'e') { returnArray[i] = '+'; continue; }
                    else returnArray[i] = ' ';
                    if (returnRoom[index, i] == '9' && left && roomChar == 'e') { returnArray[i] = '+'; continue; }
                    else returnArray[i] = ' ';
                    if (returnRoom[index, i] == '0' && right && roomChar == 'e') { returnArray[i] = '+'; continue; }
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

        private char[,] MapMaker() // this translates the small map into a big map that the player can walk on
        {
            char[,] internalMap = LevelSelection(); // gets the small map array
            char[,] playerMap = new char[internalMap.GetLength(0) * 7, internalMap.GetLength(1) * 7]; // makes an array the size of the player map
            List<char> mapList = new List<char>(); // makes a list to store the player map in 1 dimension

            for (int i = 0; i < internalMap.GetLength(0); i++) // runs through every slot in the small map, and adds each room into a big list
            {
                for (int j = 0; j < internalMap.GetLength(1); j++)
                {
                    for (int k = 0; k < 7; k++)
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

                        for (int l = 0; l < toAdd.Length; l++)
                        {
                            mapList.Add(toAdd[l]);
                        }
                    }
                }
            }

            int mapListValue = 0;

            for (int k = 0; k < internalMap.GetLength(0); k++) // runs through the list and translates it into a 2D map
            {
                for (int l = 0; l < internalMap.GetLength(1); l++)
                {
                    for (int m = 0; m < 7; m++)
                    {
                        for (int n = 0; n < 7; n++)
                        {
                            int newK = k * 7;
                            int newL = l * 7;
                            playerMap[newK + m, newL + n] = mapList[mapListValue];

                            if (mapListValue >= mapList.Count) break;
                            else mapListValue++;
                        }
                    }
                }
            }

            return playerMap;
        }

        private void loadMap() // loads the map array into a public array, so it doesn't have to translate the small array to big every time a player moves
        {

            if (level > 1)
            {
                endTime = DateTime.Now;

                TimeSpan time = endTime - startTime;

                textBox($"Time to complete last dungeon:\r\n\r\n[{time:mm}:{time:ss}]");
            }

            levelMap = MapMaker();

            questionMaker();
            resetMap();

            for (int i = 0; i < levelMap.GetLength(0); i++)
            {
                for (int j = 0; j < levelMap.GetLength(1); j++)
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
            txtLevel.Text = level.ToString();
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
                else if (isGreenChestOpen && levelMap[Y, X] == '!') return Properties.Resources.green_chest_open; // chests & doors use special chars
                else if (levelMap[Y, X] == '!') return Properties.Resources.green_chest_closed;
                else if (isYellowChestOpen && levelMap[Y, X] == '@') return Properties.Resources.yellow_chest_open;
                else if (levelMap[Y, X] == '@') return Properties.Resources.yellow_chest_closed;
                else if (isRedChestOpen && levelMap[Y, X] == '#') return Properties.Resources.red_chest_open;
                else if (levelMap[Y, X] == '#') return Properties.Resources.red_chest_closed;
                else if (isPurpleChestOpen && levelMap[Y, X] == '$') return Properties.Resources.purple_chest_open;
                else if (levelMap[Y, X] == '$') return Properties.Resources.purple_chest_closed;


                else if (levelMap[Y, X] == '%' && IsFloor(X, Y + 1) && !IsFloor(X - 1, Y) && !IsFloor(X - 1, Y + 1) && !IsFloor(X + 1, Y) && !IsFloor(X + 1, Y + 1))
                {
                    if (isGreenDoorUnlocked) return Properties.Resources.green_door_open_up;
                    else return Properties.Resources.green_door_closed_up;
                }

                else if (levelMap[Y, X] == '%' && IsFloor(X, Y - 1) && !IsFloor(X - 1, Y) && !IsFloor(X - 1, Y - 1) && !IsFloor(X + 1, Y) && !IsFloor(X + 1, Y - 1))
                {
                    if (isGreenDoorUnlocked) return Properties.Resources.green_door_open_up;
                    else return Properties.Resources.green_door_closed_up;
                }

                else if (levelMap[Y, X] == '%' && IsFloor(X - 1, Y) && !IsFloor(X - 1, Y + 1) && !IsFloor(X - 1, Y - 1) && !IsFloor(X, Y + 1) && !IsFloor(X, Y - 1))
                {
                    if (isGreenDoorUnlocked) return Properties.Resources.green_door_open_right;
                    else return Properties.Resources.green_door_closed_right;
                }

                else if (levelMap[Y, X] == '%' && IsFloor(X + 1, Y) && !IsFloor(X + 1, Y + 1) && !IsFloor(X + 1, Y - 1) && !IsFloor(X, Y + 1) && !IsFloor(X, Y - 1))
                {
                    if (isGreenDoorUnlocked) return Properties.Resources.green_door_open_left;
                    else return Properties.Resources.green_door_closed_left;
                }


                else if (levelMap[Y, X] == '^' && IsFloor(X, Y + 1) && !IsFloor(X - 1, Y) && !IsFloor(X - 1, Y + 1) && !IsFloor(X + 1, Y) && !IsFloor(X + 1, Y + 1))
                {
                    if (isYellowDoorUnlocked) return Properties.Resources.yellow_door_open_up;
                    else return Properties.Resources.yellow_door_closed_up;
                }

                else if (levelMap[Y, X] == '^' && IsFloor(X, Y - 1) && !IsFloor(X - 1, Y) && !IsFloor(X - 1, Y - 1) && !IsFloor(X + 1, Y) && !IsFloor(X + 1, Y - 1))
                {
                    if (isYellowDoorUnlocked) return Properties.Resources.yellow_door_open_up;
                    else return Properties.Resources.yellow_door_closed_up;
                }

                else if (levelMap[Y, X] == '^' && IsFloor(X - 1, Y) && !IsFloor(X - 1, Y + 1) && !IsFloor(X - 1, Y - 1) && !IsFloor(X, Y + 1) && !IsFloor(X, Y - 1))
                {
                    if (isYellowDoorUnlocked) return Properties.Resources.yellow_door_open_right;
                    else return Properties.Resources.yellow_door_closed_right;
                }

                else if (levelMap[Y, X] == '^' && IsFloor(X + 1, Y) && !IsFloor(X + 1, Y + 1) && !IsFloor(X + 1, Y - 1) && !IsFloor(X, Y + 1) && !IsFloor(X, Y - 1))
                {
                    if (isYellowDoorUnlocked) return Properties.Resources.yellow_door_open_left;
                    else return Properties.Resources.yellow_door_closed_left;
                }



                else if (levelMap[Y, X] == '&' && IsFloor(X, Y + 1) && !IsFloor(X - 1, Y) && !IsFloor(X - 1, Y + 1) && !IsFloor(X + 1, Y) && !IsFloor(X + 1, Y + 1))
                {
                    if (isRedDoorUnlocked) return Properties.Resources.red_door_open_up;
                    else return Properties.Resources.red_door_closed_up;
                }

                else if (levelMap[Y, X] == '&' && IsFloor(X, Y - 1) && !IsFloor(X - 1, Y) && !IsFloor(X - 1, Y - 1) && !IsFloor(X + 1, Y) && !IsFloor(X + 1, Y - 1))
                {
                    if (isRedDoorUnlocked) return Properties.Resources.red_door_open_up;
                    else return Properties.Resources.red_door_closed_up;
                }

                else if (levelMap[Y, X] == '&' && IsFloor(X - 1, Y) && !IsFloor(X - 1, Y + 1) && !IsFloor(X - 1, Y - 1) && !IsFloor(X, Y + 1) && !IsFloor(X, Y - 1))
                {
                    if (isRedDoorUnlocked) return Properties.Resources.red_door_open_right;
                    else return Properties.Resources.red_door_closed_right;
                }

                else if (levelMap[Y, X] == '&' && IsFloor(X + 1, Y) && !IsFloor(X + 1, Y + 1) && !IsFloor(X + 1, Y - 1) && !IsFloor(X, Y + 1) && !IsFloor(X, Y - 1))
                {
                    if (isRedDoorUnlocked) return Properties.Resources.red_door_open_left;
                    else return Properties.Resources.red_door_closed_left;
                }


                else if (levelMap[Y, X] == '*' && IsFloor(X, Y + 1) && !IsFloor(X - 1, Y) && !IsFloor(X - 1, Y + 1) && !IsFloor(X + 1, Y) && !IsFloor(X + 1, Y + 1))
                {
                    if (isPurpleDoorUnlocked) return Properties.Resources.purple_door_open_up;
                    else return Properties.Resources.purple_door_closed_up;
                }

                else if (levelMap[Y, X] == '*' && IsFloor(X, Y - 1) && !IsFloor(X - 1, Y) && !IsFloor(X - 1, Y - 1) && !IsFloor(X + 1, Y) && !IsFloor(X + 1, Y - 1))
                {
                    if (isPurpleDoorUnlocked) return Properties.Resources.purple_door_open_up;
                    else return Properties.Resources.purple_door_closed_up;
                }

                else if (levelMap[Y, X] == '*' && IsFloor(X - 1, Y) && !IsFloor(X - 1, Y + 1) && !IsFloor(X - 1, Y - 1) && !IsFloor(X, Y + 1) && !IsFloor(X, Y - 1))
                {
                    if (isPurpleDoorUnlocked) return Properties.Resources.purple_door_open_right;
                    else return Properties.Resources.purple_door_closed_right;
                }

                else if (levelMap[Y, X] == '*' && IsFloor(X + 1, Y) && !IsFloor(X + 1, Y + 1) && !IsFloor(X + 1, Y - 1) && !IsFloor(X, Y + 1) && !IsFloor(X, Y - 1))
                {
                    if (isPurpleDoorUnlocked) return Properties.Resources.purple_door_open_left;
                    else return Properties.Resources.purple_door_closed_left;
                }


                else if (levelMap[Y, X] == '+' && IsFloor(X, Y + 1) && !IsFloor(X - 1, Y) && !IsFloor(X - 1, Y + 1) && !IsFloor(X + 1, Y) && !IsFloor(X + 1, Y + 1))
                {
                    if (isEndDoorUnlocked) return Properties.Resources.brown_door_open_up;
                    else return Properties.Resources.brown_door_closed_up;
                }

                else if (levelMap[Y, X] == '+' && IsFloor(X, Y - 1) && !IsFloor(X - 1, Y) && !IsFloor(X - 1, Y - 1) && !IsFloor(X + 1, Y) && !IsFloor(X + 1, Y - 1))
                {
                    if (isEndDoorUnlocked) return Properties.Resources.brown_door_open_up;
                    else return Properties.Resources.brown_door_closed_up;
                }

                else if (levelMap[Y, X] == '+' && IsFloor(X - 1, Y) && !IsFloor(X - 1, Y + 1) && !IsFloor(X - 1, Y - 1) && !IsFloor(X, Y + 1) && !IsFloor(X, Y - 1))
                {
                    if (isEndDoorUnlocked) return Properties.Resources.brown_door_open_right;
                    else return Properties.Resources.brown_door_closed_right;
                }

                else if (levelMap[Y, X] == '+' && IsFloor(X + 1, Y) && !IsFloor(X + 1, Y + 1) && !IsFloor(X + 1, Y - 1) && !IsFloor(X, Y + 1) && !IsFloor(X, Y - 1))
                {
                    if (isEndDoorUnlocked) return Properties.Resources.brown_door_open_left;
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
            inventorySort();


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
        }

        private string isFacing(int X, int Y)
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
                if (isGreenDoorUnlocked) return "null";
                else return "Green Door";
            }
            else if (levelMap[Y, X] == '^')
            {
                if (isYellowDoorUnlocked) return "null";
                else return "Yellow Door";
            }
            else if (levelMap[Y, X] == '&')
            {
                if (isRedDoorUnlocked) return "null";
                else return "Red Door";
            }
            else if (levelMap[Y, X] == '*')
            {
                if (isPurpleDoorUnlocked) return "null";
                else return "Purple Door";
            }
            else if (levelMap[Y, X] == '+')
            {
                if (isEndDoorUnlocked) return "null";
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

        private void textBox(string text = "test", bool hide = false, bool input = false)
        {
            if (hide == false) // shows textbox
            {
                txtOutput.Text = ""; // ensures textbox is empty
                btnUp.Enabled = false;
                btnDown.Enabled = false;
                btnLeft.Enabled = false;
                btnRight.Enabled = false; //disables direction buttons
                txtOutput.Visible = true; // makes text box visible
                btnAction.Enabled = false;
                btnSay.Enabled = false;
                txtInput.Enabled = false;

                if (playerHP <= 0)
                {
                    System.Threading.Thread.Sleep(1000);
                    this.Update();
                }

                for (int i = 0; i < text.Length; i++)// outputs the text 1 char at a time
                {
                    txtOutput.Text += text[i];
                    System.Threading.Thread.Sleep(15);
                    this.Update();
                }

                if (playerHP > 1)
                {
                    btnBack.Enabled = true;
                }

                if (input)
                {
                    btnSay.Enabled = true;
                    txtInput.Enabled = true;
                    txtInput.Focus();
                }
                else btnBack.Focus();
            }
            else if (hide == true) // hides textbox
            {
                btnUp.Enabled = true;
                btnDown.Enabled = true;
                btnLeft.Enabled = true;
                btnRight.Enabled = true;
                btnAction.Enabled = true;
                btnBack.Enabled = false;
                txtOutput.Visible = false;
                btnSay.Enabled = false;
                txtInput.Enabled = false;
                txtInput.Text = "";
                Map(playerX, playerY);
            }

        } // makes a textbox with any given text, hide is called by the close button only


        // button stuff
        private void allButtonsFalse()
        {
            buttonAPressed = false;
            buttonBPressed = false;
            buttonSPressed = false;
            inv1Pressed = false;
            inv2Pressed = false;
            inv3Pressed = false;
            inv4Pressed = false;
            inv5Pressed = false;
            inv6Pressed = false;
        } // sets all the buThttons to false, used when pushing any button to ensure no accidental clicks are recorded when not actually clicked.

        private void buttonPushed(string contents = "null")
        {
            int invNumber = 0; // variable for referencing the inventory
            char buttonPressed = 'n'; // variable for which button is pressed 
            if (buttonAPressed)
            {
                buttonPressed = 'a';
                buttonAPressed = false;
            }
            if (buttonBPressed)
            {
                buttonPressed = 'b';
                buttonAPressed = false;
            }
            if (buttonSPressed)
            {
                buttonPressed = 's';
                buttonAPressed = false;
            }
            else if (inv1Pressed)
            {
                buttonPressed = '1';
                invNumber = 0;
                inv1Pressed = false;
            }
            else if (inv2Pressed)
            {
                buttonPressed = '2';
                invNumber = 1;
                inv2Pressed = false;
            }
            else if (inv3Pressed)
            {
                buttonPressed = '3';
                invNumber = 2;
                inv3Pressed = false;
            }
            else if (inv4Pressed)
            {
                buttonPressed = '4';
                invNumber = 3;
                inv4Pressed = false;
            }
            else if (inv5Pressed)
            {
                buttonPressed = '5';
                invNumber = 4;
                inv5Pressed = false;
            }
            else if (inv6Pressed)
            {
                buttonPressed = '6';
                invNumber = 5;
                inv6Pressed = false;
            }

            bool isButtonUsed = itemEvents(contents); // sees if events happen with the button pressed

            if (isButtonUsed) //if the button is used and not aciton button, clear the inventory slot associated
            {
                if (buttonPressed == 'a' || buttonPressed == 'b' || buttonPressed == 's') return;
                inventory[invNumber] = "null";
                inventorySort();
            }

            Map(playerX, playerY);

        } // Detects when a button is pushed, and translates it to a more readable format for other functions.


        //inventory stuff
        private void inventorySort()
        {
            int notNull = 0;
            for (int i = 0; i < inventory.Length; i++)
            {
                if (inventory[i] != "null")
                {
                    string item = inventory[i];
                    inventory[i] = "null";
                    inventory[notNull] = item;
                    notNull++;
                }
            }

            if (inventory[0] != "null") btnInventory1.Text = inventory[0]; else btnInventory1.Text = "Slot 1";
            if (inventory[1] != "null") btnInventory2.Text = inventory[1]; else btnInventory2.Text = "Slot 2";
            if (inventory[2] != "null") btnInventory3.Text = inventory[2]; else btnInventory3.Text = "Slot 3";
            if (inventory[3] != "null") btnInventory4.Text = inventory[3]; else btnInventory4.Text = "Slot 4";
            if (inventory[4] != "null") btnInventory5.Text = inventory[4]; else btnInventory5.Text = "Slot 5";
            if (inventory[5] != "null") btnInventory6.Text = inventory[5]; else btnInventory6.Text = "Slot 6";

            if (inventory[0] != "null") btnInventory1.Enabled = true; else btnInventory1.Enabled = false;
            if (inventory[1] != "null") btnInventory2.Enabled = true; else btnInventory2.Enabled = false;
            if (inventory[2] != "null") btnInventory3.Enabled = true; else btnInventory3.Enabled = false;
            if (inventory[3] != "null") btnInventory4.Enabled = true; else btnInventory4.Enabled = false;
            if (inventory[4] != "null") btnInventory5.Enabled = true; else btnInventory5.Enabled = false;
            if (inventory[5] != "null") btnInventory6.Enabled = true; else btnInventory6.Enabled = false;

        } // sorts the inventory so there's never holes in inventory. changes 101101 to 111100 

        private bool addToInventory(string item)
        {
            bool value = true;
            for (int i = 0; i < inventory.Length; i++)
            {
                if (inventory[i] == "null") // finds the first empty inventory slot & puts item in it, returns false if inventory is full, true if item fit
                {
                    inventory[i] = item;
                    value = true;
                    break;
                }
                else
                {
                    value = false;
                }
            }
            inventorySort();
            return value;
        } // adds an item to the end of the inventory array

        private bool itemEvents(string contents = "null") // these are all the events that can happen when pushing action or inventory 1-6
        {
            if (contents == "back") // when back button hit on text box
            {
                healthFullBox = false;
                textBox("null", true);
                return true;
            }

            if (txtLookingAt.Text == "Green Door" && contents == "action")
            {
                if (isGreenDoorUnlocked == true) textBox("An open green door.");
                else textBox("A locked green door.");
                return true;
            } // viewing doors

            if (txtLookingAt.Text == "Yellow Door" && contents == "action")
            {
                if (isYellowDoorUnlocked == true) textBox("An open yellow door.");
                else textBox("A locked yellow door.");
                return true;
            }

            if (txtLookingAt.Text == "Red Door" && contents == "action")
            {
                if (isRedDoorUnlocked == true) textBox("An open red door.");
                else textBox("A locked red door.");
                return true;
            }

            if (txtLookingAt.Text == "Purple Door" && contents == "action")
            {
                if (isPurpleDoorUnlocked == true) textBox("An open purple door.");
                else textBox("A locked purple door.");
                return true;
            }

            if (txtLookingAt.Text == "Stairwell Door" && contents == "action")
            {
                if (isGreenChestOpen && isYellowChestOpen && isRedChestOpen && isPurpleChestOpen && !isTextBoxShowing)
                {
                    textBox("The dungeon can tell you've done everything on this floor... Congradulations. \r\n\r\n [Stairwell Door Unlocked]");
                    isEndDoorUnlocked = true;
                }
                else textBox("A locked stairwell door, to open it you must unlock all 4 chests. Come back when this is complete...");
                return true;
            }

            if (txtLookingAt.Text == "Green Chest" && contents == "action")
            {
                if (isGreenChestOpen == true) textBox("An open green chest.");
                else textBox("A locked green chest.");
                return true;
            } // viewing chests

            if (txtLookingAt.Text == "Yellow Chest" && contents == "action")
            {
                if (isYellowChestOpen == true) textBox("An open yellow chest.");
                else textBox("A locked yellow chest.");
                return true;
            }

            if (txtLookingAt.Text == "Red Chest" && contents == "action")
            {
                if (isRedChestOpen == true) textBox("An open red chest.");
                else textBox("A locked red chest.");
                return true;
            }

            if (txtLookingAt.Text == "Purple Chest" && contents == "action")
            {
                if (isPurpleChestOpen == true) textBox("An open purple chest.");
                else textBox("A locked purple chest.");
                return true;
            }

            if (txtLookingAt.Text == "Stairwell" && contents == "action")
            {
                level++;
                loadMap();
                return true;
            }

            if (txtLookingAt.Text == "Wall" && contents == "action")
            {
                textBox("It's a wall.");
                return true;
            }

            if (txtLookingAt.Text == "cactus" && contents == "action")
            {
                textBox("The cactus stabs you.\r\n\r\n[-50 HP]");
                RemoveHealth(50);
            }

            if (txtLookingAt.Text == "Torch" && contents == "action")
            {
                textBox("You see a torch and go wide-eyed. You touch it immediately.\r\n\r\n[-3 HP]");
                RemoveHealth(3);
            }


            if (txtLookingAt.Text == "Green Pedestal" && contents == "action") // interacting with green pedestal
            {
                if (greenConditionMet && !hasGivenGreenKey)
                {

                    bool hasAdded = addToInventory("Green Key");
                    if (hasAdded == false)
                    {
                        textBox("Your inventory is still full.");
                        return false;
                    }
                    else
                    {
                        textBox($"The pedestal remembers you getting the answer right, and appreciates the space now in your inventory\r\n\r\n[Green Key Added]\r\n[+{xpToAdd} XP]");
                    }
                    hasGivenGreenKey = true;
                    addXP(xpToAdd);
                }
                else if (!hasGivenGreenKey)
                {
                    textBox($"Inscribed on the pedestal is a problem:\r\n\r\n\"{greenQuestion}\"\r\n\r\n[Input Required]", false, true);
                    return true;
                }
                else
                {
                    textBox("There's an empty green pedestal.");
                    return true;
                }

            }// green ped action

            if (txtLookingAt.Text == "Green Pedestal" && txtInput.Enabled)
            {
                if (contents.ToLower().Trim() == greenAnswer && !hasGivenGreenKey)
                {
                    greenConditionMet = true;
                    bool hasAdded = addToInventory("Green Key");


                    if (hasAdded == false)
                    {
                        textBox("The answer was correct, but your inventory is full!\r\n\r\n[No item added.]");
                        return false;
                    }

                    textBox("The pedestal shakes as a green key pops out of it. The text on the pedestal seemingly dissappears." +
                        "\r\nYou pick up the green key." +
                        $"\r\n\r\n[Green Key Added]\r\n[+{xpToAdd} XP]");
                    hasGivenGreenKey = true;
                    addXP(xpToAdd);
                }
                else
                {
                    textBox("The pedestal makes an angry sound.\r\n\r\n[Incorrect response]\r\n[-25 HP]");
                    RemoveHealth(25);
                }
            } // green ped riddle detection

            if (txtLookingAt.Text == "Yellow Pedestal" && contents == "action") // interacting with green pedestal
            {
                if (yellowConditionMet && !hasGivenYellowKey)
                {

                    bool hasAdded = addToInventory("Yellow Key");
                    if (hasAdded == false)
                    {
                        textBox("Your inventory is still full.");
                        return false;
                    }
                    else
                    {
                        textBox("The pedestal remembers you getting the answer right, and appreciates the space now in your inventory\r\n\r\n[Yellow Key Added]\r\n[+{xpToAdd} XP]");
                    }
                    hasGivenYellowKey = true;
                    addXP(xpToAdd);
                }
                else if (!hasGivenYellowKey)
                {
                    textBox($"Inscribed on the pedestal is a problem:\r\n\r\n\"{yellowQuestion}\"\r\n\r\n[Input Required]", false, true);
                    return true;
                }
                else
                {
                    textBox("There's an empty yellow pedestal.");
                    return true;
                }

            }// yellow ped action

            if (txtLookingAt.Text == "Yellow Pedestal" && txtInput.Enabled)
            {
                if (contents.ToLower().Trim() == yellowAnswer && !hasGivenYellowKey)
                {
                    yellowConditionMet = true;
                    bool hasAdded = addToInventory("Yellow Key");

                    if (hasAdded == false)
                    {
                        textBox("The answer was correct, but your inventory is full!\r\n\r\n[No item added.]");
                        return false;
                    }

                    textBox("The pedestal shakes as a yellow key pops out of it. The text on the pedestal seemingly dissappears." +
                        "\r\nYou pick up the yellow key." +
                        $"\r\n\r\n[Yellow Key Added]\r\n[+{xpToAdd} XP]");
                    hasGivenYellowKey = true;
                    addXP(xpToAdd);
                }
                else
                {
                    textBox("The pedestal makes an angry sound.\r\n\r\n[Incorrect response]\r\n[-25 HP]");
                    RemoveHealth(25);
                }
            } // yellow ped riddle detection

            if (txtLookingAt.Text == "Red Pedestal" && contents == "action") // interacting with green pedestal
            {
                if (redConditionMet && !hasGivenRedKey)
                {

                    bool hasAdded = addToInventory("Red Key");
                    if (hasAdded == false)
                    {
                        textBox("Your inventory is still full.");
                        return false;
                    }
                    else
                    {
                        textBox($"The pedestal remembers you getting the answer right, and appreciates the space now in your inventory\r\n\r\n[Red Key Added]\r\n[+{xpToAdd} XP]");
                    }
                    hasGivenRedKey = true;
                    addXP(xpToAdd);
                }
                else if (!hasGivenRedKey)
                {
                    textBox($"Inscribed on the pedestal is a problem:\r\n\r\n\"{redQuestion}\"\r\n\r\n[Input Required]", false, true);
                    return true;
                }
                else
                {
                    textBox("There's an empty red pedestal.");
                    return true;
                }

            }// red ped action

            if (txtLookingAt.Text == "Red Pedestal" && txtInput.Enabled)
            {
                if (contents.ToLower().Trim() == redAnswer && !hasGivenRedKey)
                {
                    redConditionMet = true;
                    bool hasAdded = addToInventory("Red Key");

                    if (hasAdded == false)
                    {
                        textBox("The answer was correct, but your inventory is full!\r\n\r\n[No item added.]");
                        return false;
                    }

                    textBox("The pedestal shakes as a red key pops out of it. The text on the pedestal seemingly dissappears." +
                        "\r\nYou pick up the red key." +
                        $"\r\n\r\n[Red Key Added]\r\n[+{xpToAdd} XP]");
                    hasGivenRedKey = true;
                    addXP(xpToAdd);
                }
                else
                {
                    textBox("The pedestal makes an angry sound.\r\n\r\n[Incorrect response]\r\n[-25 HP]");
                    RemoveHealth(25);
                }
            } // red ped riddle detection

            if (txtLookingAt.Text == "Purple Pedestal" && contents == "action") // interacting with green pedestal
            {
                if (purpleConditionMet && !hasGivenPurpleKey)
                {

                    bool hasAdded = addToInventory("Purple Key");
                    if (hasAdded == false)
                    {
                        textBox("Your inventory is still full.");
                        return false;
                    }
                    else
                    {
                        textBox($"The pedestal remembers you getting the answer right, and appreciates the space now in your inventory\r\n\r\n[Purple Key Added]\r\n[+{xpToAdd} XP]");
                    }
                    hasGivenPurpleKey = true;
                    addXP(xpToAdd);
                }
                else if (!hasGivenPurpleKey)
                {
                    textBox($"Inscribed on the pedestal is a problem:\r\n\r\n\"{purpleQuestion}\"\r\n\r\n[Input Required]", false, true);
                    return true;
                }
                else
                {
                    textBox("There's an empty purple pedestal.");
                    return true;
                }

            }// purple ped action

            if (txtLookingAt.Text == "Purple Pedestal" && txtInput.Enabled)
            {
                if (contents.ToLower().Trim() == purpleAnswer && !hasGivenPurpleKey)
                {
                    purpleConditionMet = true;
                    bool hasAdded = addToInventory("Purple Key");

                    if (hasAdded == false)
                    {
                        textBox("The answer was correct, but your inventory is full!\r\n\r\n[No item added.]");
                        return false;
                    }

                    textBox("The pedestal shakes as a purple key pops out of it. The text on the pedestal seemingly dissappears." +
                        "\r\nYou pick up the purple key." +
                        $"\r\n\r\n[Purple Key Added]\r\n\r\n[+{xpToAdd} XP]");
                    addXP(xpToAdd);
                    hasGivenPurpleKey = true;
                }
                else
                {
                    textBox("The pedestal makes an angry sound.\r\n\r\n[Incorrect response]\r\n[-25 HP]");
                    RemoveHealth(25);
                }
            } // purple ped riddle detection


            if (txtLookingAt.Text == "Green Door" && contents == "Green Key")
            {
                isGreenDoorUnlocked = true;
                textBox("You put the key into the green door and turn it. You feel all green doors in the dungeon opening...\r\n\r\n [Green doors unlocked]");
                return false;
            } // if trying to use the green key on the green door

            if (txtLookingAt.Text == "Yellow Door" && contents == "Yellow Key")
            {
                isYellowDoorUnlocked = true;
                textBox("You put the key into the yellow door and turn it. You feel all yellow doors in the dungeon opening...\r\n\r\n [Yellow doors unlocked]");
                return false;
            }// if trying to use the yellow key on the yellow door

            if (txtLookingAt.Text == "Red Door" && contents == "Red Key")
            {
                isRedDoorUnlocked = true;
                textBox("You put the key into the red door and turn it. You feel all red doors in the dungeon opening...\r\n\r\n [Red doors unlocked]");
                return false;
            }// if trying to use the red key on the red door

            if (txtLookingAt.Text == "Purple Door" && contents == "Purple Key")
            {
                isPurpleDoorUnlocked = true;
                textBox("You put the key into the purple door and turn it. You feel all purple doors in the dungeon opening...\r\n\r\n [Purple doors unlocked]");
                return false;
            }// if trying to use the purple key on the purple door



            if (txtLookingAt.Text == "Green Chest" && contents == "Green Key")
            {
                bool didAdd = true;
                if (!isGreenChestOpen)
                {
                    didAdd = addToInventory(greenItem);
                }
                if (didAdd == false)
                {
                    textBox("Inventory Full!");
                    return false;
                }
                else if (isGreenChestOpen == false)
                {
                    int itemID = findItem(greenItem);
                    textBox($"You open the green chest, and you get a {greenItem}! {Items[itemID, 3]}\r\n" +
                        "The key gets stuck in the chest.\r\n\r\n" +
                        $"[Green Key Removed.]\r\n[{greenItem} added.]");
                    isGreenChestOpen = true;
                    return true;
                }
            }// if trying to use the green key on the green chest

            if (txtLookingAt.Text == "Yellow Chest" && contents == "Yellow Key")
            {
                bool didAdd = true;
                if (!isYellowChestOpen)
                {
                    didAdd = addToInventory(yellowItem);
                }
                if (didAdd == false)
                {
                    textBox("Inventory Full!");
                    return false;
                }
                else if (isYellowChestOpen == false)
                {
                    int itemID = findItem(yellowItem);
                    textBox($"You open the yellow chest, and you get a {yellowItem}! {Items[itemID, 3]}\r\n" +
                        "The key gets stuck in the chest.\r\n\r\n" +
                        $"[Yellow Key Removed.]\r\n[{yellowItem} added.]");
                    isYellowChestOpen = true;
                    return true;
                }
            }// if trying to use the red key on the red chest

            if (txtLookingAt.Text == "Red Chest" && contents == "Red Key")
            {
                bool didAdd = true;
                if (!isRedChestOpen)
                {
                    didAdd = addToInventory(redItem);
                }
                if (didAdd == false)
                {
                    textBox("Inventory Full!");
                    return false;
                }
                else if (isRedChestOpen == false)
                {
                    int itemID = findItem(redItem);
                    textBox($"You open the red chest, and you get a {redItem}! {Items[itemID, 3]}\r\n" +
                        "The key gets stuck in the chest.\r\n\r\n" +
                        $"[Red Key Removed.]\r\n[{redItem} added.]");
                    isRedChestOpen = true;
                    return true;
                }
            }// if trying to use the yellow key on the yellow chest

            if (txtLookingAt.Text == "Purple Chest" && contents == "Purple Key")
            {
                bool didAdd = true;
                if (!isPurpleChestOpen)
                {
                    didAdd = addToInventory(purpleItem);
                }
                if (didAdd == false)
                {
                    textBox("Inventory Full!");
                    return false;
                }
                else if (isPurpleChestOpen == false)
                {
                    int itemID = findItem(purpleItem);
                    textBox($"You open the purple chest, and you get a {purpleItem}! {Items[itemID, 3]}\r\n" +
                        "The key gets stuck in the chest.\r\n\r\n" +
                        $"[Purple Key Removed.]\r\n[{purpleItem} added.]");
                    isPurpleChestOpen = true;
                    return true;
                }
            }// if trying to use the purple key on the purple chest

            if (healthFullBox)
            {
                if (contents.ToLower().Trim() == "y" || contents.ToLower().Trim() == "yes")
                {
                    // item toss

                    if (btnInventory1.Text == itemToRemove)
                    {
                        inventory[0] = "null";
                        inventorySort();
                    }
                    else if (btnInventory2.Text == itemToRemove)
                    {
                        inventory[1] = "null";
                        inventorySort();
                    }
                    else if (btnInventory3.Text == itemToRemove)
                    {
                        inventory[2] = "null";
                        inventorySort();
                    }
                    else if (btnInventory4.Text == itemToRemove)
                    {
                        inventory[3] = "null";
                        inventorySort();
                    }
                    else if (btnInventory5.Text == itemToRemove)
                    {
                        inventory[4] = "null";
                        inventorySort();
                    }
                    else if (btnInventory6.Text == itemToRemove)
                    {
                        inventory[5] = "null";
                        inventorySort();
                    }
                    healthFullBox = false;
                    textBox($"{itemToRemove} tossed.\r\n\r\n");
                    itemToRemove = "";
                    return true;
                }
                else
                {
                    healthFullBox = false;
                    textBox("", true);
                    itemToRemove = "";
                    return false;
                }
            }

            if (contents != "Green Key" && contents != "Yellow Key" && contents != "Red Key" && contents != "Purple Key" && contents != "action" && contents != "back" && !buttonSPressed)
            {
                int itemNumber = findItem(contents);
                int itemHealth = Convert.ToInt32(Items[itemNumber, 1]);
                return AddHealth(itemHealth, contents, $"You {Items[itemNumber, 4]} the {Items[itemNumber, 0]}. {Items[itemNumber, 2]}");
            }

            return false; // returns true if item is used, false when item is not used
        }


        // Event handlers
        private void btnRestart_Click(object sender, EventArgs e) // resets all variables to default state -- also changes button text the first time for better UX
        {
            level = spawnLevel;


            loadMap();


            btnRestart.Text = "Restart";
            playerHP = spawnHP;
            level = spawnLevel;
            playerXP = spawnPlayerXP;
            maxXP = spawnMaxXP;
            playerLevel = spawnPlayerLevel;
            xpToAdd = spawnXPToAdd;
            playerDirection = Properties.Resources.player_down_t;
            buttonAPressed = false;
            inv1Pressed = false;
            inv2Pressed = false;
            inv3Pressed = false;
            inv4Pressed = false;
            inv5Pressed = false;
            inv6Pressed = false;
            isTextBoxShowing = false;
            resetMap();

            btnUp.Enabled = true;
            btnDown.Enabled = true;
            btnLeft.Enabled = true;
            btnRight.Enabled = true;
            btnAction.Enabled = true;
            btnSay.Enabled = false;
            btnBack.Enabled = false;

            timer1.Start();
            stopwatch.Start();

            textBox(" ", true);
            infoBoxUpdate();
            pgbPlayerHP.Value = playerHP;

            inventory[0] = "null";
            inventory[1] = "null";
            inventory[2] = "null";
            inventory[3] = "null";
            inventory[4] = "null";
            inventory[5] = "null";
            if (isFacing(playerX, playerY - 1) == "null") txtLookingAt.Text = " - ";
            else txtLookingAt.Text = isFacing(playerX, playerY - 1);

            Map(playerX, playerY);

            if (!howToPlayShown)
            {
                textBox("HOW TO PLAY:\r\n\r\n" +
                "TO MOVE: Use the on screen arrow buttons, or WASD\r\n " +
                "TO INTERACT: Press the on screen 'A' button, or 'E'\r\n" +
                "TO USE AN ITEM: Click on it in your inventory, or press the corresponding number button\r\n" +
                "Press the back arrow or ESC to close dialogue boxes, like this one.");
                howToPlayShown = true;
            }

        }

        private void resetMap()
        {
            isGreenChestOpen = false;
            isYellowChestOpen = false;
            isRedChestOpen = false;
            isPurpleChestOpen = false;
            isGreenDoorUnlocked = false;
            isYellowDoorUnlocked = false;
            isRedDoorUnlocked = false;
            isPurpleDoorUnlocked = false;
            isEndDoorUnlocked = false;
            hasGivenGreenKey = false;
            hasGivenYellowKey = false;
            hasGivenRedKey = false;
            hasGivenPurpleKey = false;
            greenConditionMet = false;
            yellowConditionMet = false;
            redConditionMet = false;
            purpleConditionMet = false;
            greenItem = chestItemPool();
            yellowItem = chestItemPool();
            redItem = chestItemPool();
            purpleItem = chestItemPool();
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            allButtonsFalse();
            playerDirection = Properties.Resources.player_up_t; // Changes player direction

            if (isFacing(playerX, playerY + 1) == "null")
            {
                playerY++;
            }

            if (isFacing(playerX, playerY + 1) == "null") txtLookingAt.Text = " - "; else txtLookingAt.Text = isFacing(playerX, playerY + 1);

            stopwatch.Restart();

            Map(playerX, playerY);
        } // all direction buttons: changes player direction & detects 

        private void btnDown_Click(object sender, EventArgs e)
        {
            allButtonsFalse();
            playerDirection = Properties.Resources.player_down_t;

            if (isFacing(playerX, playerY - 1) == "null")
            {
                playerY--;
            }

            if (isFacing(playerX, playerY - 1) == "null") txtLookingAt.Text = " - "; else txtLookingAt.Text = isFacing(playerX, playerY - 1);

            stopwatch.Restart();

            Map(playerX, playerY);
        }

        private void btnLeft_Click(object sender, EventArgs e)
        {
            allButtonsFalse();
            playerDirection = Properties.Resources.player_left_t;

            if (isFacing(playerX - 1, playerY) == "null")
            {
                playerX--;
            }

            if (isFacing(playerX - 1, playerY) == "null") txtLookingAt.Text = " - "; else txtLookingAt.Text = isFacing(playerX - 1, playerY);

            stopwatch.Restart();

            Map(playerX, playerY);
        }

        private void btnRight_Click(object sender, EventArgs e)
        {
            allButtonsFalse();
            playerDirection = Properties.Resources.player_right_t;
            if (isFacing(playerX + 1, playerY) == "null")
            {
                playerX++;
            }

            if (isFacing(playerX + 1, playerY) == "null") txtLookingAt.Text = " - "; else txtLookingAt.Text = isFacing(playerX + 1, playerY);

            stopwatch.Restart();

            Map(playerX, playerY);
        }

        private void btnAction_Click(object sender, EventArgs e)
        {
            //changes the action button to pushed, all others to false. All others are changed to false on all button presses, inclusing movement
            //      to prevent misunderstandings for both the program and the end user.
            allButtonsFalse();
            buttonAPressed = true;
            buttonPushed("action");

            stopwatch.Restart();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            allButtonsFalse();
            buttonBPressed = true;
            buttonPushed("back");

            stopwatch.Restart();
        }

        private void txtSay_Click(object sender, EventArgs e)
        {
            allButtonsFalse();
            buttonSPressed = true;
            buttonPushed(txtInput.Text);
            txtInput.Text = "";

            stopwatch.Restart();
        }

        private void btnInventory1_Click(object sender, EventArgs e) // says inventory 1 button is pushed, all inventory buttons follow the same logic
        {
            allButtonsFalse();
            inv1Pressed = true;
            buttonPushed(inventory[0]);
            stopwatch.Restart();
        }

        private void btnInventory2_Click(object sender, EventArgs e)
        {
            allButtonsFalse();
            inv2Pressed = true;
            buttonPushed(inventory[1]);

            stopwatch.Restart();
        }

        private void btnInventory3_Click(object sender, EventArgs e)
        {
            allButtonsFalse();
            inv3Pressed = true;
            buttonPushed(inventory[2]);

            stopwatch.Restart();
        }

        private void btnInventory4_Click(object sender, EventArgs e)
        {
            allButtonsFalse();
            inv4Pressed = true;
            buttonPushed(inventory[3]);

            stopwatch.Restart();
        }

        private void btnInventory5_Click(object sender, EventArgs e)
        {
            allButtonsFalse();
            inv5Pressed = true;
            buttonPushed(inventory[4]);

            stopwatch.Restart();
        }

        private void btnInventory6_Click(object sender, EventArgs e)
        {
            allButtonsFalse();
            inv6Pressed = true;
            buttonPushed(inventory[5]);

            stopwatch.Restart();
        }

        private void timer1_Tick(object sender, EventArgs e) // a timer that ticks every half seccond, used for timed events, right now mostly for just making the fox yawn
        {


            if (levelUp && !txtOutput.Visible) { LevelUp(); infoBoxUpdate(); levelUp = false; }

            Random random = new Random();

            pbxPlayer.Size = pbx42.Size;


            if (stopwatch.ElapsedMilliseconds > randomTime)
            {
                pbxPlayer.Image = Properties.Resources.Yawnnn;
                randomTime = random.Next(15000, 30001);
                stopwatch.Restart();

            }

            if (stopwatch.ElapsedMilliseconds < 20000 && stopwatch.ElapsedMilliseconds > 1500)
            {
                pbxPlayer.Image = playerDirection;
            }


            if (playerHP == 0 && !txtOutput.Visible)
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
                btnSay.Enabled = false;
                btnBack.Enabled = false;
                btnInventory1.Enabled = false;
                btnInventory2.Enabled = false;
                btnInventory3.Enabled = false;
                btnInventory4.Enabled = false;
                btnInventory5.Enabled = false;
                btnInventory6.Enabled = false;
                textBox("You Died!\r\n\r\nPress restart to try again!");
                btnRestart.Focus();
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
                if (btnBack.Enabled && !txtInput.Enabled) btnBack.PerformClick();
                else btnAction.PerformClick();
            }

            if (e.KeyCode == Keys.Escape)
            {
                btnBack.PerformClick();
            }

            if (e.KeyCode == Keys.Enter)
            {
                btnSay.PerformClick();
            }
        }

        private void btnDebug_Click(object sender, EventArgs e)
        {

        }

        private void pbx40_Click(object sender, EventArgs e)
        {

        }

        private void btnUnlock_Click(object sender, EventArgs e)
        {
            isEndDoorUnlocked = true;
            isGreenDoorUnlocked = true;
            isYellowDoorUnlocked = true;
            isRedDoorUnlocked = true;
            isPurpleDoorUnlocked = true;
        }
    }
}