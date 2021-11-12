using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;
using IDAL;

namespace DalObject
{
    public partial class DalObject :IDal
    {
        /// <summary>
        /// add a drone to drones list in data source layer
        /// </summary>
        /// <param name="dr"> Drone object to be added </param>
        /// <exception cref = "DroneExceptionDAL" > thrown if id already exists</exception>
        public void AddDrone(Drone dr)
        {
            if (DataSource.Drones.Any(drone => (drone.Id == dr.Id)))
                throw new DroneExceptionDAL("id already exists");
            DataSource.Drones.Add(dr);
        }

        /// <summary>
        /// update a dronein the list
        /// </summary>
        /// <param name="dr"> updated version of drone </param>
        ///  <exception cref = "DroneExceptionDAL"> thrown if id not founf  </exception>
        public void UpdateDrone(Drone dr)
        {
            int index = DataSource.Drones.FindIndex(x => (x.Id == dr.Id));
            if (index == -1)
                throw new DroneExceptionDAL("id not found");
            DataSource.Drones[index] = dr;
        }

        /// <summary>
        /// remove a drone from list
        /// </summary>
        /// <param name="dr"> drone to be  removed </param>
        /// <exception cref = "DroneExceptionDAL"> thrown if id not found  </exception>
        public void RemoveDrone(Drone dr)
        {
            int index = DataSource.Drones.FindIndex(x => (x.Id == dr.Id));
            if (index == -1)
                throw new DroneExceptionDAL("id not found");
            DataSource.Drones.RemoveAt(index);
        }

        /// <summary>
        /// get a copy of a single Drone 
        /// </summary>
        /// <param name="id">  drone's ID </param>
        /// <exception cref="DroneExceptionDAL"> thrown if id not founf in list </exception>
        /// <returns> copy of drone matching the id </returns>
        public Drone GetDrone(int id)
        {
            Drone? temp = null;
            foreach (Drone dr in DataSource.Drones)
            {
                if (dr.Id == id)
                {
                    temp = dr;
                    break;
                }

            }
            if (temp == null)
            {
                throw new DroneExceptionDAL("id not found");
            }
            return (Drone)temp;
        }


        /// <summary>
        /// get a copy list containing all drones 
        /// </summary>
        /// <returns> IEnumerable<Drone> type </returns>
        public IEnumerable<Drone> GetAllDrones(Predicate<Drone> predicate = null)
        {
            if (predicate == null)
                return DataSource.Drones.ToList();
            return DataSource.Drones.FindAll(predicate).ToList();
        }

    }
}
