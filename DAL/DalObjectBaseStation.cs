using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;
using IDAL;

namespace DalObject
{
    public partial class DalObject : IDal
    {
        /// <summary>
        /// add a base station to stations list in data source layer
        /// </summary>
        /// <param name="st"> station object to be added </param>
        /// <exception cref = "BaseStationExceptionDAL"> thrown if id already exists  </exception>
        public void AddBaseStation(BaseStation st)
        {
            if (DataSource.Stations.Any(station => (station.Id == st.Id)))
                throw new BaseStationExceptionDAL($"id number {st.Id} already exists");
            DataSource.Stations.Add(st);
        }

        /// <summary>
        /// update a base station in the list
        /// </summary>
        /// <param name="bst"> updated version of base station </param>
        ///  <exception cref = "BaseStationExceptionDAL"> thrown if id not found  </exception>
        public void UpdateBaseStation(BaseStation bst)
        {
            int index = DataSource.Stations.FindIndex(x => (x.Id == bst.Id));
            if (index == -1)
                throw new BaseStationExceptionDAL($"id number: {bst.Id} not found");
            DataSource.Stations[index] = bst;
        }

        /// <summary>
        /// delete a base station from list
        /// </summary>
        /// <param name="bst"> base station to be  removed </param>
        ///  <exception cref = "BaseStationExceptionDAL"> thrown if id not found  </exception>
        public void RemoveBaseStation(BaseStation bst)
        {
            int index = DataSource.Stations.FindIndex(x => (x.Id == bst.Id));
            if (index == -1)
                throw new BaseStationExceptionDAL($"id number: {bst.Id} not found");
            DataSource.Stations.RemoveAt(index);
        }

        /// <summary>
        /// get a copy of a single base station 
        /// </summary>
        /// <param name = "id">  base station's ID </param>
        /// <exception cref = "BaseStationExceptionDAL"> thrown if id not founf in list </exception>
        /// <returns> copy of base station matching the id </returns>
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
                throw new BaseStationExceptionDAL($"id number: {id} not found");
            }
            return (BaseStation)temp;
        }

        ///  <returns> IEnumerable<BaseStation> type </returns>
        /// <summary>
        /// get a copy list of all base stations 
        /// </summary>
        ///  <returns> IEnumerable<BaseStation> type </returns>
        public IEnumerable<BaseStation> GetAllBaseStations(Predicate<BaseStation> predicate = null)
        {
            if (predicate == null)
            {
                if (DataSource.Stations.Count() <= 0)
                    throw new BaseStationExceptionDAL("no stations in list");
                return DataSource.Stations.ToList();
            }    
            List<BaseStation> tmp =  DataSource.Stations.FindAll(predicate).ToList();
            if (tmp.Count() > 0)
                return tmp;
            else
                throw new BaseStationExceptionDAL("No Base Stations in list match predicate");
        }
    }
}
