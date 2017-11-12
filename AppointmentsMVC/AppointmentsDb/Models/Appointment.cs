using Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentsDb.Models
{
    public partial class Appointment
    {
        // IF UPDATING, REMEMBER TO UPDATE THE BIND STATEMENTS IN THE API!

        public Guid AppointmentId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AppointmentIndex { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please select an appointment type")]
        [Display(Name = "Type")]
        public AppointmentType AppointmentType { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 2)]
        public string Forename { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 2)]
        public string Surname { get; set; }

        [Display(Name = "Email Address")]
        [StringLength(200, MinimumLength = 2)]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }

        [Display(Name = "Phone Number")]
        [RegularExpression(@"^(((\+44\s?\d{4}|\(?0\d{4}\)?)\s?\d{3}\s?\d{3})|((\+44\s?\d{3}|\(?0\d{3}\)?)\s?\d{3}\s?\d{4})|((\+44\s?\d{2}|\(?0\d{2}\)?)\s?\d{4}\s?\d{4}))(\s?\#(\d{4}|\d{3}))?$", ErrorMessage = "Please enter a valid phone number"), StringLength(12)]
        public string Telephone { get; set; } = "";

        [Required]
        [Display(Name = "Mobile Number")]
        [RegularExpression(@"^(((\+44\s?\d{4}|\(?0\d{4}\)?)\s?\d{3}\s?\d{3})|((\+44\s?\d{3}|\(?0\d{3}\)?)\s?\d{3}\s?\d{4})|((\+44\s?\d{2}|\(?0\d{2}\)?)\s?\d{4}\s?\d{4}))(\s?\#(\d{4}|\d{3}))?$", ErrorMessage = "Please enter a valid phone number"), StringLength(12)]
        public string TelephoneMobile { get; set; } = "";


        #region "Appointment Time"

        [Required]
        [Display(Name = "Start Time")]
        public DateTime StartTime { get; set; }

        [Required]
        [Display(Name = "Duration")]
        public int DurationInMinutes { get; set; }

        [Required]
        [Display(Name = "End Time")]
        public DateTime EndTime { get; set; }

        #endregion


        #region "Creation - i.e. who made the appointment."

        [Required]
        public UserType CreatedBy { get; set; }

        /// <summary>
        /// Who made the appointment(pro id, or client id)
        /// </summary>
        [Required]
        public Guid CreatedById { get; set; }


        [Required]
        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// If there are several steps to the process, 
        /// set this at the end so we know we're definitely done.
        /// </summary>
        [Required]
        public bool IsConfirmed { get; set; }

        #endregion


        #region "Cancellation"

        [Required]
        [Display(Name = "Cancelled")]
        public bool IsCancelled { get; set; }

        public UserType CancelledBy { get; set; }

        /// <summary>
        /// Who cancelled the appointment (pro id, or client id)
        /// </summary>
        public Guid CancelledById { get; set; }


        public DateTime CancelledTime { get; set; }

        #endregion

        /// <summary>
        /// NOTE: Never show this to a client. Make a new field for client notes.
        /// </summary>
        [StringLength(2000)]
        public string ProfessionalNotes { get; set; } = "";

        [StringLength(2000)]
        public string AuditNotes { get; set; } = "";


        public virtual Professional Professional { get; set; }

        

    }
}
