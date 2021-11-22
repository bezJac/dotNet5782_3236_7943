using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.BO;
using IBL.BO;
namespace BL
{
    /// <summary>
    /// partial class manages all details update related methods for BL
    /// </summary>
    public partial class BL: IBL.IBL
    {
        /// <summary>
        /// update an exsisting base station's name or total number of  charging slots
        /// </summary>
        /// <param name="id"> base station's ID </param>
        /// <param name="count"> new count if  total charging slots </param>
        /// <param name="name"> new  name </param>
        public void UpdateBaseStation(int id, int count, string name)
        {
            BaseStation station = GetBaseStation(id);
            if (station.DronesCharging.Count() > count)
                throw new BaseStationException($"base station: {id} Occupied slots exceed requested update");
            IDAL.DO.BaseStation st = new IDAL.DO.BaseStation();
            st.Id = id;
            if (name != "")
                st.Name = name;
            else
                st.Name = station.Name;
            st.Longitude = station.StationLocation.Longtitude;
            st.Lattitude = station.StationLocation.Lattitude;
            if (count == 0)
                st.NumOfSlots = station.NumOfSlots;
            else
                st.NumOfSlots = count - station.DronesCharging.Count();
            myDal.UpdateBaseStation(st);
        }

        /// <summary>
        /// update an exsisting drone's model name 
        /// </summary>
        /// <param name="id"> drone's ID </param>
        /// <param name="model"> new drone model name </param>
        public void UpdateDrone(int id, string model)
        {
            IDAL.DO.Drone dr;
            try
            {
                dr = myDal.GetDrone(id);
            }
            catch (Exception Ex)
            {
                throw new DroneException("BL: ", Ex);
            }
            dr.Model = model;
            myDal.UpdateDrone(dr);
            int index = Drones.FindIndex(dr => dr.Id == id);
            Drones[index].Model = model;
        }

        /// <summary>
        /// update an exsisting customer's name or phone number
        /// </summary>
        /// <param name="id"> customer's ID</param>
        /// <param name="phone"> new phone number</param>
        /// <param name="name"> new name </param>
        public void UpdateCustomer(int id, string phone, string name)
        {
            IDAL.DO.Customer cstmr;
            try
            {
                cstmr = myDal.GetCustomer(id);
            }
            catch (Exception Ex)
            {
                throw new CustomerException("BL: ", Ex);
            }
            if (name != "")
                cstmr.Name = name;
            if (phone != "")
                cstmr.Phone = phone;
            myDal.UpdateCustomer(cstmr);
        }
    }
}
