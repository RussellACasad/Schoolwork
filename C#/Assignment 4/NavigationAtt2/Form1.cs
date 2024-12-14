// Russell Casad
// CPT-230-W01
// 2023 FA
using NavigationAtt2.Properties;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace NavigationAtt2
{
    public partial class Form1 : Form
    {

        // initializing variables to detect when things are done
        public System.Drawing.Image playerDirection = Properties.Resources.player_up_t;
        public int randomNum = 0;
        public int spawnX = 0;
        public int spawnY = 0;
        public int playerX = 0;
        public int playerY = 0;
        public int spawnLevel = 1;
        public int level = 0;
        public int spawnHP = 100;
        public int playerHP = 0;
        public int randomTime = 20000;
        public bool buttonAPressed = false;
        public bool buttonBPressed = false;
        public bool buttonSPressed = false;
        public bool inv1Pressed = false;
        public bool inv2Pressed = false;
        public bool inv3Pressed = false;
        public bool inv4Pressed = false;
        public bool inv5Pressed = false;
        public bool inv6Pressed = false;
        public bool isGreenChestOpen = false;
        public bool isYellowChestOpen = false;
        public bool isRedChestOpen = false;
        public bool isPurpleChestOpen = false;
        public bool isGreenDoorUnlocked = false;
        public bool isYellowDoorUnlocked = false;
        public bool isRedDoorUnlocked = false;
        public bool isPurpleDoorUnlocked = false;
        public bool isTextBoxShowing = false;
        public bool hasGivenGreenKey = false;
        public bool hasGivenYellowKey = false;
        public bool hasGivenRedKey = false;
        public bool hasGivenPurpleKey = false;
        public bool greenConditionMet = false;
        public bool yellowConditionMet = false;
        public bool redConditionMet = false;
        public bool purpleConditionMet = false;
        public bool isTakingInput = false;
        public bool howToPlayShown = false;
        public readonly Stopwatch stopwatch = new Stopwatch();
        public string[] inventory = { "null", "null", "null", "null", "null", "null" };
        public char[,] levelMap;



        public Form1()
        {
            InitializeComponent();
            pbx42.Controls.Add(pbxPlayer);
            pbxPlayer.Location = new Point(0, 0);
            txtOutput.Location = new Point(12, 292);
            pbxPlayer.BackColor = Color.Transparent; // makes the player picturebox have a transparent background so the floor doesn't look weird
        }



        /*
         * Map array:
         * ' ' - used for the automatic wall generation, anywhere you don't want to be a floor use this
         * 1 - floor
         * ! - green chest
         * @ - yellow chest
         * # - red chest
         * $ - purple chest
         * % - green door
         * ^ - yellow door
         * & - red door
         * * - purple door
         * - - green ped
         * = - yelow ped
         * [ - red ped
         * ] - purple ped
         * t - wall with torch
         * s - player spawnpoint
         * 
         * 
         * !! Doors autmoatically pick direction, ensure when a door is placed, it has 1 floor behind it, and walls to the left and right of it, as well as behind those walls
         * example:
         * 
         * '1', '1', '1'
         * ' ', '%', ' '
         * ' ', '1', ' '
         * 
         * properly placed green door with walls
         */


        private char[,] Levels() // these are the array maps for levels -- multiple levels framework for future update.
        {
            char[,] level1 =
        {
            {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', },
            {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '1', '1', '1', '1', '1', ' ', },
            {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', ' ', },
            {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '1', '1', '1', '1', '1', '1', '1', '[', '1', '1', ' ', },
            {' ', ' ', ' ', ' ', ' ', ' ', '1', '1', '1', '1', '1', ' ', ' ', '1', '1', '1', '1', '1', ' ', ' ', '1', '1', '1', '1', '1', ' ', ' ', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', ' ', },
            {' ', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', ' ', ' ', '1', '1', '1', '1', '1', ' ', ' ', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', ' ', ' ', '1', '1', '1', '1', '1', ' ', },
            {' ', '1', '1', '1', '1', '1', '1', '1', '-', '1', '1', ' ', ' ', '1', '1', '=', '1', '1', '^', '1', '1', '1', '@', '1', '1', '1', '1', '1', '1', '1', ' ', ' ', ' ', 't', '&', 't', ' ', ' ', },
            {' ', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', ' ', ' ', '1', '1', '1', '1', '1', ' ', ' ', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', ' ', ' ', ' ', ' ', '1', ' ', ' ', ' ', },
            {' ', '1', 's', '1', ' ', ' ', '1', '1', '1', '1', '1', ' ', ' ', '1', '1', '1', '1', '1', ' ', ' ', '1', '1', '1', '1', '1', ' ', 't', ' ', 't', ' ', ' ', ' ', '1', '1', '1', '1', '1', ' ', },
            {' ', '1', '1', '1', ' ', ' ', ' ', 't', '%', 't', ' ', ' ', ' ', ' ', '1', '1', '1', ' ', ' ', ' ', ' ', 't', ' ', 't', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '1', '1', '1', '1', '1', ' ', },
            {' ', 't', ' ', 't', ' ', ' ', ' ', ' ', '1', ' ', ' ', ' ', ' ', ' ', '1', '1', '1', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '1', '1', '#', '1', '1', ' ', },
            {' ', ' ', ' ', ' ', ' ', ' ', '1', '1', '1', '1', '1', ' ', ' ', ' ', '1', '1', '1', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '1', '1', '1', '1', '1', ' ', },
            {' ', ' ', ' ', ' ', ' ', ' ', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '1', '1', '1', '1', '1', ' ', },
            {' ', ' ', ' ', ' ', ' ', ' ', '1', '1', '!', '1', '1', '1', '1', '1', '1', '1', '1', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '1', '1', '1', ' ', ' ', },
            {' ', ' ', ' ', ' ', ' ', ' ', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '1', '1', '1', '1', '1', ' ', ' ', ' ', '1', '1', '1', ' ', ' ', },
            {' ', ' ', ' ', ' ', ' ', ' ', '1', '1', '1', '1', '1', ' ', 't', ' ', 't', ' ', 't', ' ', ' ', ' ', '1', '1', '1', ' ', ' ', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', ' ', ' ', },
            {' ', ' ', ' ', ' ', ' ', ' ', ' ', 't', ' ', 't', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '1', '$', '1', '1', '*', '1', '1', ']', '1', '1', '1', '1', '1', '1', '1', '1', ' ', ' ', },
            {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '1', '1', '1', ' ', ' ', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', ' ', ' ', },
            {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', 't', ' ', ' ', ' ', '1', '1', '1', '1', '1', ' ', 't', ' ', 't', ' ', 't', ' ', ' ', },
            {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', 't', ' ', 't', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', },
            {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', },
        };

            char[,] boxRoom =
            {
                {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
                {' ', ' ', ' ', ' ', '1', ' ', '1', ' ', '1', ' ', '1', ' ', ' ', ' ', ' '},
                {' ', ' ', ' ', ' ', '%', ' ', '^', ' ', '&', ' ', '*', ' ', ' ', ' ', ' '},
                {' ', ' ', ' ', 's', '1', '1', '1', '1', '1', '1', '1', '1', ' ', ' ', ' '},
                {' ', '1', '%', '1', ' ', '1', ' ', '1', ' ', '1', ' ', '1', '%', '1', ' '},
                {' ', ' ', ' ', '1', ' ', '1', ' ', '1', ' ', '1', ' ', '1', ' ', ' ', ' '},
                {' ', '1', '^', '1', ' ', '1', ' ', '1', ' ', '1', '1', '1', '^', '1', ' '},
                {' ', ' ', ' ', '1', ' ', '1', ' ', ' ', ' ', ' ', ' ', '1', ' ', ' ', ' '},
                {' ', '1', '&', '1', ' ', '1', ' ', '1', ' ', '1', ' ', '1', '&', '1', ' '},
                {' ', ' ', ' ', '1', ' ', '1', ' ', '1', ' ', '1', ' ', '1', ' ', ' ', ' '},
                {' ', '1', '*', '1', ' ', '1', ' ', '1', ' ', '1', ' ', '1', '*', '1', ' '},
                {' ', ' ', ' ', '1', '1', '1', '1', '1', '1', '1', '1', '1', ' ', ' ', ' '},
                {' ', ' ', ' ', ' ', '%', ' ', '^', ' ', '&', ' ', '*', ' ', ' ', ' ', ' '},
                {' ', ' ', ' ', ' ', '1', ' ', '1', ' ', '1', ' ', '1', ' ', ' ', ' ', ' '},
                {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
            };

            // txtPlayerLevel.Text = level.ToString();

            if (level == 1) return level1;
            else return boxRoom;
        }

        private void loadMap(char levelMove = ' ') // a new mechanic to load a map -- a part of the multilevel framework for a future update.
        {

            if (levelMove == '+') level += 1; // moves to the next map
            if (levelMove == '-') level -= 1; // moves to the previous map

            levelMap = Levels();
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
            Map(playerX, playerY);
        }

        private bool AddHealth(int amount, string item, string message) // for add health items
        {
            if (playerHP == 100) // keeps item from being used if health is already 100
            {
                textBox("Health already full!\r\n[No item removed]");
                return false;
            }

            playerHP += amount; // adds HP
            if (playerHP > 100) // if HP is over 100, sets it to 100 to prevent the progrssbar from crashing
            {
                playerHP = 100;
            }
            boxHP.Text = "HP - " + playerHP.ToString(); // sets HP box to say new HP
            pgbPlayerHP.Value = playerHP; // sets the progressbar to show HP 
            textBox(message + $"\r\n\r\n[{item} removed]\r\n[+{amount} HP]"); // tells the user that the health was added
            return true;
        }

        private void RemoveHealth(int amount) // for removing health -- also deals with death
        {
            playerHP -= amount;

            try
            {
                pgbPlayerHP.Value = playerHP;
            }
            catch
            {
                playerHP = 1;

                boxHP.Text = "HP - 0";
                pgbPlayerHP.Value = playerHP;

                btnAction.Enabled = false;
                btnInventory1.Enabled = false;
                btnInventory2.Enabled = false;
                btnInventory3.Enabled = false;
                btnInventory4.Enabled = false;
                btnInventory5.Enabled = false;
                btnInventory6.Enabled = false;
                btnUp.Enabled = false;
                btnDown.Enabled = false;
                btnLeft.Enabled = false;
                btnRight.Enabled = false;

                textBox("You died!\r\n\r\nRestart to try again!");
                btnBack.Enabled = false;
                btnInventory1.Enabled = false;
                btnInventory2.Enabled = false;
                btnInventory3.Enabled = false;
                btnInventory4.Enabled = false;
                btnInventory5.Enabled = false;
                btnInventory6.Enabled = false;
            }

            if (playerHP > 1)
            {
                boxHP.Text = "HP - " + playerHP.ToString();
                pgbPlayerHP.Value = playerHP;
            }

        }


        // Map stuff
        private bool IsFloor(int X, int Y) // a part of the map maker, returns true if a floor tile is at a given coord, false if not.
        {
            if (levelMap[Y, X] == '1' || levelMap[Y, X] == 's' || levelMap[Y, X] == 'c' || levelMap[Y, X] == '!' || levelMap[Y, X] == '@' || levelMap[Y, X] == '#' || levelMap[Y, X] == '$' || levelMap[Y, X] == '%' || levelMap[Y, X] == '^' || levelMap[Y, X] == '&' || levelMap[Y, X] == '*' || levelMap[Y, X] == '-' || levelMap[Y, X] == '=' || levelMap[Y, X] == '[' || levelMap[Y, X] == ']')
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
                    else if (IsFloor(X - 1, Y + 1) && IsFloor(X - 1, Y - 1) && !IsFloor(X - 1, Y) && !IsFloor(X, Y + 1) && !IsFloor(X, Y - 1) && IsFloor(X + 1, Y + 1) && IsFloor(X + 1, Y - 1) && !IsFloor(X + 1, Y)) return Properties.Resources.dot_all;
                    else if (IsFloor(X - 1, Y + 1) && IsFloor(X - 1, Y - 1) && !IsFloor(X - 1, Y) && !IsFloor(X, Y + 1) && !IsFloor(X, Y - 1) && !IsFloor(X + 1, Y + 1) && !IsFloor(X + 1, Y - 1) && !IsFloor(X + 1, Y)) return Properties.Resources.dot_TL_BL;
                    else if (IsFloor(X - 1, Y + 1) && IsFloor(X - 1, Y - 1) && !IsFloor(X - 1, Y) && !IsFloor(X, Y + 1) && !IsFloor(X, Y - 1) && !IsFloor(X + 1, Y + 1) && IsFloor(X + 1, Y - 1) && !IsFloor(X + 1, Y)) return Properties.Resources.dot_TL_BL_BR;
                    else if (IsFloor(X - 1, Y + 1) && !IsFloor(X - 1, Y - 1) && !IsFloor(X - 1, Y) && !IsFloor(X, Y + 1) && !IsFloor(X, Y - 1) && !IsFloor(X + 1, Y + 1) && IsFloor(X + 1, Y - 1) && !IsFloor(X + 1, Y)) return Properties.Resources.dot_TL_BR;
                    else if (IsFloor(X - 1, Y + 1) && !IsFloor(X - 1, Y - 1) && !IsFloor(X - 1, Y) && !IsFloor(X, Y + 1) && !IsFloor(X, Y - 1) && IsFloor(X + 1, Y + 1) && !IsFloor(X + 1, Y - 1) && !IsFloor(X + 1, Y)) return Properties.Resources.dot_TL_TR;
                    else if (!IsFloor(X - 1, Y + 1) && IsFloor(X - 1, Y - 1) && !IsFloor(X - 1, Y) && !IsFloor(X, Y + 1) && !IsFloor(X, Y - 1) && !IsFloor(X + 1, Y + 1) && IsFloor(X + 1, Y - 1) && !IsFloor(X + 1, Y)) return Properties.Resources.dot_BL_BR;
                    else if (IsFloor(X - 1, Y + 1) && IsFloor(X - 1, Y - 1) && !IsFloor(X - 1, Y) && !IsFloor(X, Y + 1) && !IsFloor(X, Y - 1) && IsFloor(X + 1, Y + 1) && !IsFloor(X + 1, Y - 1) && !IsFloor(X + 1, Y)) return Properties.Resources.dot_TL_TR_BL;
                    else if (IsFloor(X - 1, Y + 1) && !IsFloor(X - 1, Y - 1) && !IsFloor(X - 1, Y) && !IsFloor(X, Y + 1) && !IsFloor(X, Y - 1) && IsFloor(X + 1, Y + 1) && IsFloor(X + 1, Y - 1) && !IsFloor(X + 1, Y)) return Properties.Resources.dot_TL_TR_BR;
                    else if (!IsFloor(X - 1, Y + 1) && IsFloor(X - 1, Y - 1) && !IsFloor(X - 1, Y) && !IsFloor(X, Y + 1) && !IsFloor(X, Y - 1) && IsFloor(X + 1, Y + 1) && !IsFloor(X + 1, Y - 1) && !IsFloor(X + 1, Y)) return Properties.Resources.dot_TR_BL;
                    else if (!IsFloor(X - 1, Y + 1) && IsFloor(X - 1, Y - 1) && !IsFloor(X - 1, Y) && !IsFloor(X, Y + 1) && !IsFloor(X, Y - 1) && IsFloor(X + 1, Y + 1) && IsFloor(X + 1, Y - 1) && !IsFloor(X + 1, Y)) return Properties.Resources.dot_TR_BL_BR;
                    else if (!IsFloor(X - 1, Y + 1) && !IsFloor(X - 1, Y - 1) && !IsFloor(X - 1, Y) && !IsFloor(X, Y + 1) && !IsFloor(X, Y - 1) && IsFloor(X + 1, Y + 1) && IsFloor(X + 1, Y - 1) && !IsFloor(X + 1, Y)) return Properties.Resources.dot_TR_BR;
                    else if (IsFloor(X + 1, Y) && IsFloor(X - 1, Y) && !IsFloor(X, Y + 1) && !IsFloor(X, Y - 1)) return Properties.Resources.Wall_L_R;
                    //else if (IsFloor(X + 1, Y) && !IsFloor(X - 1, Y) && !IsFloor(X, Y + 1) && !IsFloor(X, Y - 1)) return Properties.Resources.Wall_L_R;
                    //else if (!IsFloor(X + 1, Y) && IsFloor(X - 1, Y) && !IsFloor(X, Y + 1) && !IsFloor(X, Y - 1)) return Properties.Resources.Wall_L_R;
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
                    if (isGreenDoorUnlocked) return Properties.Resources.green_door_open_down;
                    else return Properties.Resources.green_door_closed_down;
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
                    if (isYellowDoorUnlocked) return Properties.Resources.yellow_door_open_down;
                    else return Properties.Resources.yellow_door_closed_down;
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
                    if (isRedDoorUnlocked) return Properties.Resources.red_door_open_down;
                    else return Properties.Resources.red_door_closed_down;
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
                    if (isPurpleDoorUnlocked) return Properties.Resources.purple_door_open_down;
                    else return Properties.Resources.purple_door_closed_down;
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


                else if (isGreenDoorUnlocked && levelMap[Y, X] == '%') return Properties.Resources.green_door_open;
                else if (levelMap[Y, X] == '%') return Properties.Resources.green_door_closed;
                else if (isYellowDoorUnlocked && levelMap[Y, X] == '^') return Properties.Resources.yellow_door_open;
                else if (levelMap[Y, X] == '^') return Properties.Resources.yellow_door_closed;
                else if (isRedDoorUnlocked && levelMap[Y, X] == '&') return Properties.Resources.red_door_open;
                else if (levelMap[Y, X] == '&') return Properties.Resources.red_door_closed;
                else if (isPurpleDoorUnlocked && levelMap[Y, X] == '*') return Properties.Resources.purple_door_open;
                else if (levelMap[Y, X] == '*') return Properties.Resources.purple_door_closed;

                else if (levelMap[Y, X] == '-') return Properties.Resources.green_ped; // peds use the end of the alphabet
                else if (levelMap[Y, X] == '=') return Properties.Resources.Yellow_ped;
                else if (levelMap[Y, X] == '[') return Properties.Resources.red_ped;
                else if (levelMap[Y, X] == ']') return Properties.Resources.purple_ped;

                else if (levelMap[Y, X] == 'c') return Properties.Resources.cactus;
                else if (levelMap[Y, X] == 't') return Properties.Resources.torch_wall;

                else return Properties.Resources.no_wall;// if char not found, it returns this to prevent crashes
            }
        }

        private void Map(int X, int Y) // The map updater, this is what calls the map maker for each square on the map
        {
            inventorySort();
            levelMap = Levels();
            txtCoordX.Text = X.ToString(); // sets X and Y coord boxes
            txtCoordY.Text = Y.ToString();

            pbx0n1.Image = MapRooms(X - 4, Y - 3); // sets each photobox to the proper image for it using x&y coods
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
                return "wall";
            }
            else if (X > levelMap.GetLength(1) - 1 || X < 0)
            {
                return "wall";
            }
            else if (levelMap[Y, X] == ' ')
            {
                return "wall";
            }
            else if (levelMap[Y, X] == '!')
            {
                return "green chest";
            }
            else if (levelMap[Y, X] == '@')
            {
                return "yellow chest";
            }
            else if (levelMap[Y, X] == '#')
            {
                return "red chest";
            }
            else if (levelMap[Y, X] == '$')
            {
                return "purple chest";
            }
            else if (levelMap[Y, X] == '%')
            {
                if (isGreenDoorUnlocked) return "null";
                else return "green door";
            }
            else if (levelMap[Y, X] == '^')
            {
                if (isYellowDoorUnlocked) return "null";
                else return "yellow door";
            }
            else if (levelMap[Y, X] == '&')
            {
                if (isRedDoorUnlocked) return "null";
                else return "red door";
            }
            else if (levelMap[Y, X] == '*')
            {
                if (isPurpleDoorUnlocked) return "null";
                else return "purple door";
            }
            else if (levelMap[Y, X] == '-')
            {
                return "green pedestal";
            }
            else if (levelMap[Y, X] == '=')
            {
                return "yellow pedestal";
            }
            else if (levelMap[Y, X] == '[')
            {
                return "red pedestal";
            }
            else if (levelMap[Y, X] == ']')
            {
                return "purple pedestal";
            }
            else if (levelMap[Y, X] == 'c')
            {
                return "cactus";
            }
            else if (levelMap[Y, X] == 't')
            {
                return "torch";
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
                textBox("null", true);
                return true;
            }

            if (txtLookingAt.Text == "green door" && contents == "action")
            {
                if (isGreenDoorUnlocked == true) textBox("An open green door.");
                else textBox("A locked green door.");
                return true;
            } // viewing doors

            if (txtLookingAt.Text == "yellow door" && contents == "action")
            {
                if (isYellowDoorUnlocked == true) textBox("An open yellow door.");
                else textBox("A locked yellow door.");
                return true;
            }

            if (txtLookingAt.Text == "red door" && contents == "action")
            {
                if (isRedDoorUnlocked == true) textBox("An open red door.");
                else textBox("A locked red door.");
                return true;
            }

            if (txtLookingAt.Text == "purple door" && contents == "action")
            {
                if (isPurpleDoorUnlocked == true) textBox("An open purple door.");
                else textBox("A locked purple door.");
                return true;
            }

            if (txtLookingAt.Text == "green chest" && contents == "action")
            {
                if (isGreenChestOpen == true) textBox("An open green chest.");
                else textBox("A locked green chest.");
                return true;
            } // viewing chests

            if (txtLookingAt.Text == "yellow chest" && contents == "action")
            {
                if (isYellowChestOpen == true) textBox("An open yellow chest.");
                else textBox("A locked yellow chest.");
                return true;
            }

            if (txtLookingAt.Text == "red chest" && contents == "action")
            {
                if (isRedChestOpen == true) textBox("An open red chest.");
                else textBox("A locked red chest.");
                return true;
            }

            if (txtLookingAt.Text == "purple chest" && contents == "action")
            {
                if (isPurpleChestOpen == true) textBox("An open purple chest.");
                else textBox("A locked purple chest.");
                return true;
            }

            if (txtLookingAt.Text == "wall" && contents == "action")
            {
                textBox("It's a wall.");
                return true;
            }

            if (txtLookingAt.Text == "cactus" && contents == "action")
            {
                loadMap('+');
            }

            if (txtLookingAt.Text == "torch" && contents == "action")
            {
                textBox("You see a torch and go wide-eyed. You touch it immediately.\r\n\r\n[-3 HP]");
                RemoveHealth(3);
            }

            if (contents == "health potion")
            {
                return AddHealth(25, contents, "You drink the health potion and gain 25 HP");
            }

            if (contents == "green apple")
            {
                return AddHealth(10, contents, "You eat the green apple. Crunchy!");
            }

            if (contents == "lime")
            {
                return AddHealth(8, contents, "You eat the lime, it tastes... weird...");
            }

            if (contents == "unknown gameboy game")
            {
                return AddHealth(2, contents, "You put the gameboy game in your mouth and chew on it, eventually swallowing. The gunk on the cartridge gives you some hp...");
            }

            if (contents == "junk")
            {
                return AddHealth(3, contents, "You crunk the junk");
            }




            if (txtLookingAt.Text == "green pedestal" && contents == "action") // interacting with green pedestal
            {
                if (greenConditionMet && !hasGivenGreenKey)
                {

                    bool hasAdded = addToInventory("green key");
                    if (hasAdded == false)
                    {
                        textBox("Your inventory is still full.");
                        return false;
                    }
                    else
                    {
                        textBox("The pedestal remembers you getting the answer right, and appreciates the space now in your inventory\r\n\r\n[Green Key Added]");
                    }
                    hasGivenGreenKey = true;
                }
                else if (!hasGivenGreenKey)
                {
                    textBox("Inscribed on the pedestal is a statement:\r\n\"There are 2 torches in this room\"\r\n[True]      [False]", false, true);
                    return true;
                }
                else
                {
                    textBox("There's an empty green pedestal.");
                    return true;
                }

            }// green ped action

            if (txtLookingAt.Text == "green pedestal" && txtInput.Enabled)
            {
                if (contents.ToLower().Trim() == "true" && !hasGivenGreenKey)
                {

                    try
                    {
                        bool answer = Convert.ToBoolean(contents);
                    }
                    catch
                    {
                        MessageBox.Show("An error occured", "Error");
                    }
                    greenConditionMet = true;
                    bool hasAdded = addToInventory("green key");

                    if (hasAdded == false)
                    {
                        textBox("The answer was correct, but your inventory is full!\r\n\r\n[No item added.]");
                        return false;
                    }

                    textBox("The pedestal shakes as a green key pops out of it. The text on the pedestal seemingly dissappears." +
                        "\r\nYou pick up the green key." +
                        "\r\n\r\n[Green Key Added]");
                    hasGivenGreenKey = true;
                }
                else
                {
                    textBox("The pedestal makes an angry sound.\r\n\r\n[Incorrect response]\r\n[-25 HP]");
                    RemoveHealth(25);
                }
            } // green ped riddle detection

            if (txtLookingAt.Text == "yellow pedestal" && contents == "action") // interacting with green pedestal
            {
                if (yellowConditionMet && !hasGivenYellowKey)
                {

                    bool hasAdded = addToInventory("yellow key");
                    if (hasAdded == false)
                    {
                        textBox("Your inventory is still full.");
                        return false;
                    }
                    else
                    {
                        textBox("The pedestal remembers you getting the answer right, and appreciates the space now in your inventory\r\n\r\n[Yellow Key Added]");
                    }
                    hasGivenYellowKey = true;
                }
                else if (!hasGivenYellowKey)
                {
                    textBox("Inscribed on the pedestal is a question:\r\n\r\nWhat can you see in water that never gets wet?", false, true);
                    return true;
                }
                else
                {
                    textBox("There's an empty yellow pedestal.");
                    return true;
                }

            }// yellow ped action

            if (txtLookingAt.Text == "yellow pedestal" && txtInput.Enabled)
            {
                if (contents.ToLower().Trim() == "your reflection" || contents.ToLower() == "reflection" && !hasGivenYellowKey)
                {
                    yellowConditionMet = true;
                    bool hasAdded = addToInventory("yellow key");

                    if (hasAdded == false)
                    {
                        textBox("The answer was correct, but your inventory is full!\r\n\r\n[No item added.]");
                        return false;
                    }

                    textBox("The pedestal shakes as a yellow key pops out of it. The text on the pedestal seemingly dissappears." +
                        "\r\nYou pick up the yellow key." +
                        "\r\n\r\n[Yellow Key Added]");
                    hasGivenYellowKey = true;
                }
                else
                {
                    textBox("The pedestal makes an angry sound.\r\n\r\n[Incorrect response]\r\n[-25 HP]");
                    RemoveHealth(25);
                }
            } // yellow ped riddle detection

            if (txtLookingAt.Text == "red pedestal" && contents == "action") // interacting with green pedestal
            {
                if (redConditionMet && !hasGivenRedKey)
                {

                    bool hasAdded = addToInventory("red key");
                    if (hasAdded == false)
                    {
                        textBox("Your inventory is still full.");
                        return false;
                    }
                    else
                    {
                        textBox("The pedestal remembers you getting the answer right, and appreciates the space now in your inventory\r\n\r\n[Red Key Added]");
                    }
                    hasGivenRedKey = true;
                }
                else if (!hasGivenRedKey)
                {
                    textBox("Inscribed on the pedestal is a question:\r\n\r\nWhat building has the most stories?", false, true);
                    return true;
                }
                else
                {
                    textBox("There's an empty red pedestal.");
                    return true;
                }

            }// red ped action

            if (txtLookingAt.Text == "red pedestal" && txtInput.Enabled)
            {
                if (contents.ToLower().Trim() == "the library" || contents.ToLower().Trim() == "library" && !hasGivenRedKey)
                {
                    redConditionMet = true;
                    bool hasAdded = addToInventory("red key");

                    if (hasAdded == false)
                    {
                        textBox("The answer was correct, but your inventory is full!\r\n\r\n[No item added.]");
                        return false;
                    }

                    textBox("The pedestal shakes as a red key pops out of it. The text on the pedestal seemingly dissappears." +
                        "\r\nYou pick up the red key." +
                        "\r\n\r\n[Red Key Added]");
                    hasGivenRedKey = true;
                }
                else
                {
                    textBox("The pedestal makes an angry sound.\r\n\r\n[Incorrect response]\r\n[-25 HP]");
                    RemoveHealth(25);
                }
            } // red ped riddle detection

            if (txtLookingAt.Text == "purple pedestal" && contents == "action") // interacting with green pedestal
            {
                if (purpleConditionMet && !hasGivenPurpleKey)
                {

                    bool hasAdded = addToInventory("purple key");
                    if (hasAdded == false)
                    {
                        textBox("Your inventory is still full.");
                        return false;
                    }
                    else
                    {
                        textBox("The pedestal remembers you getting the answer right, and appreciates the space now in your inventory\r\n\r\n[Purple Key Added]");
                    }
                    hasGivenRedKey = true;
                }
                else if (!hasGivenPurpleKey)
                {
                    textBox("Inscribed on the pedestal is a question:\r\n\r\nWhere does yesterday come after today?", false, true);
                    return true;
                }
                else
                {
                    textBox("There's an empty purple pedestal.");
                    return true;
                }

            }// purple ped action

            if (txtLookingAt.Text == "purple pedestal" && txtInput.Enabled)
            {
                if (contents.ToLower().Trim() == "a dictionary" || contents.ToLower().Trim() == "dictionary" && !hasGivenPurpleKey)
                {
                    purpleConditionMet = true;
                    bool hasAdded = addToInventory("purple key");

                    if (hasAdded == false)
                    {
                        textBox("The answer was correct, but your inventory is full!\r\n\r\n[No item added.]");
                        return false;
                    }

                    textBox("The pedestal shakes as a purple key pops out of it. The text on the pedestal seemingly dissappears." +
                        "\r\nYou pick up the purple key." +
                        "\r\n\r\n[Purple Key Added]");
                    hasGivenPurpleKey = true;
                }
                else
                {
                    textBox("The pedestal makes an angry sound.\r\n\r\n[Incorrect response]\r\n[-25 HP]");
                    RemoveHealth(25);
                }
            } // purple ped riddle detection



            if (txtLookingAt.Text == "green door" && contents == "green key")
            {
                isGreenDoorUnlocked = true;
                return false;
            } // if trying to use the green key on the green door

            if (txtLookingAt.Text == "yellow door" && contents == "yellow key")
            {
                isYellowDoorUnlocked = true;
                return false;
            }// if trying to use the yellow key on the yellow door

            if (txtLookingAt.Text == "red door" && contents == "red key")
            {
                isRedDoorUnlocked = true;
                return false;
            }// if trying to use the red key on the red door

            if (txtLookingAt.Text == "purple door" && contents == "purple key")
            {
                isPurpleDoorUnlocked = true;
                return false;
            }// if trying to use the purple key on the purple door

            if (txtLookingAt.Text == "green chest" && contents == "green key")
            {

                bool didAdd = true;
                if (!isGreenChestOpen)
                {
                    didAdd = addToInventory("green apple");
                }
                if (didAdd == false)
                {
                    textBox("Inventory Full!");
                    return false;
                }
                else if (isGreenChestOpen == false)
                {
                    textBox("You open the green chest, and you get a green apple! How is this still fresh?\r\n" +
                        "The key gets stuck in the chest.\r\n\r\n" +
                        "[Green Key Removed.]\r\n[Green apple added.]");
                    isGreenChestOpen = true;
                    return true;
                }
            }// if trying to use the green key on the green chest

            if (txtLookingAt.Text == "yellow chest" && contents == "yellow key")
            {
                bool didAdd = true;
                if (!isYellowChestOpen)
                {
                    didAdd = addToInventory("lime");
                }
                if (didAdd == false)
                {
                    textBox("Inventory Full!");
                    return false;
                }
                else if (isYellowChestOpen == false)
                {
                    textBox("You open the yellow chest, and you get a lime! The chest is not refrigerated.\r\n" +
                        "The key gets stuck in the chest.\r\n\r\n" +
                        "[Yellow Key Removed.]\r\n[Lime added.]");
                    isYellowChestOpen = true;
                    return true;
                }
            }// if trying to use the red key on the red chest

            if (txtLookingAt.Text == "red chest" && contents == "red key")
            {
                bool didAdd = true;
                if (!isRedChestOpen)
                {
                    didAdd = addToInventory("unknown gameboy game");
                }
                if (didAdd == false)
                {
                    textBox("Inventory Full!");
                    return false;
                }
                else if (isRedChestOpen == false)
                {
                    textBox("You open the red chest, and you get a gameboy game with no label!\r\n" +
                        "The key gets stuck in the chest.\r\n\r\n" +
                        "[Red Key Removed.]\r\n[unknown gameboy game added.]");
                    isRedChestOpen = true;
                    return true;
                }
            }// if trying to use the yellow key on the yellow chest

            if (txtLookingAt.Text == "purple chest" && contents == "purple key")
            {
                bool didAdd = true;
                if (!isPurpleChestOpen)
                {
                    didAdd = addToInventory("$3USD gold coin");
                }
                if (didAdd == false)
                {
                    textBox("Inventory Full!");
                    return false;
                }
                else if (isPurpleChestOpen == false)
                {
                    textBox("You open the purple chest, and you found a gold coin worth approx $3 USD!\r\nThis is it!\r\n\r\n" +
                        "YOU WIN!");
                    isPurpleChestOpen = true;
                    btnUp.Enabled = false;
                    btnDown.Enabled = false;
                    btnLeft.Enabled = false;
                    btnRight.Enabled = false;
                    btnAction.Enabled = false;
                    btnBack.Enabled = false;
                    return true;
                }
            }// if trying to use the purple key on the purple chest

            return false; // returns true if item is used, false when item is not used
        }


        // Event handlers
        private void btnRestart_Click(object sender, EventArgs e) // resets all variables to default state -- also changes button text the first time for better UX
        {
            btnRestart.Text = "Restart";
            playerHP = spawnHP;
            level = spawnLevel;
            playerDirection = Properties.Resources.player_down_t;
            buttonAPressed = false;
            inv1Pressed = false;
            inv2Pressed = false;
            inv3Pressed = false;
            inv4Pressed = false;
            inv5Pressed = false;
            inv6Pressed = false;
            isGreenChestOpen = false;
            isYellowChestOpen = false;
            isRedChestOpen = false;
            isPurpleChestOpen = false;
            isGreenDoorUnlocked = false;
            isYellowDoorUnlocked = false;
            isRedDoorUnlocked = false;
            isPurpleDoorUnlocked = false;
            isTextBoxShowing = false;
            hasGivenGreenKey = false;
            hasGivenYellowKey = false;
            hasGivenRedKey = false;
            hasGivenPurpleKey = false;
            greenConditionMet = false;
            yellowConditionMet = false;
            redConditionMet = false;
            purpleConditionMet = false;

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
            boxHP.Text = "HP - " + playerHP.ToString();
            pgbPlayerHP.Value = playerHP;

            inventory[0] = "null";
            inventory[1] = "null";
            inventory[2] = "null";
            inventory[3] = "null";
            inventory[4] = "null";
            inventory[5] = "null";

            loadMap();

            if (isFacing(playerX, playerY - 1) == "null") txtLookingAt.Text = "nothing";
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

        private void btnUp_Click(object sender, EventArgs e)
        {
            allButtonsFalse();
            playerDirection = Properties.Resources.player_up_t; // Changes player direction

            if (isFacing(playerX, playerY + 1) == "null")
            {
                playerY++;
            }

            if (isFacing(playerX, playerY + 1) == "null") txtLookingAt.Text = "nothing"; else txtLookingAt.Text = isFacing(playerX, playerY + 1);

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

            if (isFacing(playerX, playerY - 1) == "null") txtLookingAt.Text = "nothing"; else txtLookingAt.Text = isFacing(playerX, playerY - 1);

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

            if (isFacing(playerX - 1, playerY) == "null") txtLookingAt.Text = "nothing"; else txtLookingAt.Text = isFacing(playerX - 1, playerY);

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

            if (isFacing(playerX + 1, playerY) == "null") txtLookingAt.Text = "nothing"; else txtLookingAt.Text = isFacing(playerX + 1, playerY);

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


            if (playerHP <= 1)
            {
                btnInventory1.Enabled = false;
                btnInventory2.Enabled = false;
                btnInventory3.Enabled = false;
                btnInventory4.Enabled = false;
                btnInventory5.Enabled = false;
                btnInventory6.Enabled = false;
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

            if (e.KeyCode == Keys.D1)
            {
                btnInventory1.PerformClick();
            }

            if (e.KeyCode == Keys.D2)
            {
                btnInventory2.PerformClick();
            }

            if (e.KeyCode == Keys.D3)
            {
                btnInventory3.PerformClick();
            }

            if (e.KeyCode == Keys.D4)
            {
                btnInventory4.PerformClick();
            }

            if (e.KeyCode == Keys.D5)
            {
                btnInventory5.PerformClick();
            }

            if (e.KeyCode == Keys.D6)
            {
                btnInventory6.PerformClick();
            }
        }

        private void btnDebug_Click(object sender, EventArgs e)
        {

        }

        private void pbx40_Click(object sender, EventArgs e)
        {

        }
    }
}