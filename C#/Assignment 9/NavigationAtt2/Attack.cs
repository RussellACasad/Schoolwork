using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavigationAtt2
{
    public class Attack
    {
        public string Name { get; set; } = "null";
        public string Description { get; set; } = "null";
        public int Damage { get; set; } = 0;
        public int SucceedPercent { get; set; } = 0;
        public int AttacksPerFight { get; set; } = 0;
    }
}
