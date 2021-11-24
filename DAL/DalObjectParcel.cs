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
        public void AddParcel(Parcel pack)
        {
            if (DataSource.Parcels.Any(parcel => (parcel.Id == pack.Id)))
                throw new ExsistException($"id number {pack.Id} already exists");
            pack.Id = ++DataSource.Config.RunIdParcel;
            DataSource.Parcels.Add(pack);
        }
        public void UpdateParcel(Parcel pack)
        {
            int index = DataSource.Parcels.FindIndex(x => (x.Id == pack.Id));
            if (index == -1)
                throw new NonExistsException($"id number {pack.Id} not found");
            DataSource.Parcels[index] = pack;
        }
        public void RemoveParcel(Parcel pack)
        {
            int index = DataSource.Parcels.FindIndex(x => (x.Id == pack.Id));
            if (index == -1)
                throw new NonExistsException($"id number {pack.Id} not found");
            DataSource.Parcels.RemoveAt(index);
        }
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
                throw new NonExistsException($"id number {id} not found");
            }
            return (Parcel)temp;
        }
        public IEnumerable<Parcel> GetAllParcels(Func<Parcel,bool> predicate = null)
        {
            if (predicate == null)
            {
                if (DataSource.Parcels.Count() <= 0)
                    throw new EmptyListException("no parcels in list");
                return DataSource.Parcels.ToList();
            }
            IEnumerable<Parcel> tmp = DataSource.Parcels.Where(predicate);
            if (tmp.Count() > 0)
                return tmp;
            else
                throw new FilteredListException("No Parcels in list match predicate");

        }
    }
}
