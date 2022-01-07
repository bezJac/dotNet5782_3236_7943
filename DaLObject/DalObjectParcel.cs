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
        public void AddParcel(Parcel pack)
        {
            if (DataSource.Parcels.Any(parcel => parcel.Id == pack.Id))
                throw new ExsistException($"id number {pack.Id} already exists");
            pack.Id = ++DataSource.Config.RunIdParcel;
            DataSource.Parcels.Add(pack);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateParcel(Parcel pack)
        {
            int index = DataSource.Parcels.FindIndex(x => x.Id == pack.Id);
            if (index == -1)
                throw new NonExistsException($"id number {pack.Id} not found");
            DataSource.Parcels[index] = pack;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void RemoveParcel(Parcel pack)
        {
            int index = DataSource.Parcels.FindIndex(x => x.Id == pack.Id);
            if (index == -1)
                throw new NonExistsException($"id number {pack.Id} not found");
            DataSource.Parcels.RemoveAt(index);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public Parcel GetParcel(int id)
        {

            Parcel? temp = (from prc in DataSource.Parcels
                            where prc.Id == id
                            select prc).FirstOrDefault();
            return temp.Value.Id == 0 ? throw new NonExistsException($"id number {id} not found") : (Parcel)temp;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Parcel> GetAllParcels(Func<Parcel, bool> predicate = null)
        {
            if (predicate == null)
                return !DataSource.Parcels.Any() ? throw new EmptyListException("no parcels in list") : DataSource.Parcels;

            IEnumerable<Parcel> tmp = DataSource.Parcels.Where(predicate);
            return !tmp.Any() ? throw new FilteredListException("No Parcels in list match predicate") : tmp;

        }
    }
}
