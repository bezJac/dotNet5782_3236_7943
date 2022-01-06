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
            BaseStation? temp = null;
            foreach (BaseStation stn in DataSource.Stations)
            {
                if (stn.Id == id)
                {
                    temp = stn;
                    break;
                }
            }
            if (temp == null)
            {
                throw new NonExistsException($"id number {id} not found");
            }
            return (BaseStation)temp;
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<BaseStation> GetAllBaseStations(Func<BaseStation, bool> predicate = null)
        {
            if (predicate == null)
            {
                if (!DataSource.Stations.Any())
                    throw new EmptyListException("No stations in list");
                return DataSource.Stations;
            }    
            IEnumerable<BaseStation> tmp = DataSource.Stations.Where(predicate);
            if (tmp.Any())
                return tmp;
            else
                throw new FilteredListException("No Base Stations in list match predicate");
        }
    }
}
