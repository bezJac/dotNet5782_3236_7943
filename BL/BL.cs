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
        public static double DroneElecUseEmpty;
        public static double DroneElecUseLight;
        public static double DroneElecUseMedium;
        public static double DroneElecUseHeavy;
        public static double DroneHourlyChargeRate;
        public IEnumerable<DroneInList> Drones;
        public BL()
        {
            myDal = new DalObject.DalObject();
            double[] temp = myDal.GetElectricUse().ToArray();
            DroneElecUseEmpty = temp[0];
            DroneElecUseLight = temp[1];
            DroneElecUseMedium = temp[2];
            DroneElecUseHeavy = temp[3];
            DroneHourlyChargeRate = temp[4];
        }
        public void AddBaseStation(BaseStation station)
        {
            if(myDal.GetAllBaseStations().Any(st => st.Id == station.Id))
                throw new BaseStationException($" {station.Id} exist already");
            myDal.AddBaseStation(new IDAL.DO.BaseStation
            {
                Id = station.Id,
                Name = station.Name,
                Longitude = station.Location.Longtitude,
                Lattitude = station.Location.Lattitude,
                NumOfSlots = station.NumOfSlots,

            });
        }

        public void AddDrone(Drone drone)
        {
            if (myDal.GetAllDrones().Any(dr => dr.Id == drone.Id))
                throw new DroneException($" {drone.Id} exist already");
            myDal.AddDrone(new IDAL.DO.Drone
            {
                Id = drone.Id,
                Model = drone.Model,
                MaxWeight=(IDAL.DO.WeightCategories)drone.MaxWeight,

            });
        }

        public void AddParcel(Parcel parcel)
        {
            if (myDal.GetAllParcels().Any(pr => pr.Id == parcel.Id))
                throw new ParcelException($" {parcel.Id} exist already");
            myDal.AddParcel(new IDAL.DO.Parcel
            {
                Id = parcel.Id,
                SenderId = parcel.Sender.Id,
                TargetId = parcel.Target.Id,
                Weight = (IDAL.DO.WeightCategories)parcel.Weight,
                Priority = (IDAL.DO.Priorities)parcel.Priority,
                DroneId = parcel.Drone.Id,
                Requested=parcel.Ordered,
                Scheduled=parcel.Linked,
                PickedUp=parcel.PickedUp,
                Delivered=parcel.Delivered,

            }) ;
        }

        public void AddCustomer(Customer customer)
        {
            if (myDal.GetAllCustomers().Any(cst => cst.Id == customer.Id))
                throw new CustomerException($" {customer.Id} exist already");
            myDal.AddCustomer(new IDAL.DO.Customer
            {
                Id = customer.Id,
                Name = customer.Name,
                Phone=customer.Phone,
                Longitude=customer.Location.Longtitude,
                Lattitude=customer.Location.Lattitude,

            });
        }
    }
}
