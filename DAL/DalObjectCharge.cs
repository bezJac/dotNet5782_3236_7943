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
    /// partial DalObject class responsible for CRUD of Drone Charge entities. 
    /// </summary>
    public partial class DalObject : IDal
    {
        /// <summary>
        /// add a drone charge to  list in data source layer
        /// </summary>
        /// <param name="dc"> Drone chrge object to be added </param>
        /// <exception cref = "ExsistException" > thrown if id already exists</exception>
        public void  AddDroneCharge(DroneCharge dc)
        {
            if (DataSource.Charges.Any(charge => (charge.StationId == dc.StationId && charge.DroneId==dc.DroneId)))
                throw new ExsistException("Drone is already chrging at station");
            DataSource.Charges.Add(dc);
        }
        /// <summary>
        /// remove a drone charge from list
        /// </summary>
        /// <param name="dc">  chrge to be removed </param>
        /// <exception cref = "NonExistsException"> thrown if id not found  </exception>
        public void RemoveDroneCharge(DroneCharge dc)
        {
            int index = DataSource.Charges.FindIndex(charge => charge.StationId == dc.StationId && charge.DroneId == dc.DroneId);
            if (index == -1)
                throw new ExsistException("drone charging at station wasen't found");
            DataSource.Charges.RemoveAt(index);
        }
        /// <summary>
        /// get a copy of a single drone charge entity
        /// </summary>
        /// <param name="droneId"> id of drone currently charging</param>
        /// <exception cref = "NonExistsException"> thrown if id not found </exception>
        /// <returns> copy of droneCharge entity matching the id </returns>
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

        /// <summary>
        /// get a copy list containing of drone charges  
        /// </summary>
        /// <param name="predicate"> condition to filter list by </param>
        /// <returns> by default an IEnumerable<Drone> copy of full list , if predicate was sent as argument
        /// an IEnumerable<Drone> copy of list  of entities matching predicate </returns>
        /// <exception cref = "EmptyListException"> thrown if list is empty </exception>
        /// <exception cref = "FilteredListException"> thrown if filtered list is empty </exception>
        public IEnumerable<DroneCharge> GetAllDronecharges(Predicate<DroneCharge> predicate = null)
        {
            if (predicate == null)
                return DataSource.Charges.ToList();
            return DataSource.Charges.FindAll(predicate).ToList();
        }
    }
}
