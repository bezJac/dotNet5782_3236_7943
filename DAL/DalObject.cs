using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;

namespace DalObject
{
    public class DalObject
    {
        public void addBaseStation(ref BaseStation b)
        {
            bool flag = false;
            for (int i = 0; i < DataSource.baseStations.Length; i++)
            {
                 flag = DataSource.baseStations[i].Equals(b);
                if (flag)
                    break;
            }
            if (!flag)
            {
                DataSource.baseStations[DataSource.Config.baseIndex] = b;
                DataSource.Config.baseIndex++;
            }
        }
        public void addDrone(ref Drone d)
        {
            bool flag = false;
            for (int i = 0; i < DataSource.drones.Length; i++)
            {
                flag = DataSource.drones[i].Equals(d);
                if (flag)
                    break;
            }
            if (!flag)
            {
                DataSource.drones[DataSource.Config.droneIndex] = d;
                DataSource.Config.droneIndex++;
            }
            
        }
        public void addCustomer(ref Customer c)
        {
            bool flag = false;
            for (int i = 0; i < DataSource.customers.Length; i++)
            {
                flag = DataSource.customers[i].Equals(c);
                if (flag)
                    break;
            }
            if (!flag)
            {
                DataSource.customers[DataSource.Config.customerIndex] =c;
                DataSource.Config.customerIndex++;
            }
            
        }
        public void addParcel(ref Parcel pa )
        {
            pa.Requested = DateTime.Now;
            DataSource.parcels[DataSource.Config.parcelIndex] = pa;
            DataSource.Config.parcelIndex++;
            //bool flag = false;
            //for (int i = 0; i < DataSource.parcels.Length; i++)
            //{
            //    flag = DataSource.baseStations[i].Equals(p);
            //    if (flag)
            //        break;
            //}
            //if (!flag)
            //{
            //    DataSource.parcels[DataSource.Config.parcelIndex] = p;
            //    DataSource.Config.baseIndex++;
            //}

        }
        public void linkParcelToDrone(ref Parcel pa , ref Drone dr)
        {
            pa.Scheduled = DateTime.Now;
            pa.DroneId = dr.Id;
            //int i = 0;
            //for (; i < DataSource.Config.parcelIndex; i++)
            //{
            //    if (DataSource.parcels[i].Id == parc_id)
            //        break;
            //}
            //if(i<= DataSource.Config.parcelIndex)
            //{
                
            //    int j = 0;
            //    for (; i < DataSource.Config.droneIndex; i++)
            //    {
            //        if (DataSource.drones[j].Battery > 0 && DataSource.drones[j].Status == DroneStatus.AVAILABLE
            //            && DataSource.drones[j].MaxWeight >= DataSource.parcels[j].Weight)
            //            break;
            //    }
            //    if(j<= DataSource.Config.droneIndex)
            //    {
            //        DataSource.drones[j].Status = DroneStatus.DELIVARY;
            //        DataSource.parcels[i].DroneId = DataSource.drones[j].Id;
            //    }
            //}
        }
        public void droneParcelPickup(ref Parcel pa, ref Drone dr)
        {

            dr.Status = DroneStatus.DELIVARY; 
            pa.PickedUp = DateTime.Now;
        }
        public void parcelDelivery(ref Parcel pa, ref Drone dr)
        {
            pa.Delivered = DateTime.Now;
            dr.Status = DroneStatus.AVAILABLE;
            pa.DroneId = 0;
        }
       // public void chargeDrone(ref BaseStation , )
       public void printBaseStation(int _id )
        {
            for (int i = 0; i < DataSource.Config.baseIndex; i++)
            {
                 if(DataSource.baseStations[i].Id == _id)
                {
                    DataSource.baseStations[i].ToString();
                    break;
                }   
            }
        }
        public void printDrone(int _id)
        {
            for (int i = 0; i < DataSource.Config.droneIndex; i++)
            {
                if (DataSource.drones[i].Id == _id)
                {
                    DataSource.drones[i].ToString();
                    break;

                }
            }
        }
        public void printCustomer(int _id)
        {
            for (int i = 0; i < DataSource.Config.customerIndex; i++)
            {
                if (DataSource.customers[i].Id == _id)
                {
                    DataSource.customers[i].ToString();
                    break;

                }
            }
        } 
        public void printParcel(int _id)
        {
            for (int i = 0; i < DataSource.Config.parcelIndex; i++)
            {
                if (DataSource.parcels[i].Id == _id)
                {
                    DataSource.parcels[i].ToString();
                    break;

                }
            }
        }
        public void printAllBaseStations()
        {
            for (int i = 0; i < DataSource.Config.baseIndex; i++)
                DataSource.baseStations[i].ToString();
        }
        public void printAllDrones()
        {
            for (int i = 0; i < DataSource.Config.droneIndex; i++)
                DataSource.drones[i].ToString();
        }
        public void printAllCustomers()
        {
            for (int i = 0; i < DataSource.Config.customerIndex; i++)
                DataSource.customers[i].ToString();
        }
        public void printAllParcels()
        {
            for (int i = 0; i < DataSource.Config.parcelIndex; i++)
                DataSource.parcels[i].ToString();
        }
        public void printUnlinkedParcels()
        {
            for (int i = 0; i < DataSource.Config.parcelIndex; i++)
                if (DataSource.parcels[i].DroneId == 0)
                    DataSource.parcels[i].ToString();
        }
        public void printAvailableCharge()
        {
            {
                for (int i = 0; i < DataSource.Config.baseIndex; i++)
                    if(DataSource.baseStations[i].NumOfSlots>0)
                        DataSource.baseStations[i].ToString();
            }
        }
    }
}
       

