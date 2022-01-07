using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;
using DalApi;
using System.Runtime.CompilerServices;
using DS;

namespace Dal
{
    internal partial class DalObject : IDal
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void  AddDroneCharge(DroneCharge dc)
        {
            if (DataSource.Charges.Any(charge => (charge.StationId == dc.StationId && charge.DroneId==dc.DroneId)))
                throw new ExsistException("Drone is already chrging at station");
            DataSource.Charges.Add(dc);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void RemoveDroneCharge(DroneCharge dc)
        {
            int index = DataSource.Charges.FindIndex(charge => charge.StationId == dc.StationId && charge.DroneId == dc.DroneId);
            if (index == -1)
                throw new ExsistException("drone charging at station wasen't found");
            DataSource.Charges.RemoveAt(index);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public DroneCharge GetDroneCharge(int droneId)
        {
            DroneCharge? temp = (from dc in DataSource.Charges
                                 where dc.DroneId == droneId
                                 select dc).FirstOrDefault();

            return temp.Value.DroneId == 0 ? throw new NonExistsException($"id number {droneId} not found") : (DroneCharge)temp;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<DroneCharge> GetAllDronecharges(Func<DroneCharge,bool> predicate = null)
        {
            return predicate == null ? DataSource.Charges : DataSource.Charges.Where(predicate);
        }
    }
}
