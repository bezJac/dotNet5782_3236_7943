using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using DO;
using DalApi;
using DS;

namespace Dal
{
    internal partial class DalObject : IDal
    {
        // [MethodImpl(MethodImplOptions.Synchronized)] attribute is used to ensure that  only one thread at a time can executs function, uses instance of class object
        // calling method to lock, locks entire function that attribute is added to.

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddDrone(Drone dr)
        {
            if (DataSource.Drones.Any(drone => (drone.Id == dr.Id)))
                throw new ExsistException($"id number {dr.Id} already exists");
            DataSource.Drones.Add(dr);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateDrone(Drone dr)
        {
            int index = DataSource.Drones.FindIndex(x => (x.Id == dr.Id));
            if (index == -1)
                throw new NonExistsException($"id number {dr.Id} not found");
            DataSource.Drones[index] = dr;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void RemoveDrone(Drone dr)
        {
            int index = DataSource.Drones.FindIndex(x => (x.Id == dr.Id));
            if (index == -1)
                throw new NonExistsException($"id number {dr.Id} not found");
            DataSource.Drones.RemoveAt(index);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public Drone GetDrone(int id)
        {
            Drone? temp = (from dr in DataSource.Drones
                           where dr.Id == id
                           select dr).FirstOrDefault();

            return temp.Value.Id == 0 ? throw new NonExistsException($"id number {id} not found") : (Drone)temp;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Drone> GetAllDrones(Func<Drone, bool> predicate = null)
        {
            if (predicate == null)
                return !DataSource.Drones.Any() ? throw new EmptyListException("no drones in list") : DataSource.Drones;

            IEnumerable<Drone> tmp = DataSource.Drones.Where(predicate);
            return !tmp.Any() ? throw new FilteredListException("No Drones in list match predicate") : tmp;
        }
    }
}
