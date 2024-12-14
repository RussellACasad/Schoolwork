using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace NavigationAtt2
{
    public partial class PlayerSelect : Form
    {
        AttackList attackList = new();
        List<Attack> List1 = new();
        List<Attack> List2 = new();
        List<Attack> List3 = new();
        List<Attack> List4 = new();

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

        public void GameLaunch(int difficulty, bool load, int fileNum)
        {

            Hero hero = new()
            {
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

            var playerUp = Properties.Resources.player_up_t;
            var playerDown = Properties.Resources.player_down_t;
            var playerLeft = Properties.Resources.player_left_t;
            var playerRight = Properties.Resources.player_right_t;
            var playerSpec = Properties.Resources.Yawnnn;

            if (rdoPlayer2.Checked)
            {
                playerUp = Properties.Resources.player_2_up;
                playerDown = Properties.Resources.player_2_down;
                playerLeft = Properties.Resources.player_2_left;
                playerRight = Properties.Resources.player_2_right;
                playerSpec = Properties.Resources.player_2_down;
            }

            hero.Floor = 1;
            hero.Level = 1;
            hero.XP = 0;
            hero.MaxXP = 0;
            hero.HP = 100;
            hero.MaxHP = 100;
            hero.Up = playerUp;
            hero.Down = playerDown;
            hero.Left = playerLeft;
            hero.Right = playerRight;
            hero.Animation = playerSpec;

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

            Form1 game = new(hero, load, fileNum);
            this.Hide();
            game.Show();


        }

        private void ButtonPlay_Click(object sender, EventArgs e)
        {
            if (rdoEasy.Checked) { GameLaunch(0, false, 0); }
            else if (rdoMed.Checked) { GameLaunch(1, false, 0); }
            else { GameLaunch(2, false, 0); }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Credits credit = new Credits();
            credit.Show();
        }

        private void ButtonLoadSlot1_Click(object sender, EventArgs e)
        {
            GameLaunch(0, true, 1);
        }

        private void ButtonLoadSlot2_Click(object sender, EventArgs e)
        {
            GameLaunch(0, true, 1);
        }

        private void ButtonLoadSlot3_Click(object sender, EventArgs e)
        {
            GameLaunch(0, true, 1);
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
    }
}
