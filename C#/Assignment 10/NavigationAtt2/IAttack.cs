using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavigationAtt2
{
    public interface IAttack
    {
        string Attack(Attack attack, Character toAttack, Character attacked);
    }
}
