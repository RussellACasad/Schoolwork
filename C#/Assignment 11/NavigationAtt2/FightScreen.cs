namespace NavigationAtt2
{
    public partial class FightScreen : Form
    {

        public Opponent Opponent { get; set; } = new(); // makes a new public opponent
        public Hero Player { get; set; } = new(); // makes a public hero

        private Attack selectedAttack = new(); // makes a public attack, for when the player picks one

        public FightScreen(Hero player, System.Drawing.Image fightImage)
        {
            InitializeComponent();
            Opponent.OpponentCreate(player.Level, player.MaxHP, player.Difficulty);
            Player = player;

            Random random = new Random();

            txtOutput.Text = $"{Opponent.Name} attacks!";
            pbxEnemy.Image = Opponent.Image;
            pbxHero.Image = fightImage; // sets the textbox with the attack message, plus sets pictures

            rdoAttack1.Text = Player.Attack1.Name;
            rdoAttack2.Text = Player.Attack2.Name;
            rdoAttack3.Text = Player.Attack3.Name;
            rdoAttack4.Text = Player.Attack4.Name; // sets the player attacks to the radiobuttons 

            pgbHeroHP.Maximum = Player.MaxHP;
            pgbEnemyHP.Maximum = Opponent.HP; // adjusts the HP bars so a crash never occurs 
            SetBars(); // sets the HP bars accordingly 

            txtAttackInfo.Text = prompt(Player.Attack1);
            selectedAttack = Player.Attack1; // sets the starting attack to attack 1 to prevent errors

        }

        private void SetBars() // a function to set all the HP bars correctly 
        {
            pgbEnemyHP.Value = Opponent.HP;
            pgbHeroHP.Value = Player.HP;
            lblPlayerHealth.Text = $"HP: {Player.HP} / {Player.MaxHP}";
        }

        private string prompt(Attack attack) // sets the attack prompt when an attack is selected
        {
            return $"{attack.Description}\r\n\r\n" +
                $"Damage: {attack.Damage} | Succeed percent {attack.SucceedPercent}";
        }

        private void Attack() // the function called when the attack button is pressed 
        {
            Random random = new();
            string playerText;
            string opponentText;
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
                playerText = Player.Attack(selectedAttack, Opponent, Player);
                opponentText = Opponent.Attack(enemyAttack, Player, Opponent);
            }
            else
            {
                opponentText = Opponent.Attack(enemyAttack, Player, Opponent);
                playerText = Player.Attack(selectedAttack, Opponent, Player);
            }

            if (opponentText == "OpponentDied")
            {
                Player.HP = 0;
                this.Close();
            }
            else if (playerText == "OpponentDied")
            {
                Opponent.HP = 0;
                Player.XP += Opponent.XPAward;
                this.Close();
            }
            else
            {
                txtOutput.Clear();
                txtOutput.Text = $"{playerText}\r\n\r\n{opponentText}";
            }

            SetBars();
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
