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
    public class Professional
    {
        /// <summary>
        /// The PK but non-clustered!
        /// </summary>
        public Guid ProfessionalId { get; set; }
        /// <summary>
        /// The clustered index to avoid db fragmentation.
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProfessionalIndex { get; set; }
        /// <summary>
        /// This is the user id from the auth server.
        /// </summary>
        public Guid ProfessionalUserId { get; set; }

        /// <summary>
        /// This is the title - mr, mrs etc.
        /// Make int for sqlbulkcopy
        /// </summary>
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

        /// <summary>
        /// Make int for sqlbulkcopy
        /// </summary>
        [Required]
        public Gender? Gender { get; set; }

        [Required]
        [Display(Name = "Email Address")]
        [StringLength(200, MinimumLength = 2)]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }

 
        [Display(Name = "Phone Number")]
        [RegularExpression(@"^(((\+44\s?\d{4}|\(?0\d{4}\)?)\s?\d{3}\s?\d{3})|((\+44\s?\d{3}|\(?0\d{3}\)?)\s?\d{3}\s?\d{4})|((\+44\s?\d{2}|\(?0\d{2}\)?)\s?\d{4}\s?\d{4}))(\s?\#(\d{4}|\d{3}))?$", ErrorMessage ="Please enter a valid phone number"), StringLength(12)]
        public string Telephone { get; set; } = "";

        [Required]
        [Display(Name = "Mobile Number")]
        [RegularExpression(@"^(((\+44\s?\d{4}|\(?0\d{4}\)?)\s?\d{3}\s?\d{3})|((\+44\s?\d{3}|\(?0\d{3}\)?)\s?\d{3}\s?\d{4})|((\+44\s?\d{2}|\(?0\d{2}\)?)\s?\d{4}\s?\d{4}))(\s?\#(\d{4}|\d{3}))?$", ErrorMessage ="Please enter a valid phone number"), StringLength(12)]
        public string TelephoneMobile { get; set; } = "";
        /// <summary>
        /// If they are just the companies admin, they might not do appointments themselves.
        /// </summary>
        public bool IsAvailableForAppointments { get; set; } = true;

        // ADMIN
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;
        public bool IsApproved { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public DateTime? BannedDate { get; set; }

        [StringLength(500)]
        public string BannedReason { get; set; } = "";

        [StringLength(1000)]
        public string Notes { get; set; } = "";

        public bool IsDeleted { get; set; }

        // Companies has a list of professionals.

        public virtual ICollection<ProfessionalWorkingHour> ProfessionalWorkingHours { get; set; }



        public string GetFullName()
        {
            string name = "";
            if (this.Honorific != null)
                name = name + this.Honorific.Value + " ";
            name = name + this.Forename + " ";
            name = name + this.Surname;

            if(!String.IsNullOrWhiteSpace(this.Suffix))
            {
                name = name + " " + this.Suffix;
            }

            return name;
        }
    }
}
