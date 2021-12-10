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
    /// partial DalObject class responsible for CRUD of Drone Charge entities. 
    /// </summary>
    internal partial class DalObject : IDal
    {
        public void  AddDroneCharge(DroneCharge dc)
        {
            if (DataSource.Charges.Any(charge => (charge.StationId == dc.StationId && charge.DroneId==dc.DroneId)))
                throw new ExsistException("Drone is already chrging at station");
            DataSource.Charges.Add(dc);
        }
        public void RemoveDroneCharge(DroneCharge dc)
        {
            int index = DataSource.Charges.FindIndex(charge => charge.StationId == dc.StationId && charge.DroneId == dc.DroneId);
            if (index == -1)
                throw new ExsistException("drone charging at station wasen't found");
            DataSource.Charges.RemoveAt(index);
        }
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
                throw new NonExistsException("id not found");
            }
            return (DroneCharge)temp;
        }
        public IEnumerable<DroneCharge> GetAllDronecharges(Func<DroneCharge,bool> predicate = null)
        {
            if (predicate == null)
                return DataSource.Charges.ToList();
            return DataSource.Charges.Where(predicate);
        }
    }
}
