using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
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
            result += $"Id is {Id}\n";
            result += $"Name is {Name}\n";
            result += $"Phone is {Phone}\n";
            result += $"Number of delivered parcels {DeliveredCount}\n";
            result += $"Number of sent parcels {SentCount}\n";
            result += $"Number of recieved parcels {RecievedCount}\n";
            result += $"Number of expected parcels {ExpectedCount}\n";

            return result;
        }
}
