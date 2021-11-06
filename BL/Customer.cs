using IBL.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public Location CustomerLocation { get; set; }
        public IEnumerable<ParcelAtCustomer> From { get; set; }
        public IEnumerable<ParcelAtCustomer> To { get; set; }
        public override string ToString()
        {
            string result = "";
            result += $"Id is {Id}\n";
            result += $"Name is {Name}\n";
            result += $"Phone is {Phone}\n";
            result += $"Location is { CustomerLocation }\n"; 
            return result;
        }
    }

}
