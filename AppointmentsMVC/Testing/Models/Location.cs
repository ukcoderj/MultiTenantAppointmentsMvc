namespace Testing.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Location
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Location()
        {
            AvailableAppointments = new HashSet<AvailableAppointment>();
            CompanyLocations = new HashSet<CompanyLocation>();
            InvoiceCompaniesBookings = new HashSet<InvoiceCompaniesBooking>();
            ProfessionalAvailabilities = new HashSet<ProfessionalAvailability>();
            ProfessionalBookings = new HashSet<ProfessionalBooking>();
            ProfessionalLocations = new HashSet<ProfessionalLocation>();
        }

        public Guid LocationId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LocationIndex { get; set; }

        [Required]
        [StringLength(100)]
        public string AddressLine1 { get; set; }

        [Required]
        [StringLength(100)]
        public string AddressLine2 { get; set; }

        [Required]
        [StringLength(100)]
        public string County { get; set; }

        public bool IsABillingAddressOnly { get; set; }

        public bool IsAMobileLocation { get; set; }

        public bool IsAVisitableLocation { get; set; }

        public bool IsDeleted { get; set; }

        [Required]
        [StringLength(100)]
        public string LocationName { get; set; }

        [Required]
        [StringLength(10)]
        public string Postcode { get; set; }

        [Required]
        [StringLength(20)]
        public string Telephone { get; set; }

        [Required]
        [StringLength(100)]
        public string TownCity { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AvailableAppointment> AvailableAppointments { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CompanyLocation> CompanyLocations { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InvoiceCompaniesBooking> InvoiceCompaniesBookings { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProfessionalAvailability> ProfessionalAvailabilities { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProfessionalBooking> ProfessionalBookings { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProfessionalLocation> ProfessionalLocations { get; set; }
    }
}
