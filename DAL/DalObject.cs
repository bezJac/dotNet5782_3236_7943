using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;

// class controls managment operations on dalobjects

namespace DalObject
{
    public class DalObject
    {
        // constructor - calls DataSource.initialize() to initialize lists
        public DalObject()
        {
            DataSource.Initialize();
        } 

        // add base station to stations list
        public void addBaseStation(BaseStation st)
        {
            DataSource.Stations.Add(st);
        }

        // add drone to drones list
        public void addDrone(Drone dr)
        {
            DataSource.Drones.Add(dr);
        }

        // add customer to customers list
        public void addCustomer(Customer person)
        {
            DataSource.Customers.Add(person);
        }

        // add parcel to parcels list
        public void addParcel(Parcel pack)
        {
            pack.Id = ++DataSource.Config.RunIdParcel;
            DataSource.Parcels.Add(pack);
        }

        // links a parcel to a  drone for pickup
        // updates list 
        public void linkParcelToDrone(Parcel prc, Drone dr)
        {
            
            prc.DroneId = dr.Id;
            prc.Scheduled = DateTime.Now;
            int index = DataSource.Parcels.FindIndex(x => (x.Id == prc.Id));
            DataSource.Parcels.RemoveAt(index);
            DataSource.Parcels.Add(prc);
        }

        // drone linked to parcel pickes parcel up for delivery
        // updates list 
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

        // drone delivers parcel and ends link to parcel
        // updates list 
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

        // send drone to charge maintenance
        // updates lists
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

        // release drone from charging maintenance
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

        // return copy of specific base station identified by id
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

        // return copy of specific drone identified by id
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

        // return copy of specific customer identified by id
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

        // return copy of specific parcel identified by id
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

        // returns copy of base stations list
        public List<BaseStation> GetAllBaseStations()
        {
            return DataSource.Stations.ToList();

        }

        // returns copy of drones list
        public List<Drone> GetAllDrones()
        {
            return DataSource.Drones.ToList();
        }

        // returns copy of customers list
        public List<Customer> GetAllCustomers()
        {
            return DataSource.Customers.ToList();
        }

        // returns copy of parcels list
        public List<Parcel> GetAllParcels()
        {
            return DataSource.Parcels.ToList();
        }

        // returnns copy  of base stations with charging slots available list
        public List<BaseStation> GetAvailableCharge()
        {
            List<BaseStation> available = new List<BaseStation>();
            foreach (BaseStation stn in DataSource.Stations)
            {
                if (stn.NumOfSlots > 0)
                    available.Add(stn);

            }
            return available;
        }

        // return copy of  unlinked parcels list
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

        // returns distance in km from a base station to user inputed point
        public double GetDistance(double longt,double latt, BaseStation station)
        {
            return DistanceCalc(latt, longt, station.Lattitude, station.Longitude);
        }

        // returns distance in km from a customer to user inputed point
        public double GetDistance(double longt, double latt, Customer cstmr)
        {
            return DistanceCalc( latt,longt,  cstmr.Lattitude, cstmr.Longitude);
        }

        // calculates distance between two longtitude and lattitude form points
        public double DistanceCalc(double lat1, double lon1, double lat2, double lon2)
        {
            // usees conversions class function to get results in radians
            double R = 6371; // Radius of the earth in km
            double dLat = Conversions.DegToRad(lat2 - lat1);  
            double dLon = Conversions.DegToRad(lon2 - lon1);
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
              Math.Cos(Conversions.DegToRad(lat1)) * Math.Cos(Conversions.DegToRad(lat2)) *
              Math.Sin(dLon / 2) * Math.Sin(dLon / 2)
              ;
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double d = R * c; // Distance in km
            return d;
        }

    }
}
       

