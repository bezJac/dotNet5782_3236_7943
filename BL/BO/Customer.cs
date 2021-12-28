using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    /// <summary>
    /// BL customer - object contains customer details
    /// </summary>
    public class Customer
    {
        /// <summary>
        /// customer identification number  
        /// </summary>
        public int? Id { get; set; }
        /// <summary>
        /// full name of customer
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// phone number of customer
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// locaton of the customer
        /// </summary>
        public Location CustomerLocation { get; set; }
        /// <summary>
        /// list of all parcels sent by customer - (delivered and non delivered)
        /// </summary>
        public IEnumerable<ParcelAtCustomer> From { get; set; }
        /// <summary>
        /// list of all  parcels sent to customer - (delivered and non delivered
        /// </summary>
        public IEnumerable<ParcelAtCustomer> To { get; set; }
        public override string ToString()
        {
            int i = 1, j = 1;
            string result = "";
            result += $"Id: {Id}\n";
            result += $"Name: {Name}\n";
            result += $"Phone: {Phone}\n";
            result += $"Location:\n{ CustomerLocation }";
            if (From.Any())
            {
                result += $"List of parcels FROM customer:\n";
                foreach (ParcelAtCustomer prc in From)
                {
                    result += $"------{i++}------\n{prc}";
                }
            }
            if (To.Any())
            {
                result += $"List of parcels TO customer:\n";
                foreach (ParcelAtCustomer prc in To)
                {
                    result += $"------{j++}------\n{prc}";
                }
            }
            return result;
        }
    }

}
