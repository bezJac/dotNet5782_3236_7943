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
            pack.Id = ++DataSource.Config.RunIdParcel;
            DataSource.Parcels.Add(pack);
        }
        public void linkParcelToDrone(Parcel prc, Drone dr)
        {
            
            prc.DroneId = dr.Id;
            prc.Scheduled = DateTime.Now;
            int index = DataSource.Parcels.FindIndex(x => (x.Id == prc.Id));
            DataSource.Parcels.RemoveAt(index);
            DataSource.Parcels.Add(prc);
        }
        public void droneParcelPickup(Parcel prc)
        {
           
            Drone tempDr = GetDrone(prc.DroneId); 
            prc.PickedUp = DateTime.Now;
            tempDr.Status = (DroneStatus)3;
            int indexDrone = DataSource.Drones.FindIndex(x => (x.Id == prc.DroneId));
            DataSource.Drones.RemoveAt(indexDrone);
            DataSource.Drones.Add(tempDr);
            int index = DataSource.Parcels.FindIndex(x => (x.Id == prc.Id));
            DataSource.Parcels.RemoveAt(index);
            DataSource.Parcels.Add(prc);

        }
        public void parcelDelivery(Parcel prc)
        {
            Drone tempDr = GetDrone(prc.DroneId);
            prc.Delivered = DateTime.Now;
            tempDr.Status = (DroneStatus)1;
            prc.DroneId = 0;
            prc.SenderId = 0;
            prc.TargetId = 0;
            int indexDrone = DataSource.Drones.FindIndex(x => (x.Id == prc.DroneId));
            DataSource.Drones.RemoveAt(indexDrone);
            DataSource.Drones.Add(tempDr);
            int index = DataSource.Parcels.FindIndex(x => (x.Id == prc.Id));
            DataSource.Parcels.RemoveAt(index);
            DataSource.Parcels.Add(prc);
        }
        public void chargeDrone(BaseStation bst,Drone dr)
        {
            dr.Status = (DroneStatus)2;
            bst.NumOfSlots--;
            DroneCharge charge = new DroneCharge()
            {
                DroneId = dr.Id,
                StationId = bst.Id
            };
            DataSource.Charges.Add(charge);
            int indexDrone = DataSource.Drones.FindIndex(x => (x.Id == dr.Id));
            DataSource.Drones.RemoveAt(indexDrone);
            DataSource.Drones.Add(dr);
            int indexBst = DataSource.Stations.FindIndex(x => (x.Id == bst.Id));
            DataSource.Stations.RemoveAt(indexBst);
            DataSource.Stations.Add(bst);

        }
        public void releaseDroneCharge(BaseStation bst, Drone dr)
        {
            dr.Status = (DroneStatus)1;
            bst.NumOfSlots++;
            int indexCharge = DataSource.Charges.FindIndex(x => (x.DroneId == dr.Id && x.StationId == bst.Id));
            DataSource.Drones.RemoveAt(indexCharge);

            int indexDrone = DataSource.Drones.FindIndex(x => (x.Id == dr.Id));
            DataSource.Drones.RemoveAt(indexDrone);
            DataSource.Drones.Add(dr);
            int indexBst = DataSource.Stations.FindIndex(x => (x.Id == bst.Id));
            DataSource.Stations.RemoveAt(indexBst);
            DataSource.Stations.Add(bst);
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

        public List<BaseStation> GetAvailableCharge()
        {
            List<BaseStation> available = new List<BaseStation>();
            foreach (BaseStation stn in DataSource.Stations)
            {
                if (stn.Id > 0)
                    available.Add(stn);

            }
            return available;
        }
        public List<Parcel> GetUnlinkedParcels()
        {
            List<Parcel> unlinked = new List<Parcel>();
            foreach (Parcel prcl in DataSource.Parcels)
            {
                if (prcl.DroneId == 0)
                    unlinked.Add(prcl);

            }
            return unlinked;
        }
    }
}
       

