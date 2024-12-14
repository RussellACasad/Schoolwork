using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavigationAtt2
{
    public class Door
    {
        public bool IsUnlocked { get; set; } = false;
        public string UnlockItem { get; set; } = "xxxxx";
    }

    public class FinalDoor : Door
    {
        public bool GivenItem1 = false;
        public bool GivenItem2 = false;
        public bool GivenItem3 = false;
        public bool GivenItem4 = false;

        public string Item1 { get; set; } = string.Empty;
        public string Item2 { get; set; } = string.Empty;
        public string Item3 { get; set; } = string.Empty;
        public string Item4 { get; set; } = string.Empty;
    }
}
