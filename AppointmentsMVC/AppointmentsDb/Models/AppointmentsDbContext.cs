using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentsDb.Models
{
    // Change back to  : DbContext to split out Identity.
    public class AppointmentsDbContext : IdentityDbContext<ApplicationUser>
    {
        public virtual DbSet<Appointment> Appointments { get; set; }        
        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<CompanyLocation> CompanyLocations { get; set; }
        public virtual DbSet<CompanyLocationGroup> CompanyLocationGroups { get; set; }        
        public virtual DbSet<LinkingKey> LinkingKeys { get; set; }
        public virtual DbSet<Professional> Professionals { get; set; }
        public virtual DbSet<ProfessionalWorkingHour> ProfessionalWorkingHours { get; set; }

        public AppointmentsDbContext()
            : base("name=AppointmentsDbContext")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }


        public static AppointmentsDbContext Create()
        {
            return new AppointmentsDbContext();
        }
    }
}
