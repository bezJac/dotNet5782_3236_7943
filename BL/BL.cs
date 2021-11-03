using BL.BO;
using IBL.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    internal class BL : IBL.IBL
    {
        IDAL.IDal myDal;
        public BL()
        {
            myDal = new DalObject.DalObject();
        }
        public void AddBaseStation(BaseStation station)
        {
            if(myDal.GetAllBaseStations().Any(st => st.Id == station.Id))
                throw new BaseStationException("$ {station.Id} exist already");
            myDal.AddBaseStation(new IDAL.DO.BaseStation
            {
                Id = station.Id,
                Name = station.Name,
                Longitude = station.Longitude,
                Lattitude = station.Lattitude,
                NumOfSlots = station.NumOfSlots,

            });
        }

        public void AddDrone(Drone drone)
        {
            throw new DroneException();
        }

        public void AddParcel(Parcel parcel)
        {
            throw new ParcelException();
        }

        public void AddCustomer(Customer customer)
        {
            throw new CustomerException();
        }
    }
}
