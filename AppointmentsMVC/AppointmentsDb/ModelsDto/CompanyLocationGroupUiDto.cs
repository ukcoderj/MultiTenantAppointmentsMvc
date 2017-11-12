using Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentsDb.ModelsDto
{
    public class CompanyLocationGroupUiDto : AuthorizationStateBase
    {
        public Guid CompanyLocationGroupId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CompanyLocationGroupIndex { get; set; }

        [Display(Name ="Group Name")]
        [Required]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "The locations' name must be > 5 letters.")]
        public string LocationGroupName { get; set; }

        // Company, professional, creation pro + delete log not included
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Deleted")]
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Has locations where the professional will visit the client,
        /// rather than a client coming to a clinic/room.
        /// </summary>
        [Display(Name = "Inc's Pro Visiting Client")]
        public bool HasProfessionalVisitsClientLocations { get; set; }

        /// <summary>
        /// This is a one-to-many relationship.
        /// A group can have 1 or more locations associated with it.
        /// </summary>
        public List<CompanyLocationUiDto> CompanyLocationUiDtos { get; set; }

    }
}
