using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlApi;
using BO;
namespace BL
{
    /// <summary>
    /// partial class manages all details update related methods for BL
    /// </summary>
    public partial class BL: BlApi.IBL
    {
        public void UpdateBaseStation(int id, int count, string name)
        {
            BaseStation station;
            try
            {
                 station = GetBaseStation(id);
            }
            catch (Exception ex )
            {

                throw new UpdateException("",ex);
            }
            if (station.DronesCharging.Count() > count)
                throw new UpdateException($"base station: {id} Occupied slots exceed requested update");

            DO.BaseStation st = new() { Id = id, Longitude = station.StationLocation.Longtitude, Lattitude = station.StationLocation.Lattitude };
            if (name != "")
                st.Name = name;
            else
                st.Name = station.Name;
           
            if (count == 0)
                st.NumOfSlots = station.NumOfSlots;
            else
                st.NumOfSlots = count - station.DronesCharging.Count();
            myDal.UpdateBaseStation(st);
        }
        public void UpdateDrone(int id, string model)
        {
            DO.Drone dr;
            try
            {
                dr = myDal.GetDrone(id);
            }
            catch (Exception Ex)
            {
                throw new UpdateException("", Ex);
            }
            dr.Model = model;
            myDal.UpdateDrone(dr);
            int index = drones.FindIndex(dr => dr.Id == id);
            drones[index].Model = model;
        }
        public void UpdateCustomer(int id, string phone, string name)
        {
            DO.Customer cstmr;
            try
            {
                cstmr = myDal.GetCustomer(id);
            }
            catch (Exception Ex)
            {
                throw new UpdateException("", Ex);
            }
            if (name != "")
                cstmr.Name = name;
            if (phone != "")
                cstmr.Phone = phone;
            myDal.UpdateCustomer(cstmr);
        }
    }
}
