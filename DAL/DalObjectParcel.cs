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
    /// partial DalObject class responsible for CRUD of Parcel entities. 
    /// </summary>
    public partial class DalObject:IDal
    {

        /// <summary>
        /// add a parcel to parcels list in data source layer
        /// </summary>
        /// <param name="pack"> parcel object to be added</param>
        /// <exception cref = "ExsistException"> thrown if id already exists   </exception>
        public void AddParcel(Parcel pack)
        {
            if (DataSource.Parcels.Any(parcel => (parcel.Id == pack.Id)))
                throw new ExsistException($"id number {pack.Id} already exists");
            pack.Id = ++DataSource.Config.RunIdParcel;
            DataSource.Parcels.Add(pack);
        }

        /// <summary>
        /// update a parcel in the list
        /// </summary>
        /// <param name="person"> updated version of parcel </param>
        ///  <exception cref = "NonExistsException"> thrown if id not founf  </exception>
        public void UpdateParcel(Parcel pack)
        {
            int index = DataSource.Parcels.FindIndex(x => (x.Id == pack.Id));
            if (index == -1)
                throw new NonExistsException($"id number: {pack.Id} not found");
            DataSource.Parcels[index] = pack;
        }

        /// <summary>
        /// remove a parcel from list
        /// </summary>
        /// <param name="pack"> parcel to be removed </param>
        /// <exception cref = "NonExistsException"> thrown if id not found  </exception> 
        public void RemoveParcel(Parcel pack)
        {
            int index = DataSource.Parcels.FindIndex(x => (x.Id == pack.Id));
            if (index == -1)
                throw new NonExistsException($"id number: {pack.Id} not found");
            DataSource.Parcels.RemoveAt(index);
        }

        /// <summary>
        /// get a copy of a single parcal
        /// </summary>
        /// <param name="id">  parcel's ID </param>
        /// <exception cref="NonExistsException"> thrown if id not found in list </exception>
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
                throw new NonExistsException($"id number: {id} not found");
            }
            return (Parcel)temp;
        }



        /// <summary>
        /// get a copy list cof parcels 
        /// </summary>
        /// <param name="predicate"> condition to filter list by </param>
        /// <returns> by default an IEnumerable<Parcel> copy of full list , if predicate was sent as argument
        /// an IEnumerable<Parcel> copy of list  of entities matching predicate </returns>
        /// <exception cref = "EmptyListException"> thrown if list is empty </exception>
        /// <exception cref = "FilteredListException"> thrown if filtered list is empty </exception>
        public IEnumerable<Parcel> GetAllParcels(Predicate<Parcel> predicate = null)
        {
            if (predicate == null)
            {
                if (DataSource.Parcels.Count() <= 0)
                    throw new EmptyListException("no parcels in list");
                return DataSource.Parcels.ToList();
            }
            List<Parcel> tmp = DataSource.Parcels.FindAll(predicate).ToList();
            if (tmp.Count() > 0)
                return tmp;
            else
                throw new FilteredListException("No Parcels in list match predicate");

        }
    }
}
