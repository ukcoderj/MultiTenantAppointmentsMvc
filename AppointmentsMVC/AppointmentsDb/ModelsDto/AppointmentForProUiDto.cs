using Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Helpers;

namespace AppointmentsDb.ModelsDto
{
    /// <summary>
    /// Appointments as shown in the UI.
    /// Remember FillAdditionalUiProperties() and UpdateStartTimeFromAdditionalUiProperties() to get the dates right.   
    /// </summary>
    public class AppointmentForProUiDto
    {
        public AppointmentForProUiDto()
        {

        }

        public AppointmentForProUiDto(DateTime startTime)
        {
            this.StartTime = startTime;
            this.FillAdditionalUiProperties();
        }

        public Guid AppointmentId { get; set; }

        public int AppointmentIndex { get; set; }

        /// <summary>
        /// Fill this in. It will not be found in the automap
        /// </summary>
        public Guid ProfessionalId { get; set; }


        [Required]
        [Display(Name = "Type")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select an appointment type")]
        public AppointmentType AppointmentType { get; set; }

        public string AppointmentTypeAsDisplay
        {
            get
            {
                return this.AppointmentType.ToDisplayString();
            }
        }


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
        [Display(Name = "Mobile")]
        [RegularExpression(@"^(((\+44\s?\d{4}|\(?0\d{4}\)?)\s?\d{3}\s?\d{3})|((\+44\s?\d{3}|\(?0\d{3}\)?)\s?\d{3}\s?\d{4})|((\+44\s?\d{2}|\(?0\d{2}\)?)\s?\d{4}\s?\d{4}))(\s?\#(\d{4}|\d{3}))?$", ErrorMessage = "Please enter a valid phone number"), StringLength(12)]
        public string TelephoneMobile { get; set; } = "";


        #region "Appointment Time"

        [Required]
        [Display(Name = "Start Time")]
        public DateTime StartTime { get; set; }

        [Required]
        [Display(Name = "Duration")]
        public int DurationInMinutes { get; set; }

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
        /// NOTE: Never show this to a client. This is for the pro to make a note on the appt.
        /// </summary>
        [StringLength(2000)]
        public string ProfessionalNotes { get; set; } = "";



        #region "Additional UI Properties"

        public void FillAdditionalUiProperties()
        {
            this.StartTime_Date = StartTime.Date;
            this.StartTime_Hour = StartTime.Hour;
            this.StartTime_Minutes = StartTime.Minute;
        }

        public void UpdateTimesFromAdditionalUiProperties()
        {
            this.StartTime = this.StartTime_Date.Date.AddHours(this.StartTime_Hour).AddMinutes(this.StartTime_Minutes);
            this.EndTime = this.StartTime.AddMinutes(this.DurationInMinutes);
        }


        [Display(Name = "Start Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime StartTime_Date { get; set; }

        [Display(Name = "Start Hour")]
        [Range(0, 24, ErrorMessage = "Please pick a valid hour in the day.")]
        public int StartTime_Hour { get; set; }

        [Display(Name = "Start Minute")]
        [Range(0, 60, ErrorMessage = "Please pick a valid minute.")]
        public int StartTime_Minutes { get; set; }

        #endregion  


    }
}
