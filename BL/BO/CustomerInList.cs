using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    /// <summary>
    /// costomer details for list view
    /// </summary>
    public class CustomerInList
    {
        /// <summary>
        /// customer identification number  
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// full name of customer
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// phone number of customer
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// number of parcels sent by customer that have been delivered to target
        /// </summary>
        public int DeliveredCount { get; set; }
        /// <summary>
        /// number of parcels sent by customer waiting to be delivered
        /// </summary>
        public int SentCount { get; set; }
        /// <summary>
        /// number of parcels that have been delivered to customer
        /// </summary>
        public int RecievedCount { get; set; }
        /// <summary>
        /// number of parcels customer is expecting to be delivered to him
        /// </summary>
        public int ExpectedCount { get; set; }

        public override string ToString()
        {
            string result = "";
            result += $"Id: {Id}\n";
            result += $"Name: {Name}\n";
            result += $"Phone: {Phone}\n";
            result += $"Number of delivered parcels: {DeliveredCount}\n";
            result += $"Number of sent parcels: {SentCount}\n";
            result += $"Number of recieved parcels: {RecievedCount}\n";
            result += $"Number of expected parcels: {ExpectedCount}\n";

            return result;
        }
    }
}
