using Shared.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace AppointmentsDb.ModelsDto
{
    public class ProfessionalUiDto
    {
        public Guid ProfessionalId { get; set; }

        public int ProfessionalIndex { get; set; }

        public Guid ProfessionalUserId { get; set; }

        [Required]
        public Honorific? Honorific { get; set; }
        [Required]
        [StringLength(200, MinimumLength = 2)]
        public string Forename { get; set; }

        [StringLength(200)]
        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; } = "";

        [Required]
        [StringLength(200, MinimumLength = 2)]
        public string Surname { get; set; }

        [StringLength(200)]
        public string Suffix { get; set; } = "";

        [Required]
        public Gender? Gender { get; set; }

        [Required]
        [Display(Name = "Email Address")]
        [StringLength(200, MinimumLength = 3)]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }

        [Display(Name = "Phone Number")]
        [RegularExpression(@"^(((\+44\s?\d{4}|\(?0\d{4}\)?)\s?\d{3}\s?\d{3})|((\+44\s?\d{3}|\(?0\d{3}\)?)\s?\d{3}\s?\d{4})|((\+44\s?\d{2}|\(?0\d{2}\)?)\s?\d{4}\s?\d{4}))(\s?\#(\d{4}|\d{3}))?$", ErrorMessage = "Please enter a valid phone number"), Required, StringLength(12)]
        public string Telephone { get; set; } = "";

        [Display(Name = "Mobile Number")]
        [RegularExpression(@"^(((\+44\s?\d{4}|\(?0\d{4}\)?)\s?\d{3}\s?\d{3})|((\+44\s?\d{3}|\(?0\d{3}\)?)\s?\d{3}\s?\d{4})|((\+44\s?\d{2}|\(?0\d{2}\)?)\s?\d{4}\s?\d{4}))(\s?\#(\d{4}|\d{3}))?$", ErrorMessage = "Please enter a valid phone number"), Required, StringLength(12)]
        public string TelephoneMobile { get; set; } = "";
        /// <summary>
        /// If they are just the companies admin, they might not do appointments themselves.
        /// </summary>
        [Display(Name = "I do Appointments")]
        public bool IsAvailableForAppointments { get; set; } = true;

        // DONT ALLOW THEM TO CHANGE THEIR APPROVAL/BANNED STATUS!
    }
}
