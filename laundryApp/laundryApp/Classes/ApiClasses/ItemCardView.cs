using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace laundryApp.Classes
{
    public class CardViewItems
    {
        public Item item { get; set; }
        public Category category { get; set; }
        public User user { get; set; }
        public Agent agent { get; set; }
        public int row { get; set; }
        public int column { get; set; }

        // "sales" , "purchase" 
        public string cardType { get; set; }
    }
}
