using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavigationAtt2
{
    internal class AttackList
    {
        public List<Attack> HeroAttacks { get; } = new List<Attack>();
        public List<Attack> EnemyAttacks { get; } = new List<Attack>();

        public void CreateList()
        {
            Attack swordSwipe = new()
            {
                Name = "Sword Swipe",
                Description = "You swipe your sword at your opponent!",
                Damage = 15,
                SucceedPercent = 99,
                AttacksPerFight = 25
            };

            HeroAttacks.Add(swordSwipe);


            Attack tailWhip = new()
            {
                Name = "Tail Whip",
                Description = "Whip your opponent with your luxurious tail of fluffiness.",
                Damage = 20,
                SucceedPercent = 90,
                AttacksPerFight = 15
            };

            HeroAttacks.Add(tailWhip);


            Attack punch = new()
            {
                Name = "Punch",
                Description = "You punch your opponent!",
                Damage = 15,
                SucceedPercent = 90,
                AttacksPerFight = 35
            };

            HeroAttacks.Add(punch);
            EnemyAttacks.Add(punch);


            Attack sneeze = new()
            {
                Name = "Sneeze",
                Description = "You sneeze on the opponent! They lose health out of disgust...",
                Damage = 10,
                SucceedPercent = 100,
                AttacksPerFight = 2
            };

            HeroAttacks.Add(sneeze);

            Attack bite = new()
            {
                Name = "Bite",
                Description = "You bite your opponent",
                Damage = 15,
                SucceedPercent = 75,
                AttacksPerFight = 25
            };

            HeroAttacks.Add(bite);
            EnemyAttacks.Add(bite);


            Attack tailWhip2 = new()
            {
                Name = "Tail Whip",
                Description = "You pull a whip out of your tail fluff and whip the opponent",
                Damage = 20,
                SucceedPercent = 80,
                AttacksPerFight = 15
            };

            HeroAttacks.Add(tailWhip2);
            EnemyAttacks.Add(tailWhip2);

            Attack kick = new()
            {
                Name = "Kick",
                Description = "You kick your opponent",
                Damage = 20,
                SucceedPercent = 80,
                AttacksPerFight = 35
            };

            HeroAttacks.Add(kick);
            EnemyAttacks.Add(kick);


            Attack lunge = new()
            {
                Name = "Lunge",
                Description = "You lunge and scratch your opponent in the face",
                Damage = 20,
                SucceedPercent = 90,
                AttacksPerFight = 20
            };

            HeroAttacks.Add(lunge);
            EnemyAttacks.Add(lunge);

            Attack howl = new()
            {
                Name = "Howl",
                Description = "Null",
                Damage = 15,
                SucceedPercent = 85,
                AttacksPerFight = 10
            };

            EnemyAttacks.Add(howl);


            Attack packAttack = new()
            {
                Name = "Pack attack",
                Description = "",
                Damage = 18,
                SucceedPercent = 99,
                AttacksPerFight = 2
            };

            EnemyAttacks.Add(packAttack);


            Attack rawr = new()
            {
                Name = "Roar",
                Description = "",
                Damage = 15,
                SucceedPercent = 95,
                AttacksPerFight = 10
            };

            EnemyAttacks.Add(rawr);
        }

    }
}
