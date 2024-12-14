namespace NavigationAtt2
{
    public class Character : IFightable
    {
        public string Name { get; set; } = string.Empty;
        public Attack Attack1 { get; set; } = new Attack();
        public Attack Attack2 { get; set; } = new Attack();
        public Attack Attack3 { get; set; } = new Attack();
        public Attack Attack4 { get; set; } = new Attack();
        public int HP { get; set; }

        public string Attack(Attack attack, Character toAttack, Character attacker)
        {
            Random random = new Random();
            var percent = random.Next(1, 101);
            if (attack.SucceedPercent >= percent)
            {
                bool didOpponentDie = toAttack.Attacked(attack, toAttack);

                if (didOpponentDie)
                {
                    return "OpponentDied";
                }

                return $"{attacker.Name} used {attack.Name}, and did {attack.Damage} damage!";
            }
            else
            {
                return $"{attacker.Name} tried to use {attack.Name}! But it missed!";
            }
        }

        public bool Attacked(Attack attack, Character attacked)
        {

            attacked.HP = attacked.HP - attack.Damage;

            if (attacked.HP <= 0)
            {
                attacked.HP = 0;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
