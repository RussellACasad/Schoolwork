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

            HeroAttacks.Add(swordSwipe); // 0


            Attack tailWhip = new()
            {
                Name = "Tail Whip",
                Description = "Whip your opponent with your luxurious tail of fluffiness.",
                Damage = 20,
                SucceedPercent = 90,
                AttacksPerFight = 15
            };

            HeroAttacks.Add(tailWhip); // 1


            Attack punch = new()
            {
                Name = "Punch",
                Description = "You punch your opponent!",
                Damage = 15,
                SucceedPercent = 90,
                AttacksPerFight = 35
            };

            HeroAttacks.Add(punch); // 2
            EnemyAttacks.Add(punch);


            Attack sneeze = new()
            {
                Name = "Sneeze",
                Description = "You sneeze on the opponent! They lose health out of disgust...",
                Damage = 10,
                SucceedPercent = 100,
                AttacksPerFight = 2
            };

            HeroAttacks.Add(sneeze); // 3

            Attack bite = new()
            {
                Name = "Bite",
                Description = "You bite your opponent",
                Damage = 15,
                SucceedPercent = 75,
                AttacksPerFight = 25
            };

            HeroAttacks.Add(bite); // 4
            EnemyAttacks.Add(bite);

            Attack kick = new()
            {
                Name = "Kick",
                Description = "You kick your opponent",
                Damage = 20,
                SucceedPercent = 80,
                AttacksPerFight = 35
            };

            HeroAttacks.Add(kick); // 5
            EnemyAttacks.Add(kick);


            Attack lunge = new()
            {
                Name = "Lunge",
                Description = "You lunge and scratch your opponent in the face",
                Damage = 20,
                SucceedPercent = 90,
                AttacksPerFight = 20
            };

            HeroAttacks.Add(lunge); // 6
            EnemyAttacks.Add(lunge);

            Attack bap = new()
            {
                Name = "Bap",
                Description = "You bap the opponent in the face",
                Damage = 20,
                SucceedPercent = 90,
                AttacksPerFight = 20
            };

            HeroAttacks.Add(bap); // 7

            Attack surpAttack = new()
            {
                Name = "Surprise attack",
                Description = "You sneak around and attack your opponent from the back",
                Damage = 25,
                SucceedPercent = 80,
                AttacksPerFight = 10
            };

            HeroAttacks.Add(surpAttack); // 8

            Attack screech = new()
            {
                Name = "Screech",
                Description = "You make a deafening screech at your opponent.",
                Damage = 15,
                SucceedPercent = 95,
                AttacksPerFight = 10
            };

            HeroAttacks.Add(screech); // 9

            Attack dustPaw = new()
            {
                Name = "Dust paw",
                Description = "You pick up some dirt off the ground and throw it at your opponent. You accidentally throw a rock at them with it. ",
                Damage = 20,
                SucceedPercent = 95,
                AttacksPerFight = 10
            };

            HeroAttacks.Add(dustPaw); // 10

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
