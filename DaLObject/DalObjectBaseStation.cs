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
        public void AddBaseStation(BaseStation st)
        {
            if (DataSource.Stations.Any(station => station.Id == st.Id))
                throw new ExsistException($"id number {st.Id}, already exists");
            DataSource.Stations.Add(st);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateBaseStation(BaseStation bst)
        {
            int index = DataSource.Stations.FindIndex(x => x.Id == bst.Id);
            if (index == -1)
                throw new NonExistsException($"id number {bst.Id} not found");
            DataSource.Stations[index] = bst;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void RemoveBaseStation(BaseStation bst)
        {
            int index = DataSource.Stations.FindIndex(x => x.Id == bst.Id);
            if (index == -1)
                throw new NonExistsException($"id number {bst.Id} not found");
            DataSource.Stations.RemoveAt(index);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public BaseStation GetBaseStation(int id)
        {
            BaseStation? temp = (from st in DataSource.Stations
                                 where st.Id == id
                                 select st).FirstOrDefault();

            return temp.Value.Id == 0 ? throw new NonExistsException($"id number {id} not found") : (BaseStation)temp;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<BaseStation> GetAllBaseStations(Func<BaseStation, bool> predicate = null)
        {
            if (predicate == null)
                return !DataSource.Stations.Any() ? throw new EmptyListException("No stations in list") : DataSource.Stations;

            IEnumerable<BaseStation> tmp = DataSource.Stations.Where(predicate);
            return !tmp.Any() ? throw new FilteredListException("No Base Stations in list match predicate") : tmp;
        }
    }
}
