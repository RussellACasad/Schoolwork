namespace NavigationAtt2
{
    public partial class PlayerSelect : Form
    {
        AttackList attackList = new();
        public string dir = @"C:\C#\Casad\";
        public string defaultName = "Dungeon_";
        public string extension = ".dgn";
        public string file = "";
        public string[] SaveFiles;

        public PlayerSelect()
        {
            InitializeComponent();
            attackList.CreateList();

            foreach (Attack attack in attackList.HeroAttacks)
            {
                cbxAttack1.Items.Add(attack.Name);
                cbxAttack2.Items.Add(attack.Name);
                cbxAttack3.Items.Add(attack.Name);
                cbxAttack4.Items.Add(attack.Name);
            }

            cbxAttack1.SelectedIndex = 0;
            cbxAttack2.SelectedIndex = 1;
            cbxAttack3.SelectedIndex = 2;
            cbxAttack4.SelectedIndex = 3;

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            DefaultFileName();

            SaveFiles = Directory.GetFiles(dir, "*" + extension);

            ReloadSaveBox();
        }

        private void DefaultFileName()
        {
            for (var i = 1; true; i++)
            {
                if (!File.Exists(dir + defaultName + i + extension))
                {
                    file = defaultName + i + extension;
                    txtSaveName.PlaceholderText = defaultName + i;
                    break;
                }
            }
        }

        public Attack AttackFinder(string name)
        {
            for (var i = 0; i < attackList.HeroAttacks.Count; i++)
            {
                if (attackList.HeroAttacks[i].Name == name)
                {
                    return attackList.HeroAttacks[i];
                }
            }
            return new Attack();
        }

        public void GameLaunch(int difficulty, bool load, string fileName)
        {


            if (load)
            {
                var hero = SaveLoad.Load(fileName);

                if (hero.Name == "null" || hero.CharMap.Length != 4900 || hero.TileMap.Length != 4900)
                {
                    File.Delete(fileName);
                    lbxSavesList.Items.Clear();
                    SaveFiles = Directory.GetFiles(dir, "*" + extension);
                    DefaultFileName();
                    ReloadSaveBox();
                    MessageBox.Show("File corrupt or not found.", "Error", MessageBoxButtons.OK);
                    return;
                }

                Form1 game = new(hero, true);
                this.Hide();
                game.Show();
            }
            else
            {

                if (File.Exists(dir + file) || txtSaveName.Text.Contains(' ') || txtSaveName.Text.Contains('<') || txtSaveName.Text.Contains('>')
                     || txtSaveName.Text.Contains(':') || txtSaveName.Text.Contains('"') || txtSaveName.Text.Contains('/') || txtSaveName.Text.Contains('\\') || txtSaveName.Text.Contains('|')
                      || txtSaveName.Text.Contains('?') || txtSaveName.Text.Contains('*'))
                {
                    MessageBox.Show($"Invalid File name: \"{txtSaveName.Text}\"", "Error!", MessageBoxButtons.OK);
                    txtSaveName.Clear();
                    return;
                }

                if (txtSaveName.Text != string.Empty)
                {
                    file = txtSaveName.Text + extension;
                }

                Hero hero = new()
                {
                    FileName = file,
                    Floor = 1,
                    XP = 0,
                    MaxXP = 100,
                    HP = 100,
                    Level = 1,
                    Difficulty = 0,
                    Attack1 = AttackFinder(cbxAttack1.Text),
                    Attack2 = AttackFinder(cbxAttack2.Text),
                    Attack3 = AttackFinder(cbxAttack3.Text),
                    Attack4 = AttackFinder(cbxAttack4.Text)
                };

                hero.Name = "Fox Boye";
                if (rdoPlayer2.Checked)
                {
                    hero.Name = "Salem";
                }

                hero.Floor = 1;
                hero.Level = 1;
                hero.XP = 0;
                hero.MaxXP = 100;
                hero.HP = 100;
                hero.MaxHP = 100;

                switch (difficulty)
                {
                    case 0:
                        hero.Difficulty = 0;

                        break;

                    case 1:
                        hero.Difficulty = 1;
                        break;

                    case 2:
                        hero.Difficulty = 2;
                        break;
                    default:
                        MessageBox.Show("An unknown error has occured. Please try again.", "Error Occured", MessageBoxButtons.OK);
                        Close();
                        break;
                }

                Form1 game = new(hero, false);
                this.Hide();
                game.Show();
            }

        }

        private void ButtonPlay_Click(object sender, EventArgs e)
        {
            if (rdoEasy.Checked) { GameLaunch(0, false, ""); }
            else if (rdoMed.Checked) { GameLaunch(1, false, ""); }
            else { GameLaunch(2, false, ""); }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Credits credit = new Credits();
            credit.Show();
        }

        private void ButtonLoadSlot1_Click(object sender, EventArgs e)
        {
            if (lbxSavesList.SelectedIndex == -1)
            {
                MessageBox.Show($"Please select a file!", "Error!", MessageBoxButtons.OK);
            }
            else
            {
                var fileName = dir + lbxSavesList.Items[lbxSavesList.SelectedIndex].ToString() ?? "null";

                if (fileName != "null")
                {
                    GameLaunch(0, true, fileName);
                }

            }


        }

        private void PlayerSelect_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void cbxAttack1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxAttack1.Text == cbxAttack2.Text || cbxAttack1.Text == cbxAttack3.Text || cbxAttack1.Text == cbxAttack4.Text)
            {
                ButtonPlay.Enabled = false;
                boxAttack1.Text = "Attack 1 - Can't have 2 same attacks";
            }
            else
            {
                ButtonPlay.Enabled = true;
                boxAttack1.Text = "Attack 1";
            }
        }

        private void cbxAttack2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxAttack2.Text == cbxAttack1.Text || cbxAttack2.Text == cbxAttack3.Text || cbxAttack2.Text == cbxAttack4.Text)
            {
                ButtonPlay.Enabled = false;
                boxAttack2.Text = "Attack 2 - Can't have 2 same attacks";
            }
            else
            {
                ButtonPlay.Enabled = true;
                boxAttack2.Text = "Attack 2";
            }
        }

        private void cbxAttack3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxAttack3.Text == cbxAttack1.Text || cbxAttack3.Text == cbxAttack2.Text || cbxAttack3.Text == cbxAttack4.Text)
            {
                ButtonPlay.Enabled = false;
                boxAttack3.Text = "Attack 3 - Can't have 2 same attacks";
            }
            else
            {
                ButtonPlay.Enabled = true;
                boxAttack3.Text = "Attack 3";
            }
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxAttack4.Text == cbxAttack1.Text || cbxAttack4.Text == cbxAttack2.Text || cbxAttack4.Text == cbxAttack3.Text)
            {
                ButtonPlay.Enabled = false;
                boxAttack4.Text = "Attack 4 - Can't have 2 same attacks";
            }
            else
            {
                ButtonPlay.Enabled = true;
                boxAttack4.Text = "Attack 4";
            }
        }

        private void rdoPlayer1_CheckedChanged(object sender, EventArgs e)
        {
            lblCharacterName.Text = "Fox Boye";
            cbxAttack1.SelectedIndex = 0;
            cbxAttack2.SelectedIndex = 1;
            cbxAttack3.SelectedIndex = 2;
            cbxAttack4.SelectedIndex = 3;
        }

        private void rdoPlayer2_CheckedChanged(object sender, EventArgs e)
        {
            lblCharacterName.Text = "Salem";
            cbxAttack1.SelectedIndex = 5;
            cbxAttack2.SelectedIndex = 7;
            cbxAttack3.SelectedIndex = 8;
            cbxAttack4.SelectedIndex = 9;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            rdoPlayer1.Checked = true;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            rdoPlayer2.Checked = true;
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void lbxSavesList_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (lbxSavesList.SelectedIndex != -1)
            {
                var hero = SaveLoad.Load(SaveFiles[lbxSavesList.SelectedIndex]);
                var diff = "Normal";

                switch (hero.Difficulty)
                {
                    case 0: diff = "Easy"; break;
                    case 2: diff = "Hard"; break;
                }

                txtSaveInfo.Text =
                    $"Player: {hero.Name}\r\n" +
                    $"Difficulty: {diff}\r\n" +
                    $"Level: {hero.Level} \r\n" +
                    $"Floor: {hero.Floor} \r\n" +
                    $"File Name: {hero.FileName}";

                if (hero.Name == "Salem")
                {
                    pbxLoadImage.Image = Properties.Resources.player_2_down;
                }
                else
                {
                    pbxLoadImage.Image = Properties.Resources.player_down_t;
                }

            }
            else
            {
                txtSaveInfo.Text = "Select a file!";
                pbxLoadImage.Image = Properties.Resources.fox_gray;
            }

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lbxSavesList.SelectedIndex == -1)
            {
                MessageBox.Show($"Please select a file!", "Error!", MessageBoxButtons.OK);
            }
            else
            {
                var fileName = dir + lbxSavesList.Items[lbxSavesList.SelectedIndex].ToString() ?? "null";

                if (fileName != "null")
                {
                    var box = MessageBox.Show($"Are you sure you want to delete {lbxSavesList.Items[lbxSavesList.SelectedIndex]}?", "Confirmation", MessageBoxButtons.OKCancel);
                    if (box == DialogResult.OK)
                    {
                        File.Delete(fileName);
                        lbxSavesList.Items.Clear();
                        SaveFiles = Directory.GetFiles(dir, "*" + extension);

                        DefaultFileName();
                        ReloadSaveBox();

                    }
                }
            }
        }

        private void ReloadSaveBox()
        {
            foreach (var file in SaveFiles)
            {
                lbxSavesList.Items.Add(file.Replace(dir, ""));
            }

            if (lbxSavesList.Items.Count == 0)
            {
                txtSaveInfo.Text = "Start a new game to make a save!";
                pbxLoadImage.Image = Properties.Resources.fox_gray;
                btnLoad.Enabled = false;
                btnDelete.Enabled = false;
            }
            else
            {
                txtSaveInfo.Text = "Select a file!";
                pbxLoadImage.Image = Properties.Resources.fox_gray;
                btnLoad.Enabled = true;
                btnDelete.Enabled = true;
            }
        }
    }
}
