namespace BO
{
    /// <summary>
    /// BL Drone - object contains  Drone details
    /// </summary>
    public class Drone
    {
        /// <summary>
        /// identification number  of drone
        /// </summary>
        public int? Id { get; set; }
        /// <summary>
        /// drone model name
        /// </summary>
        public string Model { get; set; }
        /// <summary>
        ///  maximum weight category the drone can carry
        /// </summary>
        public WeightCategories? MaxWeight { get; set; }
        /// <summary>
        /// current status of drone
        /// </summary>
        public DroneStatus? Status { get; set; }
        /// <summary>
        /// battery level of drone 
        /// </summary>
        public int Battery { get; set; }
        /// <summary>
        /// details of parcel linked to drone
        /// </summary>
        public ParcelInDelivery Parcel { get; set; }
        /// <summary>
        /// drone current location
        /// </summary>
        public Location Location { get; set; }
        public override string ToString()
        {
            string result = "";
            result += $"ID: { Id }\n";
            result += $"Model: { Model }\n";
            result += $"MaxWeight: { MaxWeight }\n";
            result += $"Status: { Status }\n";
            result += $"Battery level: { Battery } %\n";
            result += $"Drone location:\n{Location}\n";
            if (Parcel != null)
                result += $"Parcel in delivery details are:\n {Parcel}";

            return result;
        }
    }


}