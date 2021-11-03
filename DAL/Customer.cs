﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//
namespace IDAL
{
    namespace DO
    {
        /// <summary>
        /// struct representing a customer's data
        /// </summary>
        public struct Customer
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Phone { get; set; }
            public double Longitude { get; set; }
            public double Lattitude { get; set; }
            
            public override string ToString()
            {
                string result = "";
                result += "Id is " + Id + "\n";
                result += "Name is " + Name + "\n";
                result += "Phone is " + Phone + "\n";
                result += "Longitude is " + StringAdapt.LongtitudeToDMS(Longitude) + "\n";
                result += "Latitude is " + StringAdapt.LattitudeToDMS(Lattitude) + "\n";
                return result;
            }


        }
    }

}
