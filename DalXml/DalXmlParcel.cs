﻿using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dal
{
    internal sealed partial class DalXml : DalApi.IDal
    {
        public void AddParcel(Parcel prc)
        {
            List<Parcel> parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(parcelPath);
            prc.Id = getParcelRunIdConfig();
            parcels.Add(prc);
            XMLTools.SaveListToXMLSerializer<Parcel>(parcels, parcelPath);
        }
        public void UpdateParcel(Parcel prc)
        {
            List<Parcel> parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(parcelPath);
            int index = parcels.FindIndex(x => x.Id == prc.Id);
            if (index == -1)
                throw new NonExistsException($"id number {prc.Id} not found");
            parcels[index] = prc;
            XMLTools.SaveListToXMLSerializer<Parcel>(parcels, parcelPath);

        }
        public void RemoveParcel(Parcel prc)
        {
            List<Parcel> parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(parcelPath);
            int index = parcels.FindIndex(x => x.Id == prc.Id);
            if (index == -1)
                throw new NonExistsException($"id number {prc.Id} not found");
            parcels.RemoveAt(index);
            XMLTools.SaveListToXMLSerializer<Parcel>(parcels, parcelPath);
        }
        public Parcel GetParcel(int id)
        {
            IEnumerable<Parcel> parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(parcelPath);
            Parcel? temp = null;
            temp = (from prc in parcels
                    where prc.Id == id
                    select prc).FirstOrDefault();
            if (temp == null)
            {
                throw new NonExistsException($"id number {id} not found");
            }
            return (Parcel)temp;
        }
        public IEnumerable<Parcel> GetAllParcels(Func<Parcel, bool> predicate = null)
        {
            IEnumerable<Parcel> parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(parcelPath);
            if (predicate == null)
            {
                if (!parcels.Any())
                    throw new EmptyListException("No parcels in list");
                return parcels;
            }
            parcels = parcels.Where(predicate);
            if (parcels.Any())
                return parcels;
            else
                throw new FilteredListException("No Base Stations in list match predicate");
        }
        private int getParcelRunIdConfig()
        {
            XElement config = XElement.Load(@"..\xml\config.xml");
            int runId = Convert.ToInt32(config.Element("runId").Value);
            XElement configElement = (from dr in config.Elements()
                                     where dr.Name== "runId"
                                      select dr).FirstOrDefault();
            configElement.Value = (runId + 1).ToString();
            config.Save(@"..\xml\config.xml");
            return runId+1;
        }
    }
}

