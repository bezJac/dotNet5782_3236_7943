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
            int i = 1,j = 1;
            string result = "";
            result += $"Id: {Id}\n";
            result += $"Name: {Name}\n";
            result += $"Phone: {Phone}\n";
            result += $"Location:\n{ CustomerLocation }";
            if (From.Any())
            {
                result += $"List of parcels from customer:\n";
                foreach (ParcelAtCustomer prc in From)
                {
                    result += $"{i++}:\n {prc}";
                }
            }
            if (To.Any())
            {
                result += $"List of parcels to customer:\n";
                foreach (ParcelAtCustomer prc in To)
                {
                    result += $"{j++}:\n {prc}";
                }
            }
            return result;
        }
    }

}
