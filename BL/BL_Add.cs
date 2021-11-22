﻿
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
        /// <summary>
        /// add a Base Station to DAL data source
        /// </summary>
        /// <param name="station"> BL BaseStation to add </param>
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

       /// <summary>
       /// add a drone to DAL Data Source
       /// </summary>
       /// <param name="drone"> BL drone to add</param>
       /// <param name="stationId"> id of station for initial charge of drone</param>
        public void AddDrone(Drone drone, int stationId)
        {
            IDAL.DO.BaseStation st;
            try
            {
                st = myDal.GetBaseStation(stationId);
            }
            catch (Exception ex)
            {
                throw new AddException("", ex);
            }
            if (st.NumOfSlots == 0)
                throw new AddException($"base station - {stationId} has no charging slots available");
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
            Random rnd = new Random();
            Drones.Add(new DroneInList
            {
                Id = drone.Id,
                Model = drone.Model,
                MaxWeight = drone.MaxWeight,
                Status = DroneStatus.Maintenance,
                Battery = rnd.Next(20, 41),
                DroneLocation = createLocation(st.Longitude, st.Lattitude),
            });
            st.NumOfSlots--;
            myDal.AddDroneCharge(new IDAL.DO.DroneCharge { DroneId = drone.Id, StationId = st.Id });
            myDal.UpdateBaseStation(st);
        }

        /// <summary>
        /// add a parcel to DAL Data Source
        /// </summary>
        /// <param name="parcel"> BL Parcel to add </param>
        public void AddParcel(Parcel parcel)
        {
            try
            {
                myDal.GetCustomer(parcel.Sender.Id);
                myDal.GetCustomer(parcel.Target.Id);
            }
            catch (Exception Ex)
            {
                throw new AddException("", Ex);
            }
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

        /// <summary>
        /// add a customer to DAL Data Source
        /// </summary>
        /// <param name="customer"> BL customer to add </param>
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
