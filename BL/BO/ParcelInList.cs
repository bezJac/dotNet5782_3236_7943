namespace BO
{
    /// <summary>
    /// Parcel details for list view 
    /// </summary>
    public class ParcelInList
    {
        /// <summary>
        /// parcel identification number
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// name of sending customer
        /// </summary>
        public string SenderName { get; set; }
        /// <summary>
        /// name of recieving customer
        /// </summary>
        public string TargetName { get; set; }
        /// <summary>
        /// weight category of parcel
        /// </summary>
        public WeightCategories Weight { get; set; }
        /// <summary>
        /// priority of the delivery
        /// </summary>
        public Priority Priority { get; set; }
        /// <summary>
        /// status of delivery
        /// </summary>   
        public ParcelStatus Status { get; set; }
        public override string ToString()
        {
            string result = "";
            result += $"Id: {Id}\n";
            result += $"Sending Customer name: { SenderName }\n";
            result += $"Target Customer name: { TargetName }\n";
            result += $"Weight: { Weight }\n";
            result += $"Priority: { Priority }\n";
            result += $"Status: {Status}\n";
            return result;

        }
    }
}
