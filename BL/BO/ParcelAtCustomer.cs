namespace BO
{
    /// <summary>
    /// parcel details for Customer object interaction
    /// </summary>
    public class ParcelAtCustomer
    {
        /// <summary>
        /// identification number of parcel
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// weight category of parcel
        /// </summary>
        public WeightCategories Weight { get; set; }
        /// <summary>
        /// priority of delivery
        /// </summary>
        public Priority Priority { get; set; }
        /// <summary>
        /// status of delivery
        /// </summary>
        public ParcelStatus Status { get; set; }
        /// <summary>
        /// details of counter coustomer (sending/recieving)
        /// </summary>
        public CustomerInParcel CounterCustomer { get; set; }

        public override string ToString()
        {

            string result = "";
            result += $"Id: {Id}\n";
            result += $"Weight: { Weight }\n";
            result += $"Priority: { Priority }\n";
            result += $"Status: {Status}\n";
            result += $"Counter Customer details are:\n{CounterCustomer}";

            return result;
        }
    }
}