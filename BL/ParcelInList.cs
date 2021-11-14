namespace IBL.BO
{
    public class ParcelInList
    {
        public int Id { get; set; }
        public string SenderName { get; set; }
        public string TargetName { get; set; }
        public WeightCategories Weight { get; set; }
        public Priority Priority { get; set; }
        public ParcelStatus Status { get; set; }
        public override string ToString()
        {
            string result = "";
            result += $"Id: {Id}\n";
            result += $"Sending Customer name: { SenderName }\n";
            result += $"Target Customer name: { TargetName }\n";
            result += $"Weight: { Weight }\n";
            result += $"Priority: { Priority }\n";
            result += $"Status: {Status}";
            return result;

        }
    }
}
