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
        public PlayerSelect()
        {
            InitializeComponent();
        }



        public void GameLaunch(int difficulty, bool load, int fileNum)
        {

            Attack nullAttack = new()
            {
                Name = "null",
                Description = "null",
                Damage = 0,
                SucceedPercent = 0,
                AttacksPerFight = 0
            };


            Hero hero = new()
            {
                Name = "",
                Floor = 1,
                XP = 0,
                MaxXP = 100,
                HP = 100,
                Level = 1,
                Difficulty = 0,
                Attack1 = nullAttack,
                Attack2 = nullAttack,
                Attack3 = nullAttack,
                Attack4 = nullAttack
            };

            AttackList attackList = new();
            attackList.CreateList();

            switch (difficulty)
            {
                case 0:
                    hero.Name = "Fox";
                    hero.Difficulty = 0;
                    hero.Floor = 1;
                    hero.Level = 1;
                    hero.XP = 0;
                    hero.MaxXP = 0;
                    hero.HP = 100;
                    hero.MaxHP = 100;
                    hero.Attack1 = attackList.HeroAttacks[0];
                    hero.Attack2 = attackList.HeroAttacks[1];
                    hero.Attack3 = attackList.HeroAttacks[2];
                    hero.Attack4 = attackList.HeroAttacks[3];
                    hero.Up = Properties.Resources.player_up_t;
                    hero.Down = Properties.Resources.player_down_t;
                    hero.Left = Properties.Resources.player_left_t;
                    hero.Right = Properties.Resources.player_right_t;
                    hero.Animation = Properties.Resources.Yawnnn;
                    break;

                case 1:

                    hero.Name = "Fox";
                    hero.Difficulty = 1;
                    hero.Floor = 1;
                    hero.Level = 1;
                    hero.XP = 0;
                    hero.MaxXP = 0;
                    hero.HP = 100;
                    hero.MaxHP = 100;
                    hero.Attack1 = attackList.HeroAttacks[0];
                    hero.Attack2 = attackList.HeroAttacks[1];
                    hero.Attack3 = attackList.HeroAttacks[2];
                    hero.Attack4 = attackList.HeroAttacks[3];
                    hero.Up = Properties.Resources.player_up_t;
                    hero.Down = Properties.Resources.player_down_t;
                    hero.Left = Properties.Resources.player_left_t;
                    hero.Right = Properties.Resources.player_right_t;
                    hero.Animation = Properties.Resources.Yawnnn;
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
            else { GameLaunch(1, false, 0); }
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
    }
}
