using Microsoft.EntityFrameworkCore;
using POApplication.Models;

namespace POApplication.Data
{
    public class POInfos : DbContext
    {
        public POInfos(DbContextOptions<POInfos> options) : base(options){}
        public DbSet<POInfomation> POInfomation { get; set; }
        public DbSet<PoDetail> PODetail { get; set; }

    }
}
