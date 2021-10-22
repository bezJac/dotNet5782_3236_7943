using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;


namespace DalObject
{
    public class DalObject
    {
        public DalObject()
        {
            DataSource.Initialize();
        }
        public void addBaseStation(BaseStation st)
        {
            DataSource.Stations.Add(st);
        }
        public void addDrone(Drone dr)
        {
            DataSource.Drones.Add(dr);
        }
        public void addCustomer(Customer person)
        {
            DataSource.Customers.Add(person);
        }
        public void addParcel(Parcel pack)
        {
            DataSource.Parcels.Add(pack);
        }
        public void linkParcelToDrone(int id)
        {

        }
        public void droneParcelPickup()
        {


        }
        public void parcelDelivery()
        {

        }
        public void chargeDrone()
        {

        }
        public void releaseDroneCharge()
        {

        }




        public BaseStation GetBaseStation(int id)
        {
            BaseStation temp = new BaseStation();
            foreach (BaseStation stn in DataSource.Stations)
            {
                if (stn.Id == id)
                {
                    temp = stn;
                    break;
                }

            }
            return temp;
        }
        public Drone GetDrone(int id)
        {
            Drone temp = new Drone();
            foreach (Drone dr in DataSource.Drones)
            {
                if (dr.Id == id)
                {
                    temp = dr;
                    break;
                }

            }
            return temp;
        }
        public Customer GetCustomer(int id)
        {
            Customer temp = new Customer();
            foreach (Customer cstmr in DataSource.Customers)
            {
                if (cstmr.Id == id)
                {
                    temp = cstmr;
                    break;
                }

            }
            return temp;
        }
        public Parcel GetParcel(int id)
        {
            Parcel temp = new Parcel();
            foreach (Parcel prcl in DataSource.Parcels)
            {
                if (prcl.Id == id)
                {
                    temp = prcl;
                    break;
                }

            }
            return temp;
        }
        public List<BaseStation> GetAllBaseStations()
        {
            return DataSource.Stations.ToList();

        }
        public List<Drone> GetAllDrones()
        {
            return DataSource.Drones.ToList();
        }
        public List<Customer> GetAllCustomers()
        {
            return DataSource.Customers.ToList();
        }
        public List<Parcel> GetAllParcels()
        {
            return DataSource.Parcels.ToList();
        }
        
    
    }
}
       

