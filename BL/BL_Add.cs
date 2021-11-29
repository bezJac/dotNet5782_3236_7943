
using IBL.BO;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    /// <summary>
    /// partial class manages all add related methods for BL
    /// </summary>
    public partial class BL:IBL.IBL
    {
        public void AddBaseStation(BaseStation station)
        {
            try 
            {
                myDal.AddBaseStation(new IDAL.DO.BaseStation
                {
                    Id = station.Id,
                    Name = station.Name,
                    Longitude = station.StationLocation.Longtitude,
                    Lattitude = station.StationLocation.Lattitude,
                    NumOfSlots = station.NumOfSlots,
                });
            }
            catch (Exception ex)
            {
                throw new AddException("", ex);
            }
        }
        public void AddDrone(Drone drone ,int stationId)
        {
            Random rnd = new();
            // check that base station for charging exsist
            IDAL.DO.BaseStation st;
            try
            {
               st = myDal.GetBaseStation(stationId);
            }
            catch (Exception ex)
            {
                throw new AddException("", ex);
            }
            // check that base station has available charging slots
            if (st.NumOfSlots == 0)
                throw new AddException($"base station - {stationId} has no charging slots available");

            //// add drone to DAL
            try
            {
                myDal.AddDrone(new IDAL.DO.Drone
                {
                    Id = drone.Id,
                    Model = drone.Model,
                    MaxWeight = (IDAL.DO.WeightCategories)drone.MaxWeight,
                });
            }
            catch (Exception Ex)
            {
                throw new AddException("", Ex);
            }
            // add drone to list in BL
            
            drones.Add(new DroneInList
            {
                Id = drone.Id,
                Model = drone.Model,
                MaxWeight = drone.MaxWeight,
                Status = DroneStatus.Maintenance,
                Battery = rnd.Next(20, 41),
                DroneLocation = createLocation(st.Longitude, st.Lattitude),
            });
            // update base station in dal , add new drone charge entity to list
            st.NumOfSlots--;
            myDal.AddDroneCharge(new IDAL.DO.DroneCharge { DroneId = drone.Id, StationId = st.Id,EntranceTime= DateTime.Now, });
            myDal.UpdateBaseStation(st);
        }
        public void AddParcel(Parcel parcel)
        {
            // check that customer id's from user are valid customers
            try
            {
                myDal.GetCustomer(parcel.Sender.Id);
                myDal.GetCustomer(parcel.Target.Id);
            }
            catch (Exception Ex)
            {
                throw new AddException("", Ex);
            }
            // add parcel
            try
            {
                myDal.AddParcel(new IDAL.DO.Parcel
                {
                    SenderId = parcel.Sender.Id,
                    TargetId = parcel.Target.Id,
                    Weight = (IDAL.DO.WeightCategories)parcel.Weight,
                    Priority = (IDAL.DO.Priorities)parcel.Priority,
                    DroneId = 0,
                    Requested = parcel.Ordered,
                    Scheduled = parcel.Linked,
                    PickedUp = parcel.PickedUp,
                    Delivered = parcel.Delivered,
                });
            }
            catch (Exception Ex)
            {
                throw new AddException("", Ex);
            }
        }
        public void AddCustomer(Customer customer)
        {
            try
            {
                myDal.AddCustomer(new IDAL.DO.Customer
                {
                    Id = customer.Id,
                    Name = customer.Name,
                    Phone = customer.Phone,
                    Longitude = customer.CustomerLocation.Longtitude,
                    Lattitude = customer.CustomerLocation.Lattitude,

                });
            }
            catch (Exception Ex)
            {
                throw new AddException("", Ex);
            }
        }
    }
}
