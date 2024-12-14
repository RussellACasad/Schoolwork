using System.Security.Cryptography.X509Certificates;

namespace NavigationAtt2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        // initializing variables to detect when things are done
        public string playerDirection = "up";
        public string playerDirectionIcon = "x";
        public int playerX = 0;
        public int playerY = 0;
        public int upCount = 0;
        public int downCount = 0;
        public int leftCount = 0;
        public int rightCount = 0;
        public bool buttonAPressed = false;
        public bool button1Pressed = false;
        public bool button2Pressed = false;
        public bool button3Pressed = false;
        public bool isFacingDoor = false;
        public bool isDoorLocked = false;
        public bool isRedDoorLocked = true;
        public bool isYellowDoorLocked = true;
        public bool isGreenDoorLocked = true;
        public bool isFinalDoorLocked = true;
        public bool hasRedKey = false;
        public bool hasYellowKey = false;
        public bool hasGreenKey = false;
        public bool hasRedAward = false;
        public bool hasYellowAward = false;
        public bool hasGreenAward = false;
        public bool isFinalConditionMet = false;
        public bool hasGivenGreenAward = false;
        public bool hasGivenYellowAward = false;
        public bool hasGivenRedAward = false;

        /*
         * Map text: 
         *    room = c
         * no room = g
         *      up = 5
         *    down = 6
         *    left = 3
         *   right = 4
        */

        //programming for each room
        private void rooms(int X, int Y) 
        {
            txtCoordX.Text = X.ToString();
            txtCoordY.Text = Y.ToString(); // all rooms are programmed on the same basic framework, room 4-1 is relatively simple
            if (X == 4 && Y == 1) // player coords
            {
                txtOutput.Text = "You enter a room. To the north is an open door, otherwise the room is empty.";
                switch (playerDirection)
                {
                    case "up": // north wall
                        txtOutput.Text += "\r\n\r\nDoor";
                        isFacingDoor = true; // says that there is a door connecting to another room here
                        isDoorLocked = false;
                        break;
                    case "down": // south wall
                        txtOutput.Text += "\r\n\r\nWall"; // says there is a wall
                        isFacingDoor = false;
                        break;
                    case "left": // West wall
                        txtOutput.Text += "\r\n\r\nWall";
                        isFacingDoor = false;
                        break;
                    case "right": // South wall
                        txtOutput.Text += "\r\n\r\nWall";
                        isFacingDoor = false;
                        break;
                }
                txtMap.Text = $"g    c    g\r\n\r\ng    {playerDirectionIcon}    g\r\n\r\ng    g    g"; 
                //the text for the minimap, always has 4 spaces in-between each character, g = no room, c = room, goes top, then middle, then bottom.
                //      Middle always has the character in it, because that is the current room.
            } // Starting room

            else if (X == 4 && Y == 2)
            {
                txtOutput.Text = "You walk into a room. On the West wall there's a green door with a green keyhole, otherwise the room is empty.";
                switch (playerDirection)
                {
                    case "up":
                        txtOutput.Text += "\r\n\r\nWall";
                        isFacingDoor = false;
                        break;
                    case "down":
                        txtOutput.Text += "\r\n\r\nDoor - Where you came from";
                        isFacingDoor = true;
                        isDoorLocked = false;
                        break;
                    case "left":

                        isFacingDoor = true;
                        if (hasGreenKey == true && button1Pressed == true)
                        {
                            txtOutput.Text += "\r\n\r\nYou unlock and open the green door, the key fades to dust...";
                            button1Pressed = false;
                            isGreenDoorLocked = false;
                            hasGreenKey = false;
                            btnInventory1.Text = "Slot 1";
                            btnInventory1.Enabled = false;
                        }
                        else if (isGreenDoorLocked == true)
                        {
                            isDoorLocked = true;
                            txtOutput.Text += "\r\n\r\nLocked green door.";
                        }
                        else
                        {
                            isDoorLocked = false;
                            txtOutput.Text += "\r\n\r\nUnlocked green door";
                        }

                        break;
                    case "right":
                        txtOutput.Text += "\r\n\r\nWall";
                        isFacingDoor = false;
                        break;
                }
                txtMap.Text = $"g    g    g\r\n\r\nc    {playerDirectionIcon}    g\r\n\r\ng    c    g";
            }// challenge room - green

            else if (X == 3 && Y == 2)
            {
                txtOutput.Text = $"You walk into a room with a door on the West wall and a treasure chest on the South wall.";
                switch (playerDirection)
                {
                    case "up":
                        txtOutput.Text += "\r\n\r\nWall";
                        isFacingDoor = false;
                        break;
                    case "down":
                        txtOutput.Text += "\r\n\r\nTreasure Chest";
                        isFacingDoor = false;
                        if (hasGreenAward == false && buttonAPressed == true)
                        {
                            txtOutput.Text += "\r\n\r\nOpened Treasure chest! Inside was a green hat. You put it on ";
                            if (hasYellowAward == true && hasRedAward == true)
                            {
                                txtOutput.Text += "top of the red and yellow hats. He's sad you forgot him.";
                            }
                            else if (hasYellowAward == true)
                            {
                                txtOutput.Text += "top of the yellow hat. He's happy you came back for him.";
                            }
                            else if (hasRedAward == true)
                            {
                                txtOutput.Text += "top of the red hat. He's wondering what the yellow hat did to hurt you...";
                            }
                            else
                            {
                                txtOutput.Text += "top of your head.";
                            }
                            hasGreenAward = true;
                            btnInventory1.Text = "Green Hat";
                            btnInventory1.Enabled = true;
                            buttonAPressed = false;
                        }
                        else if (hasGreenAward == false)
                        {
                            txtOutput.Text += " (Unopened)";
                        }
                        else
                        {
                            txtOutput.Text += " (Opened)";
                        }
                        break;
                    case "left":
                        txtOutput.Text += "\r\n\r\nDoor";
                        isFacingDoor = true;
                        isDoorLocked = false;
                        break;
                    case "right":
                        txtOutput.Text += "\r\n\r\nDoor - Where you came from";
                        isFacingDoor = true;
                        isDoorLocked = false;
                        break;
                }
                txtMap.Text = $"g    g    g\r\n\r\nc    {playerDirectionIcon}    c\r\n\r\ng    g    c";
            }// award room - green

            else if (X == 2 && Y == 2)
            {
                txtOutput.Text = $"You walk into a room with a door on the West wall. The room is otherwise empty.";
                switch (playerDirection)
                {
                    case "up":
                        txtOutput.Text += "\r\n\r\nWall";
                        isFacingDoor = false;
                        break;
                    case "down":
                        txtOutput.Text += "\r\n\r\nWall";
                        isFacingDoor = false;
                        break;
                    case "left":
                        txtOutput.Text += "\r\n\r\nDoor";
                        isFacingDoor = true;
                        isDoorLocked = false;
                        break;
                    case "right":
                        txtOutput.Text += "\r\n\r\nDoor - Where you came from";
                        isFacingDoor = true;
                        isDoorLocked = false;
                        break;
                }
                txtMap.Text = $"c    g    g\r\n\r\nc    {playerDirectionIcon}    c\r\n\r\ng    g    g";
            }// standard room

            else if (X == 1 && Y == 2)
            {
                txtOutput.Text = $"You walk into a room with a yellow door with a yellow keyhole on the North wall, otherwise the room is empty.";
                switch (playerDirection)
                {
                    case "up":
                        isFacingDoor = true;
                        if (hasYellowKey == true && button2Pressed == true)
                        {
                            txtOutput.Text += "\r\n\r\nYou unlock and open the yellow door, the key fades to dust...";
                            button2Pressed = false;
                            isYellowDoorLocked = false;
                            hasYellowKey = false;
                            btnInventory2.Text = "Slot 2";
                            btnInventory2.Enabled = false;
                        }
                        else if (isYellowDoorLocked == true)
                        {
                            isDoorLocked = true;
                            txtOutput.Text += "\r\n\r\nLocked yellow door";
                        }
                        else
                        {
                            isDoorLocked = false;
                            txtOutput.Text += "\r\n\r\nUnlocked yellow door";
                        }
                        break;
                    case "down":
                        txtOutput.Text += "\r\n\r\nWall";
                        isFacingDoor = false;
                        break;
                    case "left":
                        txtOutput.Text += "\r\n\r\nWall";
                        isFacingDoor = false;
                        break;
                    case "right":
                        txtOutput.Text += "\r\n\r\nDoor - Where you came from";
                        isFacingDoor = true;
                        isDoorLocked = false;
                        break;
                }
                txtMap.Text = $"g    c    g\r\n\r\ng    {playerDirectionIcon}    c\r\n\r\ng    g    g";
            }// challenge room - yellow

            else if (X == 1 && Y == 3)
            {
                txtOutput.Text = $"You walk into a room with a yellow treasure chest on the West wall, and an open door on the North wall. Otherwise the room is empty.";
                switch (playerDirection)
                {
                    case "up":
                        txtOutput.Text += "\r\n\r\nDoor";
                        isFacingDoor = true;
                        isDoorLocked = false;
                        break;
                    case "down":
                        txtOutput.Text += "\r\n\r\nDoor - Where you came from";
                        isFacingDoor = true;
                        isDoorLocked = false;
                        break;
                    case "left":
                        txtOutput.Text += "\r\n\r\nTreasure Chest";
                        isFacingDoor = false;
                        if (hasYellowAward == false && buttonAPressed == true)
                        {
                            txtOutput.Text += "\r\n\r\nOpened Treasure chest! Inside was a yellow hat. You put it ";
                            if (hasGreenAward == true && hasRedAward == true)
                            {
                                txtOutput.Text += "on top of the green and red hat, he's wondering how you forgot him, and thinks it's something he did. He takes it personally...";
                            }
                            else if (hasGreenAward == true)
                            {
                                txtOutput.Text += "on top of the green hat.";
                            }
                            else if (hasRedAward == true)
                            {
                                txtOutput.Text += "on top of the red hat. He can see you're backtracking, and is happy he wasn't chosen last.";
                            }
                            else
                            {
                                txtOutput.Text += "on top of your head. He wonders why you skipped the green hat... did he smell bad?";
                            }
                            hasYellowAward = true;
                            btnInventory2.Text = "Yellow Hat";
                            btnInventory2.Enabled = true;
                            buttonAPressed = false;
                        }
                        else if (hasYellowAward == false)
                        {
                            txtOutput.Text += " (Unopened)";
                        }
                        else
                        {
                            txtOutput.Text += " (Opened)";
                        }
                        break; ;
                    case "right":
                        txtOutput.Text += "\r\n\r\nWall";
                        isFacingDoor = false;
                        break;
                }
                txtMap.Text = $"g    c    c\r\n\r\ng    {playerDirectionIcon}    g\r\n\r\ng    c    c";
            }// award room - yellow

            else if (X == 1 && Y == 4)
            {
                txtOutput.Text = $"You walk into a room with a red door with a red keyhole on the East wall, the room is otherwise empty.";
                switch (playerDirection)
                {
                    case "up":
                        txtOutput.Text += "\r\n\r\nWall";
                        isFacingDoor = false;
                        break;
                    case "down":
                        txtOutput.Text += "\r\n\r\nDoor - Where you came from";
                        isFacingDoor = true;
                        isDoorLocked = false;
                        break;
                    case "left":
                        txtOutput.Text += "\r\n\r\nWall";
                        isFacingDoor = false;
                        break;
                    case "right":
                        isFacingDoor = true;
                        if (hasRedKey == true && button3Pressed == true)
                        {
                            txtOutput.Text += "\r\n\r\nYou unlock and open the Red door, the key fades to dust...";
                            button3Pressed = false;
                            isRedDoorLocked = false;
                            hasRedKey = false;
                            btnInventory3.Text = "Slot 3";
                            btnInventory3.Enabled = false;
                        }
                        else if (isRedDoorLocked == true)
                        {
                            isDoorLocked = true;
                            txtOutput.Text += "\r\n\r\nLocked red door";
                        }
                        else
                        {
                            isDoorLocked = false;
                            txtOutput.Text += "\r\n\r\nUnlocked red door";
                        }
                        break; ;
                }
                txtMap.Text = $"g    g    g\r\n\r\ng    {playerDirectionIcon}    c\r\n\r\ng    c    g";
            }// challenge room - red

            else if (X == 2 && Y == 4)
            {
                txtOutput.Text = $"You walk into a room with a red treasure chest on the North wall, and an open door on the East wall. The room is otherwise empty.";
                switch (playerDirection)
                {
                    case "up":
                        txtOutput.Text += "\r\n\r\nTreasure Chest";
                        isFacingDoor = false;
                        if (hasRedAward == false && buttonAPressed == true)
                        {
                            txtOutput.Text += "\r\n\r\nOpened Treasure chest! Inside was a green hat. You put it on ";
                            if (hasYellowAward == true && hasGreenAward == true)
                            {
                                txtOutput.Text += "top of the green and yellow hats.";
                            }
                            else if (hasYellowAward)
                            {
                                txtOutput.Text += "top of the yellow hat. She agrees the green hat is stinky, but might prove needed.";
                            }
                            else if (hasGreenAward)
                            {
                                txtOutput.Text += "top of the green hat. She wonders why you skipped the yellow hat, did he threaten to stab you?";
                            }
                            else
                            {
                                txtOutput.Text += "top of your head. She is flattered you picked her first, but is annoyed she has to travel more with you now.";
                            }
                            hasRedAward = true;
                            btnInventory3.Text = "Red Hat";
                            btnInventory3.Enabled = true;
                            buttonAPressed = false;
                        }
                        else if (hasRedAward == false)
                        {
                            txtOutput.Text += " (Unopened)";
                        }
                        else
                        {
                            txtOutput.Text += " (Opened)";
                        }
                        break;
                    case "down":
                        txtOutput.Text += "\r\n\r\nWall";
                        isFacingDoor = false;
                        break;
                    case "left":
                        txtOutput.Text += "\r\n\r\nDoor - Where you came from";
                        isFacingDoor = true;
                        isDoorLocked = false;
                        break;
                    case "right":
                        txtOutput.Text += "\r\n\r\nDoor";
                        isFacingDoor = true;
                        isDoorLocked = false;
                        break;
                }
                txtMap.Text = $"g    g    g\r\n\r\nc    {playerDirectionIcon}    c\r\n\r\nc    g    g";
            }// award room - red

            else if (X == 3 && Y == 4)
            {
                txtOutput.Text = $"You walk into a room with a big multicolour door on the East wall, a yellow pedestal on the North wall, a green pedestal on the West wall, and a red pedestal on the South wall.";
                switch (playerDirection)
                {
                    case "up":
                        txtOutput.Text += "\r\n\r\nYellow pedestal";
                        isFacingDoor = false;

                        if (hasYellowAward == true && button2Pressed == true)
                        {
                            txtOutput.Text += "\r\n\r\nSet yellow hat on the yellow pedestal";
                            button2Pressed = false;
                            hasYellowAward = false;
                            hasGivenYellowAward = true;
                            btnInventory2.Text = "Slot 2";
                            btnInventory2.Enabled = false;

                            if (hasGivenGreenAward == true && hasGivenYellowAward == true && hasGivenRedAward == true)
                            {
                                txtOutput.Text += "\r\n\r\nThe colourful door makes an unlaching noise and what appears to be a button pops out of the middle...";
                                isFinalConditionMet = true;
                            }
                        }
                        break; ;
                    case "down":
                        txtOutput.Text += "\r\n\r\nRed pedestal";
                        isFacingDoor = false;

                        if (hasRedAward == true && button3Pressed == true)
                        {
                            txtOutput.Text += "\r\n\r\nSet red hat on the red pedestal";
                            button3Pressed = false;
                            hasRedAward = false;
                            hasGivenRedAward = true;
                            btnInventory3.Text = "Slot 3";
                            btnInventory3.Enabled = false;

                            if (hasGivenGreenAward == true && hasGivenYellowAward == true && hasGivenRedAward == true)
                            {
                                txtOutput.Text += "\r\n\r\nThe colourful door makes an unlaching noise and what appears to be a button pops out of the middle...";
                                isFinalConditionMet = true;
                            }
                        }
                        break;
                    case "left":
                        txtOutput.Text += "\r\n\r\nGreen pedestal & Door - Where you came from ";
                        isFacingDoor = true;
                        isDoorLocked = false;

                        if (hasGreenAward == true && button1Pressed == true)
                        {
                            txtOutput.Text += "\r\n\r\nSet green hat on the green pedestal";
                            button1Pressed = false;
                            hasGreenAward = false;
                            hasGivenGreenAward = true;
                            btnInventory1.Text = "Slot 1";
                            btnInventory1.Enabled = false;

                            if (hasGivenGreenAward == true && hasGivenYellowAward == true && hasGivenRedAward == true)
                            {
                                txtOutput.Text += "\r\n\r\nThe colourful door makes an unlaching noise and what appears to be a button pops out of the middle...";
                                isFinalConditionMet = true;
                            }
                        }
                        break;
                    case "right":
                        isFacingDoor = true;
                        if (isFinalConditionMet == true && buttonAPressed == true)
                        {
                            txtOutput.Text += "\r\n\r\nPushed the button to unlock and open the colourful door";
                            buttonAPressed = false;
                            isFinalDoorLocked = false;
                        }
                        else if (isFinalDoorLocked == true)
                        {
                            isDoorLocked = true;
                            txtOutput.Text += "\r\n\r\nLocked colourful door";
                        }
                        else
                        {
                            isDoorLocked = false;
                            txtOutput.Text += "\r\n\r\nUnlocked colourful door";
                        }
                        break;
                }
                txtMap.Text = $"g    g    g\r\n\r\nc    {playerDirectionIcon}    c\r\n\r\ng    g    g";
            }// challenge room - final

            else if (X == 4 && Y == 4)
            {
                txtOutput.Text = "";
                string finalMessage = "!niw uoY";
                txtOutput.TextAlign = HorizontalAlignment.Center;
                for (int i = finalMessage.Length - 1; i >= 0; i--)
                {
                    txtOutput.Text += finalMessage[i];
                    System.Threading.Thread.Sleep(100);
                    this.Update();
                }
                txtOutput.Text += "\r\n\r\nPress Restart to play again!";



                txtMap.Text = $"g    g    g\r\n\r\nc    {playerDirectionIcon}    g\r\n\r\ng    g    g";
            }// end room
        }
        private void btnRestart_Click(object sender, EventArgs e) // resets all variables to default state -- also changes button text the first time for better UX
        {
            btnRestart.Text = "Restart";
            playerX = 4;
            playerY = 1;
            playerDirectionIcon = "5";
            btnAction.Enabled = true;
            btnUp.Enabled = true;
            btnDown.Enabled = true;
            btnLeft.Enabled = true;
            btnRight.Enabled = true;
            isRedDoorLocked = true;
            isYellowDoorLocked = true;
            isGreenDoorLocked = true;
            isFinalDoorLocked = true;
            hasRedKey = false;
            hasYellowKey = false;
            hasGreenKey = false;
            hasRedAward = false;
            hasYellowAward = false;
            hasGreenAward = false;
            isFinalConditionMet = false;
            // I didn't have time to program in challenges to get the keys, so you bought them at a pawn shop for 99 cents. I'll prolly 
            //     change that for week 3 tho :)
            hasGreenKey = true;
            btnInventory1.Text = "Green Key";
            btnInventory1.Enabled = true;
            hasYellowKey = true;
            btnInventory2.Text = "Yellow Key";
            btnInventory2.Enabled = true;
            hasRedKey = true;
            btnInventory3.Text = "Red Key";
            btnInventory3.Enabled = true;

            rooms(playerX, playerY);
        }

        private void btnAction_Click(object sender, EventArgs e) 
        {
            //changes the action button to pushed, all others to false. All others are changed to false on all button presses, inclusing movement
            //      to prevent misunderstandings for both the program and the end user.
            buttonAPressed = true;
            button1Pressed = false;
            button2Pressed = false;
            button3Pressed = false;
            rooms(playerX, playerY);
        }

        private void btnUp_Click(object sender, EventArgs e)
        { // all of these are programmed the same, they set the player direction, and move the player if they are facing a door, they always update the map to at least change direction.
            buttonAPressed = false;
            button1Pressed = false;
            button2Pressed = false;
            button3Pressed = false;
            playerDirection = "up";
            upCount++;
            downCount = 0;
            leftCount = 0;
            rightCount = 0;
            playerDirectionIcon = "5";

            if (upCount == 2 && isFacingDoor == true && isDoorLocked == false)
            {
                playerY++;
                upCount = 0;

            }
            else if (upCount == 2 && isFacingDoor == true && isDoorLocked == true)
            {
                upCount = 0;
                txtOutput.Text += "\r\n\r\n Door is locked!";
            }
            else if (upCount == 2 && isFacingDoor == false)
            {
                upCount = 0;
                txtOutput.Text += "\r\n\r\n bruh that's a wall";
            }
            rooms(playerX, playerY);
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            buttonAPressed = false;
            button1Pressed = false;
            button2Pressed = false;
            button3Pressed = false;
            playerDirection = "down";
            upCount = 0;
            downCount++;
            leftCount = 0;
            rightCount = 0;
            playerDirectionIcon = "6";

            if (downCount == 2 && isFacingDoor == true && isDoorLocked == false)
            {
                playerY--;
                downCount = 0;

            }
            else if (downCount == 2 && isFacingDoor == true && isDoorLocked == true)
            {
                downCount = 0;
                txtOutput.Text += "\r\n\r\n Door is locked!";
            }
            else if (downCount == 2 && isFacingDoor == false)
            {
                downCount = 0;
                txtOutput.Text += "\r\n\r\n bruh that's a wall";
            }
            rooms(playerX, playerY);
        }

        private void btnLeft_Click(object sender, EventArgs e)
        {
            buttonAPressed = false;
            button1Pressed = false;
            button2Pressed = false;
            button3Pressed = false;
            playerDirection = "left";
            upCount = 0;
            downCount = 0;
            leftCount++;
            rightCount = 0;
            playerDirectionIcon = "3";

            if (leftCount == 2 && isFacingDoor == true && isDoorLocked == false)
            {
                playerX--;
                leftCount = 0;

            }
            else if (leftCount == 2 && isFacingDoor == true && isDoorLocked == true)
            {
                leftCount = 0;
                txtOutput.Text += "\r\n\r\n Door is locked!";
            }
            else if (leftCount == 2 && isFacingDoor == false)
            {
                leftCount = 0;
                txtOutput.Text += "\r\n\r\n bruh that's a wall";
            }
            rooms(playerX, playerY);
        }

        private void btnRight_Click(object sender, EventArgs e)
        {
            buttonAPressed = false;
            button1Pressed = false;
            button2Pressed = false;
            button3Pressed = false;
            playerDirection = "right";
            upCount = 0;
            downCount = 0;
            leftCount = 0;
            rightCount++;
            playerDirectionIcon = "4";

            if (rightCount == 2 && isFacingDoor == true && isDoorLocked == false)
            {
                playerX++;
                rightCount = 0;

            }
            else if (rightCount == 2 && isFacingDoor == true && isDoorLocked == true)
            {
                rightCount = 0;
                txtOutput.Text += "\r\n\r\n Door is locked!";
            }
            else if (rightCount == 2 && isFacingDoor == false)
            {
                rightCount = 0;
                txtOutput.Text += "\r\n\r\n bruh that's a wall";
            }
            rooms(playerX, playerY);
        }

        private void btnInventory1_Click(object sender, EventArgs e) // says inventory 1 button is pushed, all inventory buttons follow the same logic
        {
            buttonAPressed = false;
            button1Pressed = true;
            button2Pressed = false;
            button3Pressed = false;
            rooms(playerX, playerY);
        }

        private void btnInventory2_Click(object sender, EventArgs e)
        {
            buttonAPressed = false;
            button1Pressed = false;
            button2Pressed = true;
            button3Pressed = false;
            rooms(playerX, playerY);
        }

        private void btnInventory3_Click(object sender, EventArgs e)
        {
            buttonAPressed = false;
            button1Pressed = false;
            button2Pressed = false;
            button3Pressed = true;
            rooms(playerX, playerY);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}