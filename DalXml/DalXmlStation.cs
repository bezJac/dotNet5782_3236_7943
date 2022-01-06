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
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddBaseStation(BaseStation st)
        {
            List<BaseStation> stations = XMLTools.LoadListFromXMLSerializer<BaseStation>(stationPath);
            if (stations.Any(station => station.Id == st.Id))
                throw new ExsistException($"id number {st.Id}, already exists");
            stations.Add(st);
            XMLTools.SaveListToXMLSerializer<BaseStation>(stations, stationPath);
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateBaseStation(BaseStation bst)
        {
            List<BaseStation> stations = XMLTools.LoadListFromXMLSerializer<BaseStation>(stationPath);
            int index = stations.FindIndex(x => x.Id == bst.Id);
            if (index == -1)
                throw new NonExistsException($"id number {bst.Id} not found");
            stations[index] = bst;
            XMLTools.SaveListToXMLSerializer<BaseStation>(stations, stationPath);
            
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void RemoveBaseStation(BaseStation bst)
        {
            List<BaseStation> stations = XMLTools.LoadListFromXMLSerializer<BaseStation>(stationPath);
            int index = stations.FindIndex(x => x.Id == bst.Id);
            if (index == -1)
                throw new NonExistsException($"id number {bst.Id} not found");
            stations.RemoveAt(index);
            XMLTools.SaveListToXMLSerializer<BaseStation>(stations, stationPath);
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public BaseStation GetBaseStation(int id)
        {
            IEnumerable<BaseStation> stations = XMLTools.LoadListFromXMLSerializer<BaseStation>(stationPath);
            BaseStation? temp = null;
           
            temp = (from st in stations
                        where st.Id == id
                        select st).FirstOrDefault();

            if (temp.Value.Id == 0)
                throw new NonExistsException($"id number {id} not found");
           
            return (BaseStation)temp;
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<BaseStation> GetAllBaseStations(Func<BaseStation, bool> predicate = null)
        {
            IEnumerable<BaseStation> stations = XMLTools.LoadListFromXMLSerializer<BaseStation>(stationPath);
            if (predicate == null)
            {
                if (!stations.Any())
                    throw new EmptyListException("No stations in list");
                return stations;
            }
            stations = stations.Where(predicate);
            if (stations.Any())
                return stations;
            else
                throw new FilteredListException("No Base Stations in list match predicate");
        }
    }
}
