namespace POApplication.Models
{
    public class POInfomation
    {
        public int id { get; set; }
        public string? CustomerRef { get; set; }
        public string? SupplierRef { get; set; }
        public int? Line { get; set; }
        public string? MaterialCode { get; set; }
        public string? MaterialDescription { get; set; }
        public decimal Quantity { get; set; }
        public string? UOM { get; set; }
        public int? NetWeight { get; set; }
        public int? GrossWeight { get; set; }
        public int? Volumn { get; set; }
    }
}
