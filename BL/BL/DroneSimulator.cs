using BlApi;
using BO;
using System;
using System.Linq;
using System.Threading;

namespace BL
{
    internal class DroneSimulator
    {
        enum ChargeMode { SetUp, Enroute, Charging }
        private const double VELOCITY = 1.0;
        private const int DELAY = 500;
        private const double TIME_STEP = DELAY / 1000.0;
        private const double STEP = VELOCITY / TIME_STEP;


        public DroneSimulator(BL bL, int id, Action update, Func<bool> checkStop)
        {
            BL theBl = bL;
            var dal = theBl.myDal;
            Drone drone = theBl.GetDrone(id);
            int? parcelId = null;
            int? baseStationId = null;
            BaseStation st = null;
            double distance = 0.0;
            int batteryUsage = 0;
            DO.Parcel parcel = new();
            bool pickedUp = false;
            Customer sender = null;
            Customer target = null;
            ChargeMode maintenance = drone.Status == DroneStatus.Maintenance ? ChargeMode.Charging : ChargeMode.SetUp;
            DateTime chargeEntrance = new();
            int index;
            void getDeliveyDetails(int id)
            {
                parcel = dal.GetParcel(id);
                batteryUsage = (int)theBl.getElectricUseForDrone((WeightCategories)parcel.Weight);
                sender = theBl.GetCustomer(parcel.SenderId);
                target = theBl.GetCustomer(parcel.TargetId);
            }

            do
            {
                index = theBl.drones.FindIndex(dr => dr.Id == drone.Id);
                switch (drone.Status)
                {
                    case DroneStatus.Available:
                        {
                            if (!sleepDelayTime()) break;
                            lock (theBl) lock (dal)
                                {
                                    
                                    try
                                    {
                                        theBl.LinkDroneToParcel((int)drone.Id);
                                        drone.Status = DroneStatus.Delivery;
                                        parcelId = theBl.drones[index].ParcelId;
                                    }
                                    catch (Exception ex)
                                    {
                                        switch (ex.Message, drone.Battery)
                                        {
                                            case ("empty", 100):
                                                {
                                                    break;
                                                }
                                            case (_, _):
                                                {

                                                    drone.Status = DroneStatus.Maintenance;
                                                    theBl.drones[index].Status = DroneStatus.Maintenance;
                                                    maintenance = ChargeMode.SetUp;
                                                    break;

                                                }
                                        }
                                    }
                                }
                            break;
                        }
                    case DroneStatus.Maintenance:
                        {
                            switch (maintenance)
                            {
                                case ChargeMode.SetUp:
                                    {
                                        lock (theBl) lock (dal)
                                            {
                                            try
                                            {

                                                st = theBl.convertToBaseStation(theBl.getNearestAvailableBasestation(drone.Location));
                                                distance = Distance.GetDistance(st.StationLocation, drone.Location);

                                                // check if drone has enough battery to cover flight  distance to the selected station
                                                if ((drone.Battery - distance * BL.droneElecUseEmpty) <= 0 || st.NumOfSlots < 1)
                                                    throw new ActionException($"charge could not be executed");
                                                maintenance = ChargeMode.Enroute;
                                            }
                                            catch (Exception ex) { break; }
                                            break;
                                        }
                                    }
                                case ChargeMode.Enroute:
                                    {
                                        if (distance < 0.01)
                                            lock (theBl) lock (dal)
                                                {
                                                drone.Location = st.StationLocation;
                                                maintenance = ChargeMode.Charging;
                                                dal.AddDroneCharge(new DO.DroneCharge { StationId = (int)st.Id, DroneId = (int)drone.Id, EntranceTime = DateTime.Now, BatteryAtEntrance = drone.Battery });
                                                theBl.drones[index].DroneLocation = st.StationLocation;
                                                chargeEntrance = DateTime.Now;
                                            }
                                        else
                                        {
                                            if (!sleepDelayTime()) break;
                                            lock (theBl)
                                            {
                                                double delta = distance < STEP ? distance : STEP;
                                                distance -= delta;
                                                drone.Battery = (int)(drone.Battery - delta * BL.droneElecUseEmpty);

                                                theBl.drones[index].Battery = drone.Battery;
                                            }
                                        }
                                        break;

                                    }
                                case ChargeMode.Charging:
                                    {
                                        TimeSpan timeSpan = DateTime.Now.Subtract(chargeEntrance);
                                        if (drone.Battery + (int)(timeSpan.Seconds * BL.DroneChargeRatePerSecond) >= 100)
                                        {
                                            lock (theBl) lock (dal)
                                                {
                                                theBl.DischargeDrone((int)drone.Id);
                                                drone.Status = DroneStatus.Available;
                                            }
                                        }
                                        else
                                        {
                                            if (!sleepDelayTime()) break;
                                            lock (theBl) 
                                                {
                                                drone.Battery = Math.Min(100, drone.Battery + (int)(chargeEntrance.Second * BL.DroneChargeRatePerSecond));
                                                theBl.drones[index].Battery = drone.Battery;
                                            }

                                        }
                                        break;
                                    }

                            }
                            break;
                        }
                    case DroneStatus.Delivery:
                        {
                            if (!sleepDelayTime()) break;
                            lock (theBl) lock (dal)
                                {

                                if (parcelId == null)
                                {
                                    getDeliveyDetails((int)theBl.drones[index].ParcelId);
                                    parcelId = theBl.drones[index].ParcelId;
                                    distance = Distance.GetDistance(drone.Location, sender.CustomerLocation);
                                }
                            }




                            if (distance < 0.01)
                                lock (theBl) lock (dal)
                                    {
                                    parcel = dal.GetParcel((int)parcelId);
                                    if (parcel.PickedUp == null)
                                    {
                                        theBl.DroneParcelPickUp((int)drone.Id);
                                    }
                                    else
                                    {
                                        theBl.DroneParcelDelivery((int)drone.Id);
                                        drone.Status = DroneStatus.Available;
                                    }
                                }
                            else
                            {
                                if (!sleepDelayTime()) break;
                                lock (theBl)
                                {
                                    double delta = distance < STEP ? distance : STEP;
                                    double proportion = delta / distance;
                                    distance -= delta;
                                    drone.Battery = drone.Battery - (int)(delta * (parcel.PickedUp == null ? BL.droneElecUseEmpty : batteryUsage));
                                    //    double lat = drone.DroneLocation.Latitude + (customer.Location.Latitude - drone.Location.Latitude) * proportion;
                                    //    double lon = drone.Location.Longitude + (customer.Location.Longitude - drone.Location.Longitude) * proportion;
                                    //    drone.Location = new() { Latitude = lat, Longitude = lon };
                                    //}
                                }
                            }
                            break;
                        }

                    default:
                        throw new Exception("Internal error: not available after Delivery...");


                }
                update();
            }

            while (!checkStop());
        }



        private static bool sleepDelayTime()
        {
            try { Thread.Sleep(DELAY); } catch (ThreadInterruptedException) { return false; }
            return true;
        }
    }
}