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
    /// partial DalObject class responsible for CRUD of BaseStation entities. 
    /// </summary>
    internal partial class DalObject : IDal
    {
        public void AddBaseStation(BaseStation st)
        {
            if (DataSource.Stations.Any(station => station.Id == st.Id))
                throw new ExsistException($"id number {st.Id}, already exists");
            DataSource.Stations.Add(st);
        }
        public void UpdateBaseStation(BaseStation bst)
        {
            int index = DataSource.Stations.FindIndex(x => x.Id == bst.Id);
            if (index == -1)
                throw new NonExistsException($"id number {bst.Id} not found");
            DataSource.Stations[index] = bst;
        }
        public void RemoveBaseStation(BaseStation bst)
        {
            int index = DataSource.Stations.FindIndex(x => x.Id == bst.Id);
            if (index == -1)
                throw new NonExistsException($"id number {bst.Id} not found");
            DataSource.Stations.RemoveAt(index);
        }
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
