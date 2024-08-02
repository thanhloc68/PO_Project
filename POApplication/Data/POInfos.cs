using Microsoft.EntityFrameworkCore;
using POApplication.Models;

namespace POApplication.Data
{
    public class POInfos : DbContext
    {
        public POInfos(DbContextOptions<POInfos> options) : base(options) { }
        public DbSet<POInfomation> POInfomation { get; set; }
        public DbSet<PoDetail> PODetail { get; set; }
        public DbSet<Vehicle> Vehicle { get; set; }
        public DbSet<Transporter> Transporter { get; set; }
        public DbSet<VehicleType> VehicleType { get; set; }
    }
}
