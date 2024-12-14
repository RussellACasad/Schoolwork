using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavigationAtt2
{
    internal class Item
    {
        public string Name { get; set; } = String.Empty;
        public int HealthChange { get; set; } = 0;
        public string EatText { get; set; } = String.Empty;
        public string GetText { get; set; } = String.Empty;
        public string EatOrDrink {  get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;
    }
}
