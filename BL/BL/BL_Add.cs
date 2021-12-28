using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlApi;
using BO;

namespace BL
{
    public partial class BL: BlApi.IBL
    {
        public void AddBaseStation(BaseStation station)
        {
            try 
            {
                myDal.AddBaseStation(new DO.BaseStation
                {
                    Id = (int)station.Id,
                    Name = station.Name,
                    Longitude = (double)station.StationLocation.Longtitude,
                    Lattitude = (double)station.StationLocation.Lattitude,
                    NumOfSlots = (int)station.NumOfSlots,
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
            DO.BaseStation st;
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
                myDal.AddDrone(new DO.Drone
                {
                    Id = (int)drone.Id,
                    Model = drone.Model,
                    MaxWeight = (DO.WeightCategories)drone.MaxWeight,
                });
            }
            catch (Exception Ex)
            {
                throw new AddException("", Ex);
            }
            // add drone to list in BL
            
            drones.Add(new DroneInList
            {
                Id = (int)drone.Id,
                Model = drone.Model,
                MaxWeight = (WeightCategories)drone.MaxWeight,
                Status = DroneStatus.Maintenance,
                Battery = rnd.Next(20, 41),
                DroneLocation = createLocation(st.Longitude, st.Lattitude),
            });
            // update base station in dal , add new drone charge entity to list
            st.NumOfSlots--;
            myDal.AddDroneCharge(new DO.DroneCharge { DroneId = (int)drone.Id, StationId = st.Id,EntranceTime= DateTime.Now, });
            myDal.UpdateBaseStation(st);
        }
        public void AddParcel(Parcel parcel)
        {
            // check that customer id's from user are valid customers
            if (parcel.Sender.Id == parcel.Target.Id)
                throw new AddException("sending customer and target customer cannot be same person");
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
                myDal.AddParcel(new DO.Parcel
                {
                    SenderId = parcel.Sender.Id,
                    TargetId = parcel.Target.Id,
                    Weight = (DO.WeightCategories)parcel.Weight,
                    Priority = (DO.Priorities)parcel.Priority,
                    DroneId = 0,
                    Requested = DateTime.Now,
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
                myDal.AddCustomer(new DO.Customer
                {
                    Id = (int)customer.Id,
                    Name = customer.Name,
                    Phone = customer.Phone,
                    Longitude = (double)customer.CustomerLocation.Longtitude,
                    Lattitude = (double)customer.CustomerLocation.Lattitude,

                });
            }
            catch (Exception Ex)
            {
                throw new AddException("", Ex);
            }
        }
    }
}
