using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentsDb.ModelsDto
{
    public class CompanyUiDto
    {
        public Guid CompanyId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CompanyIndex { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 5)]
        public string CompanyName { get; set; }

        [Required]
        [Display(Name = "Address Line 1")]
        [StringLength(200, MinimumLength = 2)]
        public string AddressLine1 { get; set; }

        [Display(Name = "Address Line 2")]
        [StringLength(200, MinimumLength = 2)]
        public string AddressLine2 { get; set; }

        [Required]
        [Display(Name = "Town/City")]
        [StringLength(200, MinimumLength = 2)]
        public string TownCity { get; set; }

        [StringLength(200, MinimumLength = 2)]
        public string County { get; set; }

        [Required]
        [DataType(DataType.PostalCode)]
        public string Postcode { get; set; }


        [Required]
        [Display(Name = "Main Contact Name")]
        [StringLength(200, MinimumLength = 2)]
        public string MainContactName { get; set; }

        [Required]
        [Display(Name = "Main Email")]
        [StringLength(200, MinimumLength = 2)]
        [DataType(DataType.EmailAddress)]
        public string MainContactEmail { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        [RegularExpression(@"^(((\+44\s?\d{4}|\(?0\d{4}\)?)\s?\d{3}\s?\d{3})|((\+44\s?\d{3}|\(?0\d{3}\)?)\s?\d{3}\s?\d{4})|((\+44\s?\d{2}|\(?0\d{2}\)?)\s?\d{4}\s?\d{4}))(\s?\#(\d{4}|\d{3}))?$", ErrorMessage = "Please enter a valid phone number"), StringLength(12)]
        public string MainContactTel { get; set; }


        [Display(Name = "Secondary Contact Name")]
        [StringLength(200)]
        [DataType(DataType.EmailAddress)]
        public string SecondaryContactName { get; set; } = "";

        [Display(Name = "Secondary Email")]
        [StringLength(200)]
        [DataType(DataType.EmailAddress)]
        public string SecondaryContactEmail { get; set; } = "";

        [Display(Name = "Secondary Phone Number")]
        [RegularExpression(@"^(((\+44\s?\d{4}|\(?0\d{4}\)?)\s?\d{3}\s?\d{3})|((\+44\s?\d{3}|\(?0\d{3}\)?)\s?\d{3}\s?\d{4})|((\+44\s?\d{2}|\(?0\d{2}\)?)\s?\d{4}\s?\d{4}))(\s?\#(\d{4}|\d{3}))?$", ErrorMessage = "Please enter a valid phone number"), StringLength(12)]
        public string SecondaryContactTel { get; set; } = "";


        /// <summary>
        /// DO NOT TRUST THIS. ONLY Use for Formatting UI on load/ server side functions.
        /// Never trust this on data coming back.
        /// </summary>
        public bool UILoadOnly_IsUserCompanyOwner { get; set; }
    }
}
