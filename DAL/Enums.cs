using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// contains enums for certian  objects fields 
namespace IDAL
{
    namespace DO
    {
        public enum WeightCategories
        {
          LIGHT = 1, MEDIUM, HEAVY
        }
        public enum Priorities
        {
            REGULAR =1 , EXPRESS, EMERGENCY
        }
        public enum DroneStatus
        {
            AVAILABLE = 1 , MAIENTNANCE, DELIVARY
        }

    }
}