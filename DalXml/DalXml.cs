using DO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using System.Runtime.CompilerServices;

namespace Dal
{
    internal sealed partial class DalXml : DalApi.IDal
    {
        XElement dronesRoot;
        XMLTools xmlTool;
        string dronePath = @"DronesXml.xml";
        string stationPath = @"StationsXml.xml";
        string parcelPath = @"ParcelsXml.xml";
        string customerPath = @"CustomersXml.xml";
        string droneChargePath = @"DroneChargesXml.xml";
        
        #region Singleton design 
        private static DalXml instance;
        private static object locker = new object();
        /// <summary>
        /// constructor - calls DataSource.initialize() to initialize lists
        /// </summary>
        private DalXml()
        {
            string dir = @"..\xml\";
            xmlTool = new XMLTools();
            if (!File.Exists(dir+ dronePath))
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

        #endregion
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<double> GetElectricUse()
        {
            XElement dalConfig = XElement.Load(@"..\xml\config.xml");
            double[] electric = new double[5]{
             double.Parse(dalConfig.Element("DroneElecUseEmpty").Value),
             double.Parse(dalConfig.Element("DroneElecUseLight").Value),
             double.Parse(dalConfig.Element("DroneElecUseMedium").Value),
             double.Parse(dalConfig.Element("DroneElecUseHeavy").Value),
             double.Parse(dalConfig.Element("DroneChargeRatePerSecond").Value),
            };
            return electric;
        }
    }
}
