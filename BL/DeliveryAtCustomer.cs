namespace IBL.BO
{
    public class DeliveryAtCustomer
    {
        public int Id { get; set; }
        public WeightCategories Weight { get; set; }
        public Priority Priority { get; set; }
        public ParcelStatus Status { get; set; }
        public CustomerDelivery Customer { get; set; }
    }
}