namespace POApplication.Models
{
    public class PoDetail
    {
        public int id { get; set; }
        public DateTime? DateShipping { get; set; }
        public int? NumberOfTrips { get; set; }
        public decimal QuantityAccordingToFlowMeter { get; set; }
        public decimal QuantityAccordingToTankMeasurement { get; set; }
        public decimal QuantityAccordingToVehicleWeight { get; set; }
        public int? POInfoID { get; set; }
    }
}