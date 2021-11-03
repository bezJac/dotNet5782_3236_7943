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
        public string name { get; set; }
        public string phone { get; set; }
        public int DeliveredCount { get; set; }
        public int SentCount { get; set; }
        public int RecievedCount { get; set; }
        public int ExpectedCount { get; set; }
    }
}
