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
    public partial class DalObject : IDal
    {
        /// <summary>
        /// constructor - calls DataSource.initialize() to initialize lists
        /// </summary>
        public DalObject()
        {
            DataSource.Initialize();
        }
        public IEnumerable<double> GetElectricUse()
        {
            double[] electric = new double[5] { DataSource.Config.DroneElecUseEmpty,
            DataSource.Config.DroneElecUseLight, DataSource.Config.DroneElecUseMedium,
            DataSource.Config.DroneElecUseHeavy,DataSource.Config.DroneHourlyChargeRate };
            return electric;
        }
    }
}
       

