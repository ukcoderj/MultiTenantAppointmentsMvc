namespace Testing.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class InvoiceCompaniesBooking
    {
        [Key]
        public Guid InvoiceCompanyBookingId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InvoiceCompanyBookingIndex { get; set; }

        public Guid AvailableAppointmentId { get; set; }

        [Column(TypeName = "money")]
        public decimal CancellationAmount { get; set; }

        [Required]
        [StringLength(200)]
        public string ClientFullName { get; set; }

        public Guid? ClientId { get; set; }

        [Column(TypeName = "money")]
        public decimal Discount { get; set; }

        public DateTime EndTime { get; set; }

        public Guid? InvoiceId { get; set; }

        public bool IsCancelled { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsPaid { get; set; }

        public Guid? LocationId { get; set; }

        [Required]
        [StringLength(200)]
        public string LocationName { get; set; }

        [Column(TypeName = "money")]
        public decimal OriginalFee { get; set; }

        [Required]
        [StringLength(200)]
        public string ProfessionalFullName { get; set; }

        public Guid? ProfessionalId { get; set; }

        public DateTime StartTime { get; set; }

        [Column(TypeName = "money")]
        public decimal TotalFee { get; set; }

        [Column(TypeName = "money")]
        public decimal TotalMinusCancellation { get; set; }

        public virtual Client Client { get; set; }

        public virtual Invoice Invoice { get; set; }

        public virtual Location Location { get; set; }

        public virtual Professional Professional { get; set; }
    }
}
