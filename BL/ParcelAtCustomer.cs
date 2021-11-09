namespace IBL.BO
{
    public class ParcelAtCustomer
    {
        public int Id { get; set; }
        public WeightCategories Weight { get; set; }
        public Priority Priority { get; set; }
        public ParcelStatus Status { get; set; }
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