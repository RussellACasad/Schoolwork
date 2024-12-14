// Russell Casad
// CPT-230-W01
// 2023 FA
using NavigationAtt2.Properties;
using System.Diagnostics.Eventing.Reader;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;

namespace NavigationAtt2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            pbx42.Controls.Add(pbxPlayer);
            pbxPlayer.Location = new Point(0, 0);
            pbxPlayer.BackColor = Color.Transparent; // makes the player picturebox have a transparent background so the floor doesn't look weird


        }

        // initializing variables to detect when things are done
        public System.Drawing.Image playerDirection = Properties.Resources.player_up_t;
        public int randomNum = 0;
        public int playerX = 0;
        public int playerY = 0;
        public bool buttonAPressed = false;
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

        public string[] inventory = { "null", "null", "null", "null", "null", "null" };

        // map in an array -- use key -- goes from top to bottom ([0,0] = bottom left)
        public char[,] rooms = // !! alert !! Rooms reference (Y, X), NOT (X, Y) !!
            {
                {'h', 'a', 'a', 'a', 'a', 'a', 'i', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
                {'b', '1', '1', '1', '1', '1', 'd', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
                {'b', '1', '1', '1', '1', '1', 'd', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
                {'b', '1', '1', '1', '1', '1', 'd', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
                {'b', '1', '1', '1', '1', '1', 'd', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
                {'b', '1', '1', '1', '1', '1', 'd', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},

                {'b', 'c', '1', '1', '1', 'c', 'd', ' ', 'h', 'a', 'a', 'a', 'a', 'a', 'i'},
                {'h', 'e', '1', '1', '1', 'f', 'i', ' ', 'h', 'a', 'a', 'a', 'a', 'a', 'i'},

                {'b', '1', '1', '1', '1', '1', 'f', 'a', 'e', '1', '1', '1', '1', '1', 'd'},
                {'b', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', 'd'},
                {'b', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', '-', '1', '1', 'd'},
                {'b', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', 'd'},
                {'b', '1', '1', '1', '1', '1', 'c', 'c', 'c', '1', '1', '1', '1', '1', 'd'},

                {'b', 'c', 'c', '%', 'c', 'c', 'd', ' ', 'b', 'c', 'c', 'c', 'c', 'c', 'd'},
                {'h', 'a', 'e', '1', 'f', 'a', 'i', ' ', 'h', 'a', 'a', 'a', 'a', 'a', 'i'},

                {'b', '1', '1', '1', '1', '1', 'f', 'a', 'e', '1', '1', '1', '1', '1', 'd'},
                {'b', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', 'd'},
                {'b', '!', '1', '1', '1', '1', '1', '1', '1', '1', '1', '=', '1', '1', 'd'},
                {'b', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', 'd'},
                {'b', '1', '1', '1', '1', '1', 'c', 'c', 'c', '1', '1', '1', '1', '1', 'd'},

                {'b', 'c', 'c', '^', 'c', 'c', 'd', ' ', 'b', 'c', 'c', 'c', 'c', 'c', 'd'},
                {'h', 'a', 'e', '1', 'f', 'a', 'i', ' ', 'h', 'a', 'a', 'a', 'a', 'a', 'i'},

                {'b', '1', '1', '1', '1', '1', 'f', 'a', 'e', '1', '1', '1', '1', '1', 'd'},
                {'b', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', 'd'},
                {'b', '@', '1', '1', '1', '1', '1', '1', '1', '1', '1', '[', '1', '1', 'd'},
                {'b', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', 'd'},
                {'b', '1', '1', '1', '1', '1', 'c', 'c', 'c', '1', '1', '1', '1', '1', 'd'},

                {'b', 'c', 'c', '&', 'c', 'c', 'd', ' ', 'b', 'c', 'c', 'c', 'c', 'c', 'd'},
                {'h', 'a', 'e', '1', 'f', 'a', 'i', ' ', 'h', 'a', 'a', 'a', 'a', 'a', 'i'},

                {'b', '1', '1', '1', '1', '1', 'f', 'a', 'e', '1', '1', '1', '1', '1', 'd'},
                {'b', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', 'd'},
                {'b', '#', '1', '1', '1', '1', '1', '1', '1', '1', '1', ']', '1', '1', 'd'},
                {'b', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', 'd'},
                {'b', '1', '1', '1', '1', '1', 'c', 'c', 'c', '1', '1', '1', '1', '1', 'd'},

                {'b', 'c', 'c', '*', 'c', 'c', 'd', ' ', 'b', 'c', 'c', 'c', 'c', 'c', 'd'},
                {'h', 'a', 'e', '1', 'f', 'a', 'i', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},

                {'b', '1', '1', '1', '1', '1', 'd', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
                {'b', '1', '1', '1', '1', '1', 'd', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
                {'b', '1', '1', '$', '1', '1', 'd', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
                {'b', '1', '1', '1', '1', '1', 'd', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
                {'b', '1', '1', '1', '1', '1', 'd', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},

                {'b', 'c', 'c', 'c', 'c', 'c', 'd', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},

            };


        // map stuff
        private System.Drawing.Image mapRooms(int X, int Y)
        {
            Random random = new Random(1);  // random number stuff to be implemented later
            int tileNumber = random.Next(0, 2);

            if (Y > rooms.GetLength(0) - 1 || Y < 0) // if statement makes it so nonexistant variables get referenced
            {
                return Properties.Resources.no_wall;
            }
            else if (X > rooms.GetLength(1) - 1 || X < 0)
            {
                return Properties.Resources.no_wall;
            }
            else
            {

                if (rooms[Y, X] == '1') // attemts to randomize the floor tiles postponed-- floor tiles use 1 in map array
                {
                    if (tileNumber == 0) return Properties.Resources.floor;
                    else if (tileNumber == 1) return Properties.Resources.floor_1;
                    else return Properties.Resources.floor_2;
                }
                else if (rooms[Y, X] == 'a') return Properties.Resources.wall_top; // wall tiles use alphabet chars in map aray
                else if (rooms[Y, X] == 'b') return Properties.Resources.wall_right;
                else if (rooms[Y, X] == 'c') return Properties.Resources.wall_bot;
                else if (rooms[Y, X] == 'd') return Properties.Resources.wall_left;
                else if (rooms[Y, X] == 'e') return Properties.Resources.wall_corner_top_right;
                else if (rooms[Y, X] == 'f') return Properties.Resources.wall_corner_top_left;
                else if (rooms[Y, X] == 'g') return Properties.Resources.no_wall;
                else if (rooms[Y, X] == 'h') return Properties.Resources.dot_top_right;
                else if (rooms[Y, X] == 'i') return Properties.Resources.dot_top_left;

                else if (isGreenChestOpen && rooms[Y, X] == '!') return Properties.Resources.green_chest_open; // chests & doors use special chars
                else if (rooms[Y, X] == '!') return Properties.Resources.green_chest_closed;
                else if (isYellowChestOpen && rooms[Y, X] == '@') return Properties.Resources.yellow_chest_open;
                else if (rooms[Y, X] == '@') return Properties.Resources.yellow_chest_closed;
                else if (isRedChestOpen && rooms[Y, X] == '#') return Properties.Resources.red_chest_open;
                else if (rooms[Y, X] == '#') return Properties.Resources.red_chest_closed;
                else if (isPurpleChestOpen && rooms[Y, X] == '$') return Properties.Resources.purple_chest_open;
                else if (rooms[Y, X] == '$') return Properties.Resources.purple_chest_closed;

                else if (isGreenDoorUnlocked && rooms[Y, X] == '%') return Properties.Resources.green_door_open;
                else if (rooms[Y, X] == '%') return Properties.Resources.green_door_closed;
                else if (isYellowDoorUnlocked && rooms[Y, X] == '^') return Properties.Resources.yellow_door_open;
                else if (rooms[Y, X] == '^') return Properties.Resources.yellow_door_closed;
                else if (isRedDoorUnlocked && rooms[Y, X] == '&') return Properties.Resources.red_door_open;
                else if (rooms[Y, X] == '&') return Properties.Resources.red_door_closed;
                else if (isPurpleDoorUnlocked && rooms[Y, X] == '*') return Properties.Resources.purple_door_open;
                else if (rooms[Y, X] == '*') return Properties.Resources.purple_door_closed;

                else if (rooms[Y, X] == '-') return Properties.Resources.green_ped; // peds use the end of the alphabet
                else if (rooms[Y, X] == '=') return Properties.Resources.Yellow_ped;
                else if (rooms[Y, X] == '[') return Properties.Resources.red_ped;
                else if (rooms[Y, X] == ']') return Properties.Resources.purple_ped;

                else return Properties.Resources.no_wall;// if char not found, it returns this to prevent crashes
            }
        } // translates the map from characters to images,returning the image name for any given X and Y coord

        private void map(int X, int Y)
        {
            inventorySort();
            txtCoordX.Text = X.ToString(); // sets X and Y coord boxes
            txtCoordY.Text = Y.ToString();

            pbx0n1.Image = mapRooms(X - 4, Y - 3); // sets each photobox to the proper image for it using x&y coods
            pbx1n1.Image = mapRooms(X - 3, Y - 3);
            pbx2n1.Image = mapRooms(X - 2, Y - 3);
            pbx3n1.Image = mapRooms(X - 1, Y - 3);
            pbx4n1.Image = mapRooms(X + 0, Y - 3);
            pbx5n1.Image = mapRooms(X + 1, Y - 3);
            pbx6n1.Image = mapRooms(X + 2, Y - 3);
            pbx7n1.Image = mapRooms(X + 3, Y - 3);
            pbx8n1.Image = mapRooms(X + 4, Y - 3);

            pbx00.Image = mapRooms(X - 4, Y - 2);
            pbx10.Image = mapRooms(X - 3, Y - 2);
            pbx20.Image = mapRooms(X - 2, Y - 2);
            pbx30.Image = mapRooms(X - 1, Y - 2);
            pbx40.Image = mapRooms(X + 0, Y - 2);
            pbx50.Image = mapRooms(X + 1, Y - 2);
            pbx60.Image = mapRooms(X + 2, Y - 2);
            pbx70.Image = mapRooms(X + 3, Y - 2);
            pbx80.Image = mapRooms(X + 4, Y - 2);

            pbx01.Image = mapRooms(X - 4, Y - 1);
            pbx11.Image = mapRooms(X - 3, Y - 1);
            pbx21.Image = mapRooms(X - 2, Y - 1);
            pbx31.Image = mapRooms(X - 1, Y - 1);
            pbx41.Image = mapRooms(X + 0, Y - 1);
            pbx51.Image = mapRooms(X + 1, Y - 1);
            pbx61.Image = mapRooms(X + 2, Y - 1);
            pbx71.Image = mapRooms(X + 3, Y - 1);
            pbx81.Image = mapRooms(X + 4, Y - 1);

            pbx02.Image = mapRooms(X - 4, Y - 0);
            pbx12.Image = mapRooms(X - 3, Y - 0);
            pbx22.Image = mapRooms(X - 2, Y - 0);
            pbx32.Image = mapRooms(X - 1, Y - 0);
            pbx42.Image = mapRooms(X, Y);
            pbx52.Image = mapRooms(X + 1, Y - 0);
            pbx62.Image = mapRooms(X + 2, Y - 0);
            pbx72.Image = mapRooms(X + 3, Y - 0);
            pbx82.Image = mapRooms(X + 4, Y - 0);

            pbx03.Image = mapRooms(X - 4, Y + 1);
            pbx13.Image = mapRooms(X - 3, Y + 1);
            pbx23.Image = mapRooms(X - 2, Y + 1);
            pbx33.Image = mapRooms(X - 1, Y + 1);
            pbx43.Image = mapRooms(X + 0, Y + 1);
            pbx53.Image = mapRooms(X + 1, Y + 1);
            pbx63.Image = mapRooms(X + 2, Y + 1);
            pbx73.Image = mapRooms(X + 3, Y + 1);
            pbx83.Image = mapRooms(X + 4, Y + 1);

            pbx04.Image = mapRooms(X - 4, Y + 2);
            pbx14.Image = mapRooms(X - 3, Y + 2);
            pbx24.Image = mapRooms(X - 2, Y + 2);
            pbx34.Image = mapRooms(X - 1, Y + 2);
            pbx44.Image = mapRooms(X + 0, Y + 2);
            pbx54.Image = mapRooms(X + 1, Y + 2);
            pbx64.Image = mapRooms(X + 2, Y + 2);
            pbx74.Image = mapRooms(X + 3, Y + 2);
            pbx84.Image = mapRooms(X + 4, Y + 2);

            pbx05.Image = mapRooms(X - 4, Y + 3);
            pbx15.Image = mapRooms(X - 3, Y + 3);
            pbx25.Image = mapRooms(X - 2, Y + 3);
            pbx35.Image = mapRooms(X - 1, Y + 3);
            pbx45.Image = mapRooms(X + 0, Y + 3);
            pbx55.Image = mapRooms(X + 1, Y + 3);
            pbx65.Image = mapRooms(X + 2, Y + 3);
            pbx75.Image = mapRooms(X + 3, Y + 3);
            pbx85.Image = mapRooms(X + 4, Y + 3);

            pbxPlayer.Image = playerDirection;
        } // Updates the map and the X/Y coords whenever called

        private string isFacing(int X, int Y)
        {
            if (Y > rooms.GetLength(0) - 1 || Y < 0)
            {
                return "wall";
            }
            else if (X > rooms.GetLength(1) - 1 || X < 0)
            {
                return "wall";
            }
            else if (rooms[Y, X] == 'a' || rooms[Y, X] == 'b' || rooms[Y, X] == 'c' || rooms[Y, X] == 'd' || rooms[Y, X] == 'e' || rooms[Y, X] == 'f')
            {
                return "wall";
            }
            else if (rooms[Y, X] == '!')
            {
                return "green chest";
            }
            else if (rooms[Y, X] == '@')
            {
                return "yellow chest";
            }
            else if (rooms[Y, X] == '#')
            {
                return "red chest";
            }
            else if (rooms[Y, X] == '$')
            {
                return "purple chest";
            }
            else if (rooms[Y, X] == '%')
            {
                if (isGreenDoorUnlocked) return "null";
                else return "green door";
            }
            else if (rooms[Y, X] == '^')
            {
                if (isYellowDoorUnlocked) return "null";
                else return "yellow door";
            }
            else if (rooms[Y, X] == '&')
            {
                if (isRedDoorUnlocked) return "null";
                else return "red door";
            }
            else if (rooms[Y, X] == '*')
            {
                if (isPurpleDoorUnlocked) return "null";
                else return "purple door";
            }
            else if (rooms[Y, X] == '-')
            {
                return "green pedestal";
            }
            else if (rooms[Y, X] == '=')
            {
                return "yellow pedestal";
            }
            else if (rooms[Y, X] == '[')
            {
                return "red pedestal";
            }
            else if (rooms[Y, X] == ']')
            {
                return "purple pedestal";
            }
            else
            {
                return "null";
            }
        } // gives a text description of any X,Y coord, used wenever you want to know what the player is facing

        private void textBox(string text = "test", bool hide = false)
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
                for (int i = 0; i < text.Length; i++)// outputs the text 1 char at a time
                {
                    txtOutput.Text += text[i];
                    System.Threading.Thread.Sleep(15);
                    this.Update();
                }
                btnAction.Text = "\u21A9";//changes action button to back button
                btnAction.Enabled = true;

            }
            else if (hide == true) // hides textbox
            {
                btnUp.Enabled = true;
                btnDown.Enabled = true;
                btnLeft.Enabled = true;
                btnRight.Enabled = true;
                btnAction.Text = "A";
                txtOutput.Visible = false;
                map(playerX, playerY);
            }



        } // makes a textbox with any given text, hide is called by the close button only


        // button stuff
        private void allButtonsFalse()
        {
            buttonAPressed = false;
            inv1Pressed = false;
            inv2Pressed = false;
            inv3Pressed = false;
            inv4Pressed = false;
            inv5Pressed = false;
            inv6Pressed = false;
        } // sets all the buttons to false, used when pushing any button to ensure no accidental clicks are recorded when not actually clicked.

        private void buttonPushed(string contents = "null")
        {
            int invNumber = 0; // variable for referencing the inventory
            char buttonPressed = 'n'; // variable for which button is pressed 
            if (buttonAPressed)
            {
                buttonPressed = 'a';
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
                if (buttonPressed == 'a') return;
                inventory[invNumber] = "null";
                inventorySort();
            }

            map(playerX, playerY);

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
            if (contents == "action" && btnAction.Text != "A") // when back button hit on text box
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

            if (txtLookingAt.Text == "green pedestal" && contents == "action") // interacting with green pedestal
            {
                bool didAdd = true;
                if (!hasGivenGreenKey)
                {
                    didAdd = addToInventory("green key");
                }

                if (didAdd == false)
                {
                    textBox("Inventory Full!");
                    return false;
                }
                else if (hasGivenGreenKey == false)
                {
                    textBox("There's a green key sitting on the green pedestal. You pick it up. \r\n\r\n [Green Key added.]");
                    hasGivenGreenKey = true;
                    return true;
                }
                else
                {
                    textBox("There's an empty green pedestal.");
                    return true;
                }

            }// interacting with pedestals

            if (txtLookingAt.Text == "yellow pedestal" && contents == "action") // interacting with yellow ped
            {
                bool didAdd = true;
                if (!hasGivenYellowKey)
                {
                    didAdd = addToInventory("yellow key");
                }

                if (didAdd == false)
                {
                    textBox("Inventory Full!");
                    return false;
                }
                else if (hasGivenYellowKey == false)
                {
                    textBox("There's a yellow key sitting on the yellow pedestal You pick it up. \r\n\r\n [Yellow Key added.]");
                    hasGivenYellowKey = true;
                    return true;
                }
                else
                {
                    textBox("There's an empty yellow pedestal.");
                    return true;
                }
            }

            if (txtLookingAt.Text == "red pedestal" && contents == "action") // interacting with red ped 
            {
                bool didAdd = true;
                if (!hasGivenRedKey)
                {
                    didAdd = addToInventory("red key");
                }

                if (didAdd == false)
                {
                    textBox("Inventory Full!");
                    return false;
                }
                else if (hasGivenRedKey == false)
                {
                    textBox("There's a red key sitting on the red pedestal You pick it up. \r\n\r\n [Red Key added.]");
                    hasGivenRedKey = true;
                    return true;
                }
                else
                {
                    textBox("There's an empty red pedestal.");
                    return true;
                }
            }

            if (txtLookingAt.Text == "purple pedestal" && contents == "action") // interacting with purple ped
            {
                bool didAdd = true;
                if (!hasGivenPurpleKey)
                {
                    didAdd = addToInventory("purple key");
                }

                if (didAdd == false)
                {
                    textBox("Inventory Full!");
                    return false;
                }
                else if (hasGivenPurpleKey == false)
                {
                    textBox("There's a purple key sitting on the purple pedestal You pick it up. \r\n\r\n [Purple Key added.]");
                    hasGivenPurpleKey = true;
                    return true;
                }
                else
                {
                    textBox("There's an empty purple pedestal.");
                    return true;
                }
            }

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
                    return true;
                }
            }// if trying to use the purple key on the purple chest

            return false; // returns true if item is used, false when item is not used
        }


        // Event handlers
        private void btnRestart_Click(object sender, EventArgs e) // resets all variables to default state -- also changes button text the first time for better UX
        {
            btnRestart.Text = "Restart";
            playerX = 3;
            playerY = 3;
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

            btnUp.Enabled = true;
            btnDown.Enabled = true;
            btnLeft.Enabled = true;
            btnRight.Enabled = true;
            btnAction.Enabled = true;

            txtLookingAt.Text = "nothing";

            map(playerX, playerY);
            textBox("HOW TO PLAY:\r\n\r\n" +
                "TO MOVE: Use the on screen arrow buttons\r\n " +
                "TO INTERACT: Press the on screen 'A' button\r\n" +
                "TO USE AN ITEM: Click on it in your inventory\r\n" +
                "Press the back arrow to close dialogue boxes, like this one.");
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

            map(playerX, playerY);
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

            map(playerX, playerY);
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

            map(playerX, playerY);
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

            map(playerX, playerY);
        }

        private void btnAction_Click(object sender, EventArgs e)
        {
            //changes the action button to pushed, all others to false. All others are changed to false on all button presses, inclusing movement
            //      to prevent misunderstandings for both the program and the end user.
            allButtonsFalse();
            buttonAPressed = true;
            buttonPushed("action");
        }

        private void btnInventory1_Click(object sender, EventArgs e) // says inventory 1 button is pushed, all inventory buttons follow the same logic
        {
            allButtonsFalse();
            inv1Pressed = true;
            buttonPushed(inventory[0]);
        }

        private void btnInventory2_Click(object sender, EventArgs e)
        {
            allButtonsFalse();
            inv2Pressed = true;
            buttonPushed(inventory[1]);
        }

        private void btnInventory3_Click(object sender, EventArgs e)
        {
            allButtonsFalse();
            inv3Pressed = true;
            buttonPushed(inventory[2]);
        }

        private void btnInventory4_Click(object sender, EventArgs e)
        {
            allButtonsFalse();
            inv4Pressed = true;
            buttonPushed(inventory[3]);
        }

        private void btnInventory5_Click(object sender, EventArgs e)
        {
            allButtonsFalse();
            inv5Pressed = true;
            buttonPushed(inventory[4]);
        }

        private void btnInventory6_Click(object sender, EventArgs e)
        {
            allButtonsFalse();
            inv6Pressed = true;
            buttonPushed(inventory[5]);
        }

    }
}