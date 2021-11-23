using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL;

namespace IBL.BO
{
    public class BaseStationInList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int AvailableSlots { get; set; }
        public int OccupiedSlots{ get; set; }
        public override string ToString()
        {
            string result = "";
            result += $"Id: {Id}\n";
            result += $"Name: {Name}\n";
            result += $"Num of Available Slots: { AvailableSlots }\n";
            result += $"Num of Occupied Slots: {OccupiedSlots}\n";
            return result;
        }
    }
}
