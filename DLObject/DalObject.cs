using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;
using DalApi;
using DS;

namespace Dal
{

    /// <summary>
    /// class manages technical layer functions for DAL
    /// </summary>
    internal sealed partial class DalObject : IDal
    {
        static readonly DalObject instance = new DalObject();

        static DalObject()
        {
            DataSource.Initialize();
        }
       
        /// <summary>
        /// constructor - calls DataSource.initialize() to initialize lists
        /// </summary>
        DalObject()
        {
            DataSource.Initialize();
        }

        public static DalObject Instance { get { return instance; } }
        public IEnumerable<double> GetElectricUse()
        {
            double[] electric = new double[5] { DataSource.Config.DroneElecUseEmpty,
            DataSource.Config.DroneElecUseLight, DataSource.Config.DroneElecUseMedium,
            DataSource.Config.DroneElecUseHeavy,DataSource.Config.DroneHourlyChargeRate };
            return electric;
        }
    }
}
       

