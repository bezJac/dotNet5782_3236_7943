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
        public DalObject() { DataSource.Initialize(); }
        public void addBaseStation()
        {
            int num;
            double x;
            BaseStation station = new BaseStation();
            Console.WriteLine("enter Base Station's id\n");
            int.TryParse(Console.ReadLine(), out num);
            station.Id = num;
            Console.WriteLine("enter Base Station's name\n");
            station.Name = Console.ReadLine();
            Console.WriteLine("enter Base Station's longtitude coordinate\n");
            double.TryParse(Console.ReadLine(), out x);
            station.Longitude = x;
            Console.WriteLine("enter Base Station's lattitude coordinate\n");
            double.TryParse(Console.ReadLine(), out x);
            station.Lattitude = x;
            Console.WriteLine("enter Base Station's number of charging slots\n");
            int.TryParse(Console.ReadLine(), out num);
            station.NumOfSlots = num;
            DataSource.baseStations[DataSource.Config.baseIndex] = station;
            DataSource.Config.baseIndex++;
            
        }
        public void addDrone()
        {
            int num;
            double x;
            Drone dr = new Drone();
            Console.WriteLine("enter Drone's id\n");
            int.TryParse(Console.ReadLine(), out num);
            dr.Id = num;
            Console.WriteLine("enter Drone's model\n");
            dr.Model = Console.ReadLine();
            Console.WriteLine(@"enter Drone's Max Weight category\n 
                                1: light   2: medium   3: heavy \n");
            int.TryParse(Console.ReadLine(), out num);
            dr.MaxWeight = (WeightCategories)num;
            Console.WriteLine(@"enter Drone's Status\n 
                                1: available   2: maintenance   3: delivery \n");
            int.TryParse(Console.ReadLine(), out num);
            dr.Status = (DroneStatus)num;
            Console.WriteLine("enter Drone's battery level\n");
            double.TryParse(Console.ReadLine(), out x);
            dr.Battery = x;
            DataSource.drones[DataSource.Config.droneIndex] = dr;
                DataSource.Config.droneIndex++;
            
            
        }


        // fix from here on
        public void addCustomer()
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
                DataSource.customers[DataSource.Config.customerIndex] = b;
                DataSource.Config.customerIndex++;
            }
            
        }
        public void addParcel( )
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
        public void linkParcelToDrone()
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
        public void droneParcelPickup()
        {

            dr.Status = DroneStatus.DELIVARY; 
            pa.PickedUp = DateTime.Now;
        }
        public void parcelDelivery()
        {
            
        }
        public void chargeDrone() { }
        public void releaseDroneCharge() { }
       public void printBaseStation( )
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
        public void printDrone()
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
        public void printCustomer()
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
        public void printParcel()
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
       

