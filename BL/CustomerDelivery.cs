using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class CustomerDelivery
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public override string ToString()
        {
            string result = "";
            result += $"Id is {Id}\n";
            result += $"Name is {Name}\n";
            return result;
        }
    }
}
