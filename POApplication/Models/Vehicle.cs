namespace POApplication.Models
{
    public class Vehicle
    {
        public int ID { get; set; }
        public string? LicensePlate { get; set; }
        public string? RegistrationNumber { get; set; }
        public double? Capacity { get; set; }
        public Boolean? Status { get; set; }
        public int? VehicleTypeID { get; set; }
        public virtual VehicleType? VehicleType { get; set; }
        public int? TransporterID { get; set; }
        public virtual Transporter? Transporter { get; set; }
    }
}
