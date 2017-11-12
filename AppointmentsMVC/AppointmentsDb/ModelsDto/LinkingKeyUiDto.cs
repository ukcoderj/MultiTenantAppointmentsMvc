using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentsDb.ModelsDto
{
    public class LinkingKeyUiDto
    {
        public int LinkingKeyId { get; set; }

        [Required]
        [Display(Name = "Date Created")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedDateTime { get; set; }

        /// <summary>
        /// If filled, this is the only person that can use the code (specialkey).
        /// </summary>
        [Display(Name = "For Email")]
        [StringLength(200, MinimumLength = 2)]
        public string AvailableForEmailAddress { get; set; }

        /// <summary>
        /// The table you are going to link from
        /// e.g. Professional. The side the user might see.
        /// </summary>
        [StringLength(200, MinimumLength = 2)]
        public string FromTableName { get; set; }

        [StringLength(200)]
        public string FromTablePK { get; set; }

        /// <summary>
        /// The table you are going to link to
        /// e.g. Company. The side the user will want to get to.
        /// </summary>
        [StringLength(200, MinimumLength = 2)]
        public string ToTableName { get; set; }

        [StringLength(200)]
        public string ToTablePK { get; set; }

        [Required]
        [Display(Name = "Key")]
        [StringLength(2000, MinimumLength = 20)]
        public string SpecialKey { get; set; }

        [Required]
        [Display(Name = "Expiry")]
        [DataType(DataType.DateTime)]
        public DateTime? ExpiryDateTime { get; set; }

        [Display(Name = "Used Date")]
        [DataType(DataType.DateTime)]
        public DateTime UsedDateTime { get; set; }

        /// <summary>
        /// Not linked to professionals. We can link in our own queries if needed.
        /// </summary>
        [Required]
        public Guid UsedByProfessionalId { get; set; }

    }
}
