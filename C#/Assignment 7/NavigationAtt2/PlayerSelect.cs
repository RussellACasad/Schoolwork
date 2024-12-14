using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NavigationAtt2
{
    public partial class PlayerSelect : Form
    {
        public PlayerSelect()
        {
            InitializeComponent();
        }


        public void GameLaunch(int difficulty, bool load, int fileNum)
        {
            string PlayerChoice = "Null";

            switch (difficulty)
            {
                case 0:
                    PlayerChoice = "Deer";
                    break;
                case 1:
                    PlayerChoice = "Fox";
                    break;
                case 2:
                    PlayerChoice = "Tiger";
                    break;
                default:
                    MessageBox.Show("An unknown error has occured. Please try again.", "Error Occured", MessageBoxButtons.OK);
                    Close();
                    break;
            }

            Form1 game = new(PlayerChoice, load, fileNum);
            this.Hide();
            game.Show();


        }

        private void ButtonPlayEasy_Click(object sender, EventArgs e)
        {
            GameLaunch(0, false, 0);
        }

        private void ButtonPlayMed_Click(object sender, EventArgs e)
        {
            GameLaunch(1, false, 0);
        }

        private void ButtonPlayHard_Click(object sender, EventArgs e)
        {
            GameLaunch(2, false, 0);
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
            Application.Exit();
        }

        
    }
}
