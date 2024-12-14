using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace NavigationAtt2
{
    public class Opponent : Character
    {
        public int XPAward { get; set; }
        public int Level { get; set; }
        public System.Drawing.Image Image { get; set; } = Properties.Resources.border;

        public void OpponentCreate(int playerLevel, int playerMaxHP, int playerDifficulty)
        {
            AttackList attackList = new AttackList();
            attackList.CreateList();
            Random random = new Random();

            Attack1 = attackList.EnemyAttacks[random.Next(0, attackList.EnemyAttacks.Count)];
            Attack2 = attackList.EnemyAttacks[random.Next(0, attackList.EnemyAttacks.Count)];
            Attack3 = attackList.EnemyAttacks[random.Next(0, attackList.EnemyAttacks.Count)];
            Attack4 = attackList.EnemyAttacks[random.Next(0, attackList.EnemyAttacks.Count)];

            HP = playerMaxHP + random.Next(-30, 6);
            XPAward = 20 + (5 * playerLevel) + random.Next(-5, 6);
            Level = playerLevel + random.Next(-2, 3);

            var type = random.Next(0, 4);

            if (playerDifficulty == 0) // adjusts the fight based on the selected difficulty
            {
                HP -= 25;
                Attack1.SucceedPercent -= random.Next(10, 20);
                Attack2.SucceedPercent -= random.Next(10, 20);
                Attack3.SucceedPercent -= random.Next(10, 20);
                Attack4.SucceedPercent -= random.Next(10, 20);
                Attack1.Damage -= random.Next(8, 13);
                Attack2.Damage -= random.Next(8, 13);
                Attack3.Damage -= random.Next(8, 13);
                Attack4.Damage -= random.Next(8, 13);
            }
            else if (playerDifficulty == 1)
            {
                Attack1.Damage -= random.Next(9, 11);
                Attack2.Damage -= random.Next(9, 11);
                Attack3.Damage -= random.Next(9, 11);
                Attack4.Damage -= random.Next(9, 11);
            }
            else if (playerDifficulty == 2)
            {
                HP += 10;
                Attack1.SucceedPercent -= random.Next(-10, 5);
                Attack2.SucceedPercent -= random.Next(-10, 5);
                Attack3.SucceedPercent -= random.Next(-10, 5);
                Attack4.SucceedPercent -= random.Next(-10, 5);
            }

            if (Attack1.Damage < 0) Attack1.Damage = Math.Abs(Attack1.Damage);
            if (Attack2.Damage < 0) Attack2.Damage = Math.Abs(Attack2.Damage);
            if (Attack3.Damage < 0) Attack3.Damage = Math.Abs(Attack3.Damage);
            if (Attack4.Damage < 0) Attack4.Damage = Math.Abs(Attack4.Damage);
            if (Attack1.Damage == 0) Attack1.Damage = random.Next(3, 8);
            if (Attack2.Damage == 0) Attack2.Damage = random.Next(3, 8);
            if (Attack3.Damage == 0) Attack3.Damage = random.Next(3, 8);
            if (Attack4.Damage == 0) Attack4.Damage = random.Next(3, 8);

            switch (type)
            {
                case 0:
                    Image = Properties.Resources.Wolf;
                    Name = "Wolf";
                    break;
                case 1:
                    Image = Properties.Resources.Werewolf;
                    Name = "Werewolf";
                    break;
                case 2:
                    Image = Properties.Resources.FrostWolf;
                    Name = "Frost Wolf";
                    break;
                case 3:
                    Image = Properties.Resources.GreyWolf;
                    Name = "Grey Wolf";
                    break; ;
            }
        }
    }
}
