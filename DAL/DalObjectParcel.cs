using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;
using IDAL;

namespace DalObject
{
    public partial class DalObject:IDal
    {
        /// <summary>
        /// add a parcel to parcels list in data source layer
        /// </summary>
        /// <param name="pack"> parcel object to be added</param>
        /// <exception cref = "ParcelExceptionDAL"> thrown if id already exists   </exception>
        public void AddParcel(Parcel pack)
        {
            if (DataSource.Parcels.Any(parcel => (parcel.Id == pack.Id)))
                throw new ParcelExceptionDAL("id already exists");
            pack.Id = ++DataSource.Config.RunIdParcel;
            DataSource.Parcels.Add(pack);
        }

        /// <summary>
        /// update a parcel in the list
        /// </summary>
        /// <param name="person"> updated version of parcel </param>
        ///  <exception cref = "ParcelExceptionDAL"> thrown if id not founf  </exception>
        public void UpdateParcel(Parcel pack)
        {
            int index = DataSource.Parcels.FindIndex(x => (x.Id == pack.Id));
            if (index == -1)
                throw new ParcelExceptionDAL("id not found");
            DataSource.Parcels[index] = pack;
        }

        /// <summary>
        /// remove a parcel from list
        /// </summary>
        /// <param name="pack"> parcel to be removed </param>
        /// <exception cref = "ParcelExceptionDAL"> thrown if id not found  </exception> 
        public void RemoveParcel(Parcel pack)
        {
            int index = DataSource.Parcels.FindIndex(x => (x.Id == pack.Id));
            if (index == -1)
                throw new ParcelExceptionDAL("id not found");
            DataSource.Parcels.RemoveAt(index);
        }

        /// <summary>
        /// get a copy of a single parcal
        /// </summary>
        /// <param name="id">  parcel's ID </param>
        /// <exception cref="ParcelExceptionDAL"> thrown if id not founf in list </exception>
        /// <returns> copy of parcel matching the id </returns>
        public Parcel GetParcel(int id)
        {

            Parcel? temp = null;
            foreach (Parcel prcl in DataSource.Parcels)
            {
                if (prcl.Id == id)
                {
                    temp = prcl;
                    break;
                }

            }
            if (temp == null)
            {
                throw new ParcelExceptionDAL("id not found");
            }
            return (Parcel)temp;
        }

        /// <summary>
        /// get a copy list containing all parcels 
        /// </summary>
        /// <returns> IEnumerable<Parcel> type </returns>
        public IEnumerable<Parcel> GetAllParcels(Predicate<Parcel> predicate = null)
        {
            if (predicate == null)
                return DataSource.Parcels.ToList();
            return DataSource.Parcels.FindAll(predicate).ToList();

        }
    }
}
