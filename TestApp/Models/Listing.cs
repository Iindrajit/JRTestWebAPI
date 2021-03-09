namespace TestApp.Models
{
    public class Listing
    {
        public string Name { get; set; }
        public double PricePerPassenger { get; set; }

        public VehicleType VehicleType { get; set; }

        public double TotalPrice { get; set; }
    }
}
