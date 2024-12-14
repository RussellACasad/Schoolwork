using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavigationAtt2
{
    public class Opponent
    {


        public Attack Attack1 { get; set; } = new Attack();
        public Attack Attack2 { get; set; } = new Attack();
        public Attack Attack3 { get; set; } = new Attack();
        public Attack Attack4 { get; set; } = new Attack();
        public int HP { get; set; }
        public int XPAward { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Level { get; set; }
        public System.Drawing.Image Image { get; set; } = Properties.Resources.border;




        public void OpponentCreate(int playerLevel, int playerMaxHP)
        {
            AttackList attackList = new AttackList();
            attackList.CreateList();
            Random random = new Random();

            Attack1 = attackList.EnemyAttacks[random.Next(0, attackList.EnemyAttacks.Count)];
            Attack2 = attackList.EnemyAttacks[random.Next(0, attackList.EnemyAttacks.Count)];
            Attack3 = attackList.EnemyAttacks[random.Next(0, attackList.EnemyAttacks.Count)];
            Attack4 = attackList.EnemyAttacks[random.Next(0, attackList.EnemyAttacks.Count)];

            HP = playerMaxHP + random.Next(-30, 6);
            XPAward = playerLevel * random.Next(5, 26);
            Level = playerLevel + random.Next(-2, 3);

            var type = random.Next(0, 4);
            

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
