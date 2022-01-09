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
            DO.Parcel? parcel = null;
            bool pickedUp = false;
            Customer customer = null;
            ChargeMode maintenance = ChargeMode.SetUp;

            void initDelivery(int id)
            {
                parcel = dal.GetParcel(id);
                batteryUsage = (int)Enum.Parse(typeof(BatteryUsage), parcel?.Weight.ToString());
                pickedUp = parcel?.PickedUp is not null;
                customer = bl.GetCustomer((int)(pickedUp ? parcel?.TargetId : parcel?.SenderId));
            }

            do
            {
                switch (drone.Status)
                {
                    case DroneStatus.Available:
                        {
                            if (!sleepDelayTime()) break;
                            try
                            {
                                theBl.LinkDroneToParcel((int)drone.Id);
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
                                            int index = theBl.drones.FindIndex(dr => dr.Id == drone.Id);
                                            theBl.drones[index].Status = DroneStatus.Maintenance;
                                            maintenance = ChargeMode.SetUp;
                                            break;
                                        }
                                }
                            }
                            break;
                        }
                    case DroneStatus.Maintenance:
                        {
                            try
                            {
                                st = theBl.convertToBaseStation(theBl.getNearestAvailableBasestation(drone.Location));
                                distance = Distance.GetDistance(st.StationLocation,drone.Location);

                                // check if drone has enough battery to cover flight  distance to the selected station
                                if ((drone.Battery - distance *BL.droneElecUseEmpty) <= 0)
                                    throw new ActionException($"drone - {id} does not have enough battery to reach nearest base station");
                            }
                            catch (Exception)
                            {

                                throw;
                            }
                            break;
                        }

                }
                update();
            } while (!checkStop());
        }

        private static bool sleepDelayTime()
        {
            try { Thread.Sleep(DELAY); } catch (ThreadInterruptedException) { return false; }
            return true;
        }
    }
}