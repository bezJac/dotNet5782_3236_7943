using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace Dal
{
    internal sealed partial class DalXml : DalApi.IDal
    {
        // [MethodImpl(MethodImplOptions.Synchronized)] attribute is used to ensure that  only one thread at a time can executs function, uses instance of class object
        // calling method to lock, locks entire function that attribute is added to.

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddDroneCharge(DroneCharge dc)
        {
            List<DroneCharge> charges = XMLTools.LoadListFromXMLSerializer<DroneCharge>(droneChargePath);
            if (charges.Any(charge => (charge.StationId == dc.StationId && charge.DroneId == dc.DroneId)))
                throw new ExsistException("Drone is already chrging at station");
            charges.Add(dc);
            XMLTools.SaveListToXMLSerializer<DroneCharge>(charges, droneChargePath);
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void RemoveDroneCharge(DroneCharge dc)
        {
            List<DroneCharge> charges = XMLTools.LoadListFromXMLSerializer<DroneCharge>(droneChargePath);
            int index = charges.FindIndex(charge => charge.StationId == dc.StationId && charge.DroneId == dc.DroneId);
            if (index == -1)
                throw new ExsistException("drone charging at station wasen't found");
            charges.RemoveAt(index);
            XMLTools.SaveListToXMLSerializer<DroneCharge>(charges, droneChargePath);
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public DroneCharge GetDroneCharge(int droneId)
        {
            IEnumerable<DroneCharge> charges = XMLTools.LoadListFromXMLSerializer<DroneCharge>(droneChargePath);
            DroneCharge? temp;
            try
            {
                temp = (from dc in charges
                        where dc.DroneId == droneId
                        select dc).First();
            }
            catch (Exception)
            {
                throw new NonExistsException("id not found");
            }
            return (DroneCharge)temp;
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<DroneCharge> GetAllDronecharges(Func<DroneCharge, bool> predicate = null)
        {
            IEnumerable<DroneCharge> charges = XMLTools.LoadListFromXMLSerializer<DroneCharge>(droneChargePath);
            if (predicate == null)
                return charges;
            return charges.Where(predicate);
        }
    }
}
