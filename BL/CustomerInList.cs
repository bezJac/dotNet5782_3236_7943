using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class CustomerInList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public int DeliveredCount { get; set; }
        public int SentCount { get; set; }
        public int RecievedCount { get; set; }
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
