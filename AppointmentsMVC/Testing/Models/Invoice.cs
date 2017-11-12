namespace Testing.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Invoice
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Invoice()
        {
            InvoiceCompaniesBookings = new HashSet<InvoiceCompaniesBooking>();
        }

        public Guid InvoiceId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InvoiceIndex { get; set; }

        [Required]
        [StringLength(2000)]
        public string ChaserNotes { get; set; }

        public Guid? CompanyId { get; set; }

        [Required]
        [StringLength(200)]
        public string CompanyName { get; set; }

        public bool HasCompanyPaidUs { get; set; }

        [Column(TypeName = "money")]
        public decimal InvoiceFinalTotalAfterVat { get; set; }

        [Column(TypeName = "money")]
        public decimal InvoiceTotalBeforeVat { get; set; }

        [Column(TypeName = "money")]
        public decimal InvoiceVat { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime PaymentDue { get; set; }

        public DateTime PeriodFrom { get; set; }

        public DateTime PeriodTo { get; set; }

        [StringLength(200)]
        public string UniqueInvoiceNumber { get; set; }

        public virtual Company Company { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InvoiceCompaniesBooking> InvoiceCompaniesBookings { get; set; }
    }
}
