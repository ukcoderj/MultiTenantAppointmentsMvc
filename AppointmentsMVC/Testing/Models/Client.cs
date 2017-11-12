namespace Testing.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Client
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Client()
        {
            InvoiceCompaniesBookings = new HashSet<InvoiceCompaniesBooking>();
            ProfessionalBookings = new HashSet<ProfessionalBooking>();
        }

        public Guid ClientId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ClientIndex { get; set; }

        [Required]
        [StringLength(200)]
        public string Forename { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InvoiceCompaniesBooking> InvoiceCompaniesBookings { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProfessionalBooking> ProfessionalBookings { get; set; }
    }
}
