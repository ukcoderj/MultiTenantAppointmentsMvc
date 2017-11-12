namespace Testing.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ProfessionalBooking
    {
        public Guid ProfessionalBookingId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProfessionalBookingIndex { get; set; }

        public Guid? AvailableAppointmentId { get; set; }

        [Column(TypeName = "money")]
        public decimal CancellationAmount { get; set; }

        [Required]
        [StringLength(500)]
        public string CancellationNotes { get; set; }

        [Required]
        [StringLength(500)]
        public string CancellationTransactionId { get; set; }

        public Guid? CancelledByClientId { get; set; }

        public Guid? CancelledByProfessionalId { get; set; }

        public Guid? ClientId { get; set; }

        [StringLength(200)]
        public string ClientName { get; set; }

        [Column(TypeName = "money")]
        public decimal Discount { get; set; }

        public int DurationInMinutes { get; set; }

        public DateTime EndTime { get; set; }

        public bool IsCancelledByClient { get; set; }

        public bool IsCancelledByProfessional { get; set; }

        public bool IsClientInformedOfCancellation { get; set; }

        public bool IsConfirmed { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsPaid { get; set; }

        public Guid? LocationId { get; set; }

        [Column(TypeName = "money")]
        public decimal OriginalFee { get; set; }

        [Required]
        [StringLength(500)]
        public string PaymentIdentifier { get; set; }

        public Guid? ProfessionalId { get; set; }

        public DateTime? SmsReminderTimeSent { get; set; }

        public DateTime StartTime { get; set; }

        [Column(TypeName = "money")]
        public decimal TotalFee { get; set; }

        public int WhoMadeAppointment { get; set; }

        public Guid? WhoMadeAppointmentId { get; set; }

        public virtual Client Client { get; set; }

        public virtual Location Location { get; set; }

        public virtual Professional Professional { get; set; }
    }
}
