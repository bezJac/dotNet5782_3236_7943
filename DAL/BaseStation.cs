﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    namespace DO
    {
        struct BaseStation
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public double Longitude { get; set; }
            public double Latitude { get; set; }
            public int NumOfSlots { get; set; }
            public override string ToString()
            {
                string result = "";
                result += $"Id is {Id}\n";
                result += $"Id is {Id}\n";
                result += $"Id is {Id}\n";
                result += $"Id is {Id}\n";
                return result;
            }


        }
    }
    
}
