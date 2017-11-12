namespace Testing.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Company
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Company()
        {
            Companies1 = new HashSet<Company>();
            CompanyLocations = new HashSet<CompanyLocation>();
            CompanyProfessionals = new HashSet<CompanyProfessional>();
            Invoices = new HashSet<Invoice>();
        }

        public Guid CompanyId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CompanyIndex { get; set; }

        [Required]
        [StringLength(100)]
        public string AddressLine1 { get; set; }

        [Required]
        [StringLength(100)]
        public string AddressLine2 { get; set; }

        [Required]
        [StringLength(500)]
        public string APILiveKey { get; set; }

        [Required]
        [StringLength(500)]
        public string APITestKey { get; set; }

        public DateTime? ApprovedDate { get; set; }

        public DateTime? BannedDate { get; set; }

        [Required]
        [StringLength(500)]
        public string BannedReason { get; set; }

        [Required]
        [StringLength(100)]
        public string CompanyName { get; set; }

        [Required]
        [StringLength(100)]
        public string County { get; set; }

        public bool IsApproved { get; set; }

        public bool IsDeleted { get; set; }

        [Required]
        [StringLength(100)]
        public string MainContactEmail { get; set; }

        [Required]
        [StringLength(100)]
        public string MainContactName { get; set; }

        [Required]
        [StringLength(20)]
        public string MainContactTel { get; set; }

        [Required]
        [StringLength(1000)]
        public string Notes { get; set; }

        public Guid? OwnerProfessionalProfessionalId { get; set; }

        public Guid? ParentCompanyCompanyId { get; set; }

        [Required]
        [StringLength(10)]
        public string Postcode { get; set; }

        [Required]
        [StringLength(100)]
        public string SecondaryContactEmail { get; set; }

        [Required]
        [StringLength(100)]
        public string SecondaryContactName { get; set; }

        [Required]
        [StringLength(20)]
        public string SecondaryContactTel { get; set; }

        [Required]
        [StringLength(100)]
        public string TownCity { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Company> Companies1 { get; set; }

        public virtual Company Company1 { get; set; }

        public virtual Professional Professional { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CompanyLocation> CompanyLocations { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CompanyProfessional> CompanyProfessionals { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Invoice> Invoices { get; set; }
    }
}
