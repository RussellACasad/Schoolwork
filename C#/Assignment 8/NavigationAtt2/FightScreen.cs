using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NavigationAtt2
{
    public partial class FightScreen : Form
    {

        public Opponent Opponent { get; set; } = new();
        public Hero Player { get; set; } = new();

        private Attack selectedAttack = new Attack();

        public FightScreen(Hero player)
        {
            InitializeComponent();
            Opponent.OpponentCreate(player.Level, player.MaxHP);
            Player = player;

            txtOutput.Text = $"{Opponent.Name} attacks!";
            pbxEnemy.Image = Opponent.Image;
            pbxHero.Image = Properties.Resources.player_right_t;

            rdoAttack1.Text = Player.Attack1.Name;
            rdoAttack2.Text = Player.Attack2.Name;
            rdoAttack3.Text = Player.Attack3.Name;
            rdoAttack4.Text = Player.Attack4.Name;

            if (Player.Difficulty == 0)
            {
                Opponent.HP -= 25;
            }

            pgbHeroHP.Maximum = Player.MaxHP;
            pgbEnemyHP.Maximum = Opponent.HP;
            SetBars();



        }

        private void SetBars()
        {
            pgbEnemyHP.Value = Opponent.HP;
            pgbHeroHP.Value = Player.HP;
            lblPlayerHealth.Text = $"HP: {Player.HP} / {Player.MaxHP}";
        }

        private string prompt(Attack attack)
        {
            return $"{attack.Description}\r\n\r\n" +
                $"Damage: {attack.Damage} | Succeed percent {attack.SucceedPercent}";
        }

        private void Attack()
        {
            Random random = new();

            bool playerDead;
            bool opponentDead;
            var enemyAttackNum = random.Next(0, 4);
            var enemyAttack = new Attack();

            switch (enemyAttackNum)
            {
                case 0:
                    enemyAttack = Opponent.Attack1;
                    break;
                case 1:
                    enemyAttack = Opponent.Attack2;
                    break;
                case 2:
                    enemyAttack = Opponent.Attack3;
                    break;
                case 3:
                    enemyAttack = Opponent.Attack4;
                    break;
            }

            if (Player.Level > Opponent.Level)
            {
                txtOutput.Clear();
                opponentDead = PlayerAttack();
                txtOutput.Text += "\r\n\r\n";
                playerDead = EnemyAttack(enemyAttack);
            }
            else
            {
                txtOutput.Clear();
                playerDead = EnemyAttack(enemyAttack);
                txtOutput.Text += "\r\n\r\n";
                opponentDead = PlayerAttack();
            }

            if (playerDead)
            {
                Player.HP = 0;
                this.Close();
            }
            else if (opponentDead)
            {
                Opponent.HP = 0;
                Player.XP += Opponent.XPAward;
                this.Close();
            }

            SetBars();
        }

        private bool PlayerAttack()
        {
            Random random = new Random();
            var percent = random.Next(1, 101);

            if (selectedAttack.SucceedPercent >= percent)
            {
                Opponent.HP = Opponent.HP - selectedAttack.Damage;
                txtOutput.Text += $"You use {selectedAttack.Name}! {selectedAttack.Damage} damage done";
            }
            else
            {
                txtOutput.Text += $"You use {selectedAttack.Name}! But it misses!";
            }

            if (Opponent.HP <= 0)
            {
                Opponent.HP = 0;
                return true;
            }
            else
            {
                return false;
            }

        }

        private bool EnemyAttack(Attack enemyAttack)
        {
            Random random = new Random();

            int percent;

            enemyAttack.SucceedPercent -= random.Next(5, 16);


            if (Player.Difficulty == 0)
            {
                percent = random.Next(1, 126);
            }
            else
            {
                percent = random.Next(1, 101);
            }


            if (enemyAttack.SucceedPercent >= percent)
            {
                Player.HP -= enemyAttack.Damage - 10;
                txtOutput.Text += $"{Opponent.Name} uses {enemyAttack.Name}! {enemyAttack.Damage - 10} damage done";
            }
            else
            {
                txtOutput.Text += $"{Opponent.Name} uses use {enemyAttack.Name}! But it misses!";
            }

            if (Player.HP <= 0)
            {
                Player.HP = 0;
                return true;
            }
            else
            {
                return false;
            }
        }

        private void rdoAttack1_Click(object sender, EventArgs e)
        {
            txtAttackInfo.Text = prompt(Player.Attack1);
            selectedAttack = Player.Attack1;
        }

        private void rdoAttack2_Click(object sender, EventArgs e)
        {
            txtAttackInfo.Text = prompt(Player.Attack2);
            selectedAttack = Player.Attack2;
        }

        private void rdoAttack3_Click(object sender, EventArgs e)
        {
            txtAttackInfo.Text = prompt(Player.Attack3);
            selectedAttack = Player.Attack3;
        }

        private void rdoAttack4_Click(object sender, EventArgs e)
        {
            txtAttackInfo.Text = prompt(Player.Attack4);
            selectedAttack = Player.Attack4;
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            Attack();
        }

        private void FightScreen_Load(object sender, EventArgs e)
        {

        }

    }
}
