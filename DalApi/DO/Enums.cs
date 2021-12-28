using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// contains enums for certian  objects fields 

namespace DO
{
    /// <summary>
    /// weight carriyng abillities by a drone
    /// </summary>
    public enum WeightCategories
    {
        LIGHT = 1, MEDIUM, HEAVY
    }
    /// <summary>
    /// priority levels for a delivery 
    /// </summary>
    public enum Priorities
    {
        REGULAR = 1, EXPRESS, EMERGENCY
    }
}
