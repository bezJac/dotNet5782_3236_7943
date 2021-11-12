using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;
using IDAL;

namespace DalObject
{
    public partial class DalObject : IDal
    {
        public void  AddDroneCharge(DroneCharge dc)
        {
            if (DataSource.Charges.Any(charge => (charge.StationId == dc.StationId && charge.DroneId==dc.DroneId)))
                throw new BaseStationExceptionDAL("Drone is already chrging at station");
            DataSource.Charges.Add(dc);
        }

        public void RemoveDroneCharge(DroneCharge dc)
        {
            int index = DataSource.Charges.FindIndex(charge => charge.StationId == dc.StationId && charge.DroneId == dc.DroneId);
            if (index == -1)
                throw new BaseStationExceptionDAL("drone charging at station wasen't found");
            DataSource.Charges.RemoveAt(index);
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
                if (dr.DroneId == droneId)
                {
                    temp = dr;
                    break;
                }

            }
            if (temp == null)
            {
                throw new DroneExceptionDAL("id not found");
            }
            return (DroneCharge)temp;
        }

        /// <summary>
        /// get a copy list containing all drone charges entity 
        /// </summary>
        /// <returns> IEnumerable<DroneCharge> type </returns>
        public IEnumerable<DroneCharge> GetAllDronecharges(Predicate<DroneCharge> predicate = null)
        {
            if (predicate == null)
                return DataSource.Charges.ToList();
            return DataSource.Charges.FindAll(predicate).ToList();
        }
    }
}
