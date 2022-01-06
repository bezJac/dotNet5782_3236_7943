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
     internal partial class DalObject :IDal
    {
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
                throw new NonExistsException($"id number {id} not found");
            }
            return (Drone)temp;
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Drone> GetAllDrones(Func<Drone,bool> predicate = null)
        {
            if (predicate == null)
            {
                if (DataSource.Drones.Count <= 0)
                    throw new EmptyListException("no drones in list");
                return DataSource.Drones;
            }
            IEnumerable<Drone> tmp = DataSource.Drones.Where(predicate);
            if (tmp.Any())
                return tmp;
            else
                throw new FilteredListException("No Drones in list match predicate");
        }
    }
}
