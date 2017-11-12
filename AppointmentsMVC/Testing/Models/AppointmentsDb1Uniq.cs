namespace Testing.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    // TESTING ONLY. DOES NOT FEATURE IN END PRODUCT.
    public partial class AppointmentsDb1Uniq : DbContext
    {
        public AppointmentsDb1Uniq()
            : base("name=AppointmentsDb1Uniq")
        {
        }

        public virtual DbSet<C__EFMigrationsHistory> C__EFMigrationsHistory { get; set; }
        public virtual DbSet<AvailableAppointment> AvailableAppointments { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<CompanyLocation> CompanyLocations { get; set; }
        public virtual DbSet<CompanyProfessional> CompanyProfessionals { get; set; }
        public virtual DbSet<InvoiceCompaniesBooking> InvoiceCompaniesBookings { get; set; }
        public virtual DbSet<Invoice> Invoices { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<ProfessionalAvailability> ProfessionalAvailabilities { get; set; }
        public virtual DbSet<ProfessionalAvailabilityException> ProfessionalAvailabilityExceptions { get; set; }
        public virtual DbSet<ProfessionalBooking> ProfessionalBookings { get; set; }
        public virtual DbSet<ProfessionalLocation> ProfessionalLocations { get; set; }
        public virtual DbSet<Professional> Professionals { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>()
                .Property(e => e.Forename)
                .IsUnicode(false);

            modelBuilder.Entity<Company>()
                .Property(e => e.AddressLine1)
                .IsUnicode(false);

            modelBuilder.Entity<Company>()
                .Property(e => e.AddressLine2)
                .IsUnicode(false);

            modelBuilder.Entity<Company>()
                .Property(e => e.APILiveKey)
                .IsUnicode(false);

            modelBuilder.Entity<Company>()
                .Property(e => e.APITestKey)
                .IsUnicode(false);

            modelBuilder.Entity<Company>()
                .Property(e => e.BannedReason)
                .IsUnicode(false);

            modelBuilder.Entity<Company>()
                .Property(e => e.CompanyName)
                .IsUnicode(false);

            modelBuilder.Entity<Company>()
                .Property(e => e.County)
                .IsUnicode(false);

            modelBuilder.Entity<Company>()
                .Property(e => e.MainContactEmail)
                .IsUnicode(false);

            modelBuilder.Entity<Company>()
                .Property(e => e.MainContactName)
                .IsUnicode(false);

            modelBuilder.Entity<Company>()
                .Property(e => e.MainContactTel)
                .IsUnicode(false);

            modelBuilder.Entity<Company>()
                .Property(e => e.Notes)
                .IsUnicode(false);

            modelBuilder.Entity<Company>()
                .Property(e => e.Postcode)
                .IsUnicode(false);

            modelBuilder.Entity<Company>()
                .Property(e => e.SecondaryContactEmail)
                .IsUnicode(false);

            modelBuilder.Entity<Company>()
                .Property(e => e.SecondaryContactName)
                .IsUnicode(false);

            modelBuilder.Entity<Company>()
                .Property(e => e.SecondaryContactTel)
                .IsUnicode(false);

            modelBuilder.Entity<Company>()
                .Property(e => e.TownCity)
                .IsUnicode(false);

            modelBuilder.Entity<Company>()
                .HasMany(e => e.Companies1)
                .WithOptional(e => e.Company1)
                .HasForeignKey(e => e.ParentCompanyCompanyId);

            modelBuilder.Entity<Company>()
                .HasMany(e => e.CompanyLocations)
                .WithRequired(e => e.Company)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Company>()
                .HasMany(e => e.CompanyProfessionals)
                .WithRequired(e => e.Company)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<InvoiceCompaniesBooking>()
                .Property(e => e.CancellationAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<InvoiceCompaniesBooking>()
                .Property(e => e.ClientFullName)
                .IsUnicode(false);

            modelBuilder.Entity<InvoiceCompaniesBooking>()
                .Property(e => e.Discount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<InvoiceCompaniesBooking>()
                .Property(e => e.LocationName)
                .IsUnicode(false);

            modelBuilder.Entity<InvoiceCompaniesBooking>()
                .Property(e => e.OriginalFee)
                .HasPrecision(19, 4);

            modelBuilder.Entity<InvoiceCompaniesBooking>()
                .Property(e => e.ProfessionalFullName)
                .IsUnicode(false);

            modelBuilder.Entity<InvoiceCompaniesBooking>()
                .Property(e => e.TotalFee)
                .HasPrecision(19, 4);

            modelBuilder.Entity<InvoiceCompaniesBooking>()
                .Property(e => e.TotalMinusCancellation)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Invoice>()
                .Property(e => e.ChaserNotes)
                .IsUnicode(false);

            modelBuilder.Entity<Invoice>()
                .Property(e => e.CompanyName)
                .IsUnicode(false);

            modelBuilder.Entity<Invoice>()
                .Property(e => e.InvoiceFinalTotalAfterVat)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Invoice>()
                .Property(e => e.InvoiceTotalBeforeVat)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Invoice>()
                .Property(e => e.InvoiceVat)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Invoice>()
                .Property(e => e.UniqueInvoiceNumber)
                .IsUnicode(false);

            modelBuilder.Entity<Location>()
                .Property(e => e.AddressLine1)
                .IsUnicode(false);

            modelBuilder.Entity<Location>()
                .Property(e => e.AddressLine2)
                .IsUnicode(false);

            modelBuilder.Entity<Location>()
                .Property(e => e.County)
                .IsUnicode(false);

            modelBuilder.Entity<Location>()
                .Property(e => e.LocationName)
                .IsUnicode(false);

            modelBuilder.Entity<Location>()
                .Property(e => e.Postcode)
                .IsUnicode(false);

            modelBuilder.Entity<Location>()
                .Property(e => e.Telephone)
                .IsUnicode(false);

            modelBuilder.Entity<Location>()
                .Property(e => e.TownCity)
                .IsUnicode(false);

            modelBuilder.Entity<Location>()
                .HasMany(e => e.CompanyLocations)
                .WithRequired(e => e.Location)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Location>()
                .HasMany(e => e.ProfessionalLocations)
                .WithRequired(e => e.Location)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ProfessionalBooking>()
                .Property(e => e.CancellationAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<ProfessionalBooking>()
                .Property(e => e.CancellationNotes)
                .IsUnicode(false);

            modelBuilder.Entity<ProfessionalBooking>()
                .Property(e => e.CancellationTransactionId)
                .IsUnicode(false);

            modelBuilder.Entity<ProfessionalBooking>()
                .Property(e => e.ClientName)
                .IsUnicode(false);

            modelBuilder.Entity<ProfessionalBooking>()
                .Property(e => e.Discount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<ProfessionalBooking>()
                .Property(e => e.OriginalFee)
                .HasPrecision(19, 4);

            modelBuilder.Entity<ProfessionalBooking>()
                .Property(e => e.PaymentIdentifier)
                .IsUnicode(false);

            modelBuilder.Entity<ProfessionalBooking>()
                .Property(e => e.TotalFee)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Professional>()
                .Property(e => e.BannedReason)
                .IsUnicode(false);

            modelBuilder.Entity<Professional>()
                .Property(e => e.EmailAddress)
                .IsUnicode(false);

            modelBuilder.Entity<Professional>()
                .Property(e => e.Forename)
                .IsUnicode(false);

            modelBuilder.Entity<Professional>()
                .Property(e => e.MiddleName)
                .IsUnicode(false);

            modelBuilder.Entity<Professional>()
                .Property(e => e.Notes)
                .IsUnicode(false);

            modelBuilder.Entity<Professional>()
                .Property(e => e.Suffix)
                .IsUnicode(false);

            modelBuilder.Entity<Professional>()
                .Property(e => e.Surname)
                .IsUnicode(false);

            modelBuilder.Entity<Professional>()
                .Property(e => e.Telephone)
                .IsUnicode(false);

            modelBuilder.Entity<Professional>()
                .Property(e => e.TelephoneMobile)
                .IsUnicode(false);

            modelBuilder.Entity<Professional>()
                .Property(e => e.Title)
                .IsUnicode(false);

            modelBuilder.Entity<Professional>()
                .HasMany(e => e.Companies)
                .WithOptional(e => e.Professional)
                .HasForeignKey(e => e.OwnerProfessionalProfessionalId);

            modelBuilder.Entity<Professional>()
                .HasMany(e => e.CompanyProfessionals)
                .WithRequired(e => e.Professional)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Professional>()
                .HasMany(e => e.ProfessionalLocations)
                .WithRequired(e => e.Professional)
                .WillCascadeOnDelete(false);
        }
    }
}
