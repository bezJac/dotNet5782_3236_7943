using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.BO;
using IBL.BO;
namespace BL
{
    public partial class BL: IBL.IBL
    {
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
