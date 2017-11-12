namespace Testing.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Professional
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Professional()
        {
            AvailableAppointments = new HashSet<AvailableAppointment>();
            Companies = new HashSet<Company>();
            CompanyProfessionals = new HashSet<CompanyProfessional>();
            InvoiceCompaniesBookings = new HashSet<InvoiceCompaniesBooking>();
            ProfessionalAvailabilities = new HashSet<ProfessionalAvailability>();
            ProfessionalAvailabilityExceptions = new HashSet<ProfessionalAvailabilityException>();
            ProfessionalBookings = new HashSet<ProfessionalBooking>();
            ProfessionalLocations = new HashSet<ProfessionalLocation>();
        }

        public Guid ProfessionalId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProfessionalIndex { get; set; }

        public Guid ProfessionalUserId { get; set; }

        public DateTime? ApprovalDate { get; set; }

        public DateTime? BannedDate { get; set; }

        [Required]
        [StringLength(500)]
        public string BannedReason { get; set; }

        [Required]
        [StringLength(200)]
        public string EmailAddress { get; set; }

        [Required]
        [StringLength(200)]
        public string Forename { get; set; }

        public bool IsApproved { get; set; }

        public bool IsAvailableForAppointments { get; set; }

        public bool IsDeleted { get; set; }

        [Required]
        [StringLength(200)]
        public string MiddleName { get; set; }

        [Required]
        [StringLength(1000)]
        public string Notes { get; set; }

        [Required]
        [StringLength(100)]
        public string Suffix { get; set; }

        [Required]
        [StringLength(200)]
        public string Surname { get; set; }

        [Required]
        [StringLength(20)]
        public string Telephone { get; set; }

        [Required]
        [StringLength(20)]
        public string TelephoneMobile { get; set; }

        [Required]
        [StringLength(20)]
        public string Title { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AvailableAppointment> AvailableAppointments { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Company> Companies { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CompanyProfessional> CompanyProfessionals { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InvoiceCompaniesBooking> InvoiceCompaniesBookings { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProfessionalAvailability> ProfessionalAvailabilities { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProfessionalAvailabilityException> ProfessionalAvailabilityExceptions { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProfessionalBooking> ProfessionalBookings { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProfessionalLocation> ProfessionalLocations { get; set; }
    }
}
