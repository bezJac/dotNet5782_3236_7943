using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;
using IDAL;


namespace DalObject
{

    /// <summary>
    /// class manages technical layer functions for DAL
    /// </summary>
    public class DalObject : IDal
    {


        /// <summary>
        /// constructor - calls DataSource.initialize() to initialize lists
        /// </summary>
        public DalObject()
        {
            DataSource.Initialize();
        }

        /// <summary>
        /// add a base station to stations list in data source layer
        /// </summary>
        /// <param name="st"> station object to be added </param>
        /// <exception cref = "BaseStationException"> thrown if id already exists  </exception>
        public void AddBaseStation(BaseStation st)
        {
            if(DataSource.Stations.Any(station => (station.Id == st.Id)))
                throw new BaseStationException("id already exists");
            DataSource.Stations.Add(st);
        }

        /// <summary>
        /// add a drone to drones list in data source layer
        /// </summary>
        /// <param name="dr"> Drone object to be added </param>
        /// <exception cref = "DroneException" > thrown if id already exists</exception>
        public void AddDrone(Drone dr)
        {
            if(DataSource.Drones.Any(drone => (drone.Id == dr.Id)))
                throw new DroneException("id already exists");
            DataSource.Drones.Add(dr);
        }

        /// <summary>
        /// add a customer to customers list in data source layer
        /// </summary>
        /// <param name="person"> customer object to be added </param>
        /// <exception cref = "CustomerException"> thrown if id already exists  </exception>
        public void AddCustomer(Customer person)
        {
            if(DataSource.Customers.Any(customer => (customer.Id == person.Id)))
                throw new CustomerException("id already exists");
            DataSource.Customers.Add(person);
        }

        /// <summary>
        /// add a parcel to parcels list in data source layer
        /// </summary>
        /// <param name="pack"> parcel object to be added</param>
        /// <exception cref = "ParcelException"> thrown if id already exists   </exception>
        public void AddParcel(Parcel pack)
        {
            if(DataSource.Parcels.Any(parcel => (parcel.Id == pack.Id)))
                throw new ParcelException("id already exists");
            pack.Id = ++DataSource.Config.RunIdParcel;
            DataSource.Parcels.Add(pack);
        }

        /// <summary>
        /// update a base station in the list
        /// </summary>
        /// <param name="bst"> updated version of base station </param>
        ///  <exception cref = "BaseStationException"> thrown if id not found  </exception>
        public void UpdateBaseStation(BaseStation bst)
        {
           int index = DataSource.Stations.FindIndex(x => (x.Id == bst.Id));
            if (index == -1)
                throw new BaseStationException("id not found");
            DataSource.Stations[index] = bst;
        }

        /// <summary>
        /// update a dronein the list
        /// </summary>
        /// <param name="dr"> updated version of drone </param>
        ///  <exception cref = "DroneException"> thrown if id not founf  </exception>
        public void UpdateDrone(Drone dr)
        {
            int index = DataSource.Drones.FindIndex(x => (x.Id == dr.Id));
            if (index == -1)
                throw new DroneException("id not found");
            DataSource.Drones[index] = dr;
        }


        /// <summary>
        /// update a customer in the list
        /// </summary>
        /// <param name="person"> updated version of customer </param>
        ///  <exception cref = "CustomerException"> thrown if id not founf  </exception>
        public void UpdateCustomer(Customer person)
        {
            int index = DataSource.Customers.FindIndex(x => (x.Id == person.Id));
            if (index == -1)
                throw new CustomerException("id not found");
            DataSource.Customers[index] = person;
        }

        /// <summary>
        /// update a parcel in the list
        /// </summary>
        /// <param name="person"> updated version of parcel </param>
        ///  <exception cref = "ParcelException"> thrown if id not founf  </exception>
        public void UpdateParcel(Parcel pack)
        {
            int index = DataSource.Parcels.FindIndex(x => (x.Id == pack.Id));
            if (index == -1)
                throw new ParcelException("id not found");
            DataSource.Parcels[index] =pack;
        }

        /// <summary>
        /// delete a base station from list
        /// </summary>
        /// <param name="bst"> base station to be  removed </param>
        ///  <exception cref = "BaseStationException"> thrown if id not found  </exception>
        public void RemoveBaseStation(BaseStation bst)
        {
            int index = DataSource.Stations.FindIndex(x => (x.Id == bst.Id));
            if (index == -1)
                throw new BaseStationException("id not found");
            DataSource.Stations.RemoveAt(index);
        }

        /// <summary>
        /// remove a drone from list
        /// </summary>
        /// <param name="dr"> drone to be  removed </param>
        /// <exception cref = "DroneException"> thrown if id not found  </exception>
        public void RemoveDrone(Drone dr)
        {
            int index = DataSource.Drones.FindIndex(x => (x.Id == dr.Id));
            if (index == -1)
                throw new DroneException("id not found");
            DataSource.Drones.RemoveAt(index);
        }

        /// <summary>
        /// remove a customer from list
        /// </summary>
        /// <param name="person"> customer to be removed </param>
        /// <exception cref = "CustomerException"> thrown if id not found  </exception> 
        public void RemoveCustomer(Customer person)
        {
            int index = DataSource.Customers.FindIndex(x => (x.Id == person.Id));
            if (index == -1)
                throw new CustomerException("id not found");
            DataSource.Customers.RemoveAt(index);
        }

        /// <summary>
        /// remove a parcel from list
        /// </summary>
        /// <param name="pack"> parcel to be removed </param>
        /// <exception cref = "ParcelException"> thrown if id not found  </exception> 
        public void RemoveParcel(Parcel pack)
        {
            int index = DataSource.Parcels.FindIndex(x => (x.Id == pack.Id));
            if (index == -1)
                throw new ParcelException("id not found");
            DataSource.Parcels.RemoveAt(index);
        }
       

        /// <summary>
        /// link a parcel to a drone for delivery
        /// </summary>
        /// <param name="prc"> parcel to be linked</param>
        /// <param name="dr"> drone to link to </param>
        public void LinkParcelToDrone(Parcel prc, Drone dr)
        {
            
            prc.DroneId = dr.Id;
            prc.Scheduled = DateTime.Now;

            // add  updated objects back to list 
            UpdateParcel(prc); 
        }

        /// <summary>
        /// pick up parcel by its linked drone
        /// </summary>
        /// <param name="prc"> parcel to be picked up</param>
        public void DroneParcelPickup(Parcel prc)
        {
           
            // update parcel information
            prc.PickedUp = DateTime.Now;

            // get copy of parcel's linked drone to update
            Drone tempDr = GetDrone(prc.DroneId);
            //tempDr.Status = (DroneStatus)3;

            // replace non updated objects in list with new updated objects
            UpdateDrone(tempDr);
            UpdateParcel(prc);

        }

        /// <summary>
        /// deliver parcel to customer by its linked  drone
        /// </summary>
        /// <param name="prc"> parcel to be delivered</param> 
        public void ParcelDelivery(Parcel prc)
        {
            

            // update parcel information
            prc.Delivered = DateTime.Now;

            // get copy of parcel's linked drone to update
            Drone tempDr = GetDrone(prc.DroneId);
            //tempDr.Status = (DroneStatus)1;
            prc.DroneId = 0;
            
            

            // replace non updated objects in list with new updated objects
            UpdateDrone(tempDr);
            UpdateParcel(prc);
           
        }

       /// <summary>
       /// send a drone to charge maintenance at base station
       /// </summary>
       /// <param name="bst"> drone to be charged</param>
       /// <param name="dr"> target base station</param>
        public void ChargeDrone(BaseStation bst,Drone dr)
        {
           

            // update drone and station information
            //dr.Status = (DroneStatus)2;
            bst.NumOfSlots--;

            // create new charging entity and add to list
            DroneCharge charge = new DroneCharge()
            {
                DroneId = dr.Id,
                StationId = bst.Id
            };
            DataSource.Charges.Add(charge);

            // replace non updated objects in lists with new updated objects
            UpdateDrone(dr);                // ??? why 
            UpdateBaseStation(bst);
        }

        /// <summary>
        /// end drone charge maintenance
        /// </summary>
        /// <param name="bst"> drone to be detached from charging </param>
        /// <param name="dr">  base station currently charging the drone </param>
        public void ReleaseDroneCharge(BaseStation bst, Drone dr)
        {
            int index;
            // update station information
            
            bst.NumOfSlots++;

            // remove charging entity from list 
            index = DataSource.Charges.FindIndex(x => (x.DroneId == dr.Id && x.StationId == bst.Id));
            DataSource.Charges.RemoveAt(index);

            // replace non updated objects in lists with new updated objects
            UpdateBaseStation(bst);
        
        }

        /// <summary>
        /// get a copy of a single base station 
        /// </summary>
        /// <param name = "id">  base station's ID </param>
        /// <exception cref = "BaseStationException"> thrown if id not founf in list </exception>
        /// <returns> copy of base station matching the id </returns>
        public BaseStation GetBaseStation(int id)
        {
            BaseStation? temp = null;
            foreach (BaseStation stn in DataSource.Stations)
            {
                if (stn.Id == id)
                {
                    temp = stn;
                    break;
                }
            }
            if (temp == null)
            {
                throw new BaseStationException("id not found");
            }
            return (BaseStation)temp;   
        }

        /// <summary>
        /// get a copy of a single Drone 
        /// </summary>
        /// <param name="id">  drone's ID </param>
        /// <exception cref="DroneException"> thrown if id not founf in list </exception>
        /// <returns> copy of drone matching the id </returns>
        public Drone GetDrone(int id)
        {
            Drone? temp = null;
            foreach (Drone dr in DataSource.Drones)
            {
                if (dr.Id == id)
                {
                    temp = dr;
                    break;
                }

            }
            if (temp == null)
            {
                throw new DroneException("id not found");
            }
            return (Drone)temp;
        }

        /// <summary>
        /// get a copy of a single customer 
        /// </summary>
        /// <param name="id">  customer's ID </param>
        /// <exception cref="DroneException"> thrown if id not founf in list </exception>
        /// <returns> copy of customer matching the id </returns>
        public Customer GetCustomer(int id)
        {
            Customer? temp = null;
            foreach (Customer cstmr in DataSource.Customers)
            {
                if (cstmr.Id == id)
                {
                    temp = cstmr;
                    break;
                }

            }
            if (temp == null)
            {
                throw new CustomerException("id not found");
            }
            return (Customer)temp;
        }

        /// <summary>
        /// get a copy of a single parcal
        /// </summary>
        /// <param name="id">  parcel's ID </param>
        /// <exception cref="ParcelException"> thrown if id not founf in list </exception>
        /// <returns> copy of parcel matching the id </returns>
        public Parcel GetParcel(int id)
        {
           
            Parcel? temp = null ;
            foreach (Parcel prcl in DataSource.Parcels)
            {
                if (prcl.Id == id)
                {
                    temp = prcl;
                    break;
                }

            }
            if(temp == null)
            {
                throw new ParcelException("id not found");
            }
            return (Parcel)temp;
        }

        /// <summary>
        /// get a copy of a single drone charge entity
        /// </summary>
        /// <param name="droneId">id of drone currently charging</param>
        /// <returns> copy of droneCharge entity matchibg the id </returns>
       public DroneCharge GetDroneCharge(int droneId)
        {
            DroneCharge? temp = null;
            foreach (DroneCharge dr in DataSource.Charges)
            {
                if (dr.DroneId ==droneId)
                {
                    temp = dr;
                    break;
                }

            }
            if (temp == null)
            {
                throw new DroneException("id not found");
            }
            return (DroneCharge)temp;
        }

        ///  <returns> IEnumerable<BaseStation> type </returns>
        /// <summary>
        /// get a copy list of all base stations 
        /// </summary>
        ///  <returns> IEnumerable<BaseStation> type </returns>
        public IEnumerable<BaseStation> GetAllBaseStations()
        {
            return DataSource.Stations.ToList();
        }

        /// <summary>
        /// get a copy list containing all drones 
        /// </summary>
        /// <returns> IEnumerable<Drone> type </returns>
        public IEnumerable<Drone> GetAllDrones()
        {
            return DataSource.Drones.ToList();
        }

        /// <summary>
        /// get a copy list containing all drones linked to parcels
        /// </summary>
        /// <returns> IEnumerable<Drone> type </returns>
        public IEnumerable<Drone> GetLinkedDrones()
        {
            List<Drone> temp = new List<Drone>();
            foreach (Parcel parcel in DataSource.Parcels)
            {
                if (parcel.DroneId != 0)
                    temp.Add(GetDrone(parcel.DroneId));
            }
            return temp;
        }

        /// <summary>
        /// get a copy list containing all customers
        /// </summary>
        /// <returns> IEnumerable<Customer> type </returns>
        public IEnumerable<Customer> GetAllCustomers()
        {
            return DataSource.Customers.ToList();
        }

        /// <summary>
        /// get a copy list containing all parcels 
        /// </summary>
        /// <returns> IEnumerable<Parcel> type </returns>
        public IEnumerable<Parcel> GetAllParcels()
        {
            return DataSource.Parcels.ToList();
        }

        /// <summary>
        /// get a copy list containing base Stations with available 
        /// charging slots
        /// </summary>
        /// <returns> IEnumerable<BaseStation> type </returns>
        public IEnumerable<BaseStation> GetAvailableCharge()
        {
            List<BaseStation> available = new List<BaseStation>();
            foreach (BaseStation stn in DataSource.Stations)
            {
                if (stn.NumOfSlots > 0)
                    available.Add(stn);

            }
            return available;
        }

        /// <summary>
        /// get a copy list containing parcels that are  unlinked yet to a  drone
        /// </summary>
        /// <returns> IEnumerable<Parcel> type </returns>
        public IEnumerable<Parcel> GetUnlinkedParcels()
        {
            List<Parcel> unlinked = new List<Parcel>();
            foreach (Parcel prcl in DataSource.Parcels)
            {
                if (prcl.DroneId == 0)
                    unlinked.Add(prcl);
            }
            return unlinked;
        }






        /// <summary>
        /// get a copy list containing all drone charges entity 
        /// </summary>
        /// <returns> IEnumerable<DroneCharge> type </returns>
        public IEnumerable<DroneCharge> GetAllDronecharges()
        {
            return DataSource.Charges.ToList();
        }

        /// <summary>
        /// returns  distance from a point to a base station
        /// </summary>
        /// <param name="longt"> longtitude coordinate of point </param>
        /// <param name="latt"> lattitude coordinate of point </param>
        /// <param name="station"> station picked  to calculate distance to </param>
        /// <returns> double type - Distance in KM </returns>
        public double GetDistance(double longt,double latt, BaseStation station)
        {
            return DistanceCalc(latt, longt, station.Lattitude, station.Longitude);
        }

        /// <summary>
        /// returns  distance from a point to a customer
        /// </summary>
        /// <param name="longt"> longtitude Coordinate of point </param>
        /// <param name="latt"> lattitude Coordinate of point </param>
        /// <param name="cstmr"> customer picked  to calculate distance to</param>
        /// <returns> double type - Distance in KM </returns>
        public double GetDistance(double longt, double latt, Customer cstmr)
        {
            return DistanceCalc( latt,longt,  cstmr.Lattitude, cstmr.Longitude);  
        }

        /// <summary>
        /// calculates distance between two points 
        /// represented in longtitude and lattitude Coordinates
        /// </summary>
        /// <param name="lat1"> lattitude of point 1 </param>
        /// <param name="lon1"> Longtitude of point 1 </param>
        /// <param name="lat2"> lattitude of point 2 </param>
        /// <param name="lon2"> Longtitude of point 2 </param>
        /// <returns> Distance between points in KM </returns>
        public double DistanceCalc(double lat1, double lon1, double lat2, double lon2)
        {
            // uses conversions class DegToRad function to convert results to radians
            double R = 6371;                    // Radius of the earth in km
            double dLat = StringAdapt.DegToRad(lat2 - lat1);  
            double dLon = StringAdapt.DegToRad(lon2 - lon1);
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
              Math.Cos(StringAdapt.DegToRad(lat1)) * Math.Cos(StringAdapt.DegToRad(lat2)) *
              Math.Sin(dLon / 2) * Math.Sin(dLon / 2)
              ;
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double d = R * c;                  // Distance in km
            return d;
        }

        /// <summary>
        /// get array containing with DataSource.Config fields
        /// </summary>
        /// <returns> IEnumerable<double> </returns>
        public IEnumerable<double> GetElectricUse()
        {
            double[] electric = new double[5] { DataSource.Config.DroneElecUseEmpty,
              DataSource.Config.DroneElecUseLight, DataSource.Config.DroneElecUseMedium,
            DataSource.Config.DroneElecUseHeavy,DataSource.Config.DroneHourlyChargeRate };
            return electric;
        }
    }
}
       

