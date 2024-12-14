using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavigationAtt2
{
    public class Pedestal
    {
        public string Item { get; set; } = String.Empty;
        public string Color { get; set; } = String.Empty;
        public bool ConditionMet { get; set; } = false;
        public bool HasGivenItem { get; set; } = false;

    }
}
