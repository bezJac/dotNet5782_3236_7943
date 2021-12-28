using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    /// <summary>
    /// customer details for Parcel object interaction
    /// </summary>
    public class CustomerInParcel
    {
        /// <summary>
        /// customer identification number
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// full name of customer
        /// </summary>
        public string Name { get; set; }
        public override string ToString()
        {
            string result = "";
            result += $"Id: {Id}\n";
            result += $"Name: {Name}\n";
            return result;
        }
    }
}
