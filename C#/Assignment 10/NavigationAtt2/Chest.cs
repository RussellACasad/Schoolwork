using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavigationAtt2
{
    public class Chest
    {
        public string Contents {  get; set; } = String.Empty;
        public string UnlockItem { get; set; } = String.Empty;
        public bool IsOpen { get; set; } = false;
    }
}
