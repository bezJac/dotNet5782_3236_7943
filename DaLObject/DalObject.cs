using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;
using DalApi;
using DS;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Linq;
using System.Runtime.CompilerServices;


namespace Dal
{

    /// <summary>
    /// class manages technical layer functions for DAL
    /// </summary>
    internal sealed partial class DalObject : IDal
    {
        #region Singleton design 
        private static DalObject instance;
        private static object locker = new();


        /// <summary>
        /// constructor - calls DataSource.initialize() to initialize lists
        /// </summary>
        private DalObject()
        {
            DataSource.Initialize();
         
        }

        /// <summary>
        /// instance of DalObject class - same object is always returned
        /// </summary>
        public static DalObject Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (locker)
                    {
                        if (instance == null)
                            instance = new DalObject();
                    }
                }
                return instance;
            }
        }
        #endregion


        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<double> GetElectricUse()
        {
            double[] electric = new double[5] { DataSource.Config.DroneElecUseEmpty,
            DataSource.Config.DroneElecUseLight, DataSource.Config.DroneElecUseMedium,
            DataSource.Config.DroneElecUseHeavy,DataSource.Config.DroneChargeRatePerSecond };
            return electric;
        }
        
    }
}


