using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BO
{
    /// <summary>
    /// weight carriyng abillities by a drone
    /// </summary>
    public enum WeightCategories
    {
        Light = 1, Medium, Heavy
    }
    /// <summary>
    /// priority levels for a delivery 
    /// </summary>
    public enum Priority
    {
        Regular = 1, Express, Emergency
    }
    /// <summary>
    /// <para>
    /// options for status of drone. 
    /// </para>
    /// <para>
    /// Available - free to link to a delivery or charge.
    /// Maintanence -currently charging at a base station.
    /// Delivery - occupied with delivery proccess.
    /// </para>
    /// </summary>
    public enum DroneStatus
    {
        Available = 1, Maintenance, Delivery
    }
    /// <summary>
    /// <para>
    /// options for status of a delivery - (parcel)
    /// </para>
    /// <para>
    /// Ordered - delivery was ordered - waiting for link.
    /// Linked - drone was linked to execyte the delivery.
    /// PickedUp - linked Parcel picked up the delivery.
    /// Delivered - linked drone delivered the parcel.
    /// </para>
    /// </summary>
    public enum ParcelStatus
    {
        Ordered = 1, Linked, PickedUp, Delivered
    }
}

