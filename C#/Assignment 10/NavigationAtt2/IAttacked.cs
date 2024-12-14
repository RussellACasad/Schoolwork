using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavigationAtt2
{
    public interface IAttacked
    {
        bool Attacked(Attack attack, Character attacked);
    }
}
