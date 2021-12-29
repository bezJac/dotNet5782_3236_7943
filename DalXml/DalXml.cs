using DO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace Dal
{
    internal sealed partial class DalXml : DalApi.IDal
    {
        #region Singleton design 
        private static DalXml instance;
        private static object locker = new object();

        XElement dronesRoot;

        string dronePath = @"DronesXml.xml";
        string stationPath = @"StationsXml.xml";
        string parcelPath = @"ParcelsXml.xml";
        string customerPath = @"CustomersXml.xml";
        string droneChargePath = @"DroneChargesXml.xml";

        /// <summary>
        /// constructor - calls DataSource.initialize() to initialize lists
        /// </summary>
        private DalXml()
        {
            if (!File.Exists(dronePath))
                CreateFiles();
            else
                LoadData();
        }

        /// <summary>
        /// instance of DalObject class - same object is always returned
        /// </summary>
        public static DalXml Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (locker)
                    {
                        if (instance == null)
                            instance = new DalXml();
                    }
                }
                return instance;
            }
        }

       




        public void AddBaseStation(BaseStation st)
        {
            throw new NotImplementedException();
        }

        public void AddCustomer(Customer person)
        {
            throw new NotImplementedException();
        }

        

        public void AddDroneCharge(DroneCharge dc)
        {
            throw new NotImplementedException();
        }

        public void AddParcel(Parcel pack)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BaseStation> GetAllBaseStations(Func<BaseStation, bool> predicate = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Customer> GetAllCustomers(Func<Customer, bool> predicate = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DroneCharge> GetAllDronecharges(Func<DroneCharge, bool> predicate = null)
        {
            throw new NotImplementedException();
        }

       

        public IEnumerable<Parcel> GetAllParcels(Func<Parcel, bool> predicate = null)
        {
            throw new NotImplementedException();
        }

        public BaseStation GetBaseStation(int id)
        {
            throw new NotImplementedException();
        }

        public Customer GetCustomer(int id)
        {
            throw new NotImplementedException();
        }

        
        public DroneCharge GetDroneCharge(int droneId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<double> GetElectricUse()
        {
            throw new NotImplementedException();
        }

        public Parcel GetParcel(int id)
        {
            throw new NotImplementedException();
        }

        public void RemoveBaseStation(BaseStation bst)
        {
            throw new NotImplementedException();
        }

        public void RemoveCustomer(Customer person)
        {
            throw new NotImplementedException();
        }

      

        public void RemoveDroneCharge(DroneCharge dc)
        {
            throw new NotImplementedException();
        }

        public void RemoveParcel(Parcel pack)
        {
            throw new NotImplementedException();
        }

        public void UpdateBaseStation(BaseStation bst)
        {
            throw new NotImplementedException();
        }

        public void UpdateCustomer(Customer person)
        {
            throw new NotImplementedException();
        }

       

        public void UpdateParcel(Parcel pack)
        {
            throw new NotImplementedException();
        }
    }
}
