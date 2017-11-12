using Shared.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentsDb.Models
{
    public class CompanyLocation
    {
        public Guid CompanyLocationId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CompanyLocationIndex { get; set; }

        /// <summary>
        /// Is this an address where the professional will visit the client (at a place of the clients choosing)
        /// NOTE: The more details that are added, the more restrictive it will be.
        ///       So the 'Plaza Hotel' could be a static location, or a client visit location if ticked.
        /// </summary>
        [Display(Name = "Visiting Clients")]
        public bool IsProfessionalVisitToClientLocation { get; set; }

        [Required]
        public string Postcode { get; set; }


        /// <summary>
        /// IF it's a ProfessionalVisitToClient postcode, is it an area and district.e.g. "BS98" only
        /// </summary>
        public bool IsPostCodeAreaAndDistrictOnly { get; set; }

        /// <summary>
        /// IF it's a ProfessionalVisitToClient postcode, is it an area, district and sector .e.g. "BS98 1" only
        /// </summary>
        public bool IsPostCodeAreaDistrictSectorOnly { get; set; }


        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public string TownCity { get; set; }

        public string County { get; set; }


        [Display(Name = "Deleted")]
        public bool IsDeleted { get; set; }


        public Guid CreatedByProfessionalId { get; set; }

        public DateTime CreatedDate { get; set; }

        public Guid UpdatedByProfessionalId { get; set; }

        public DateTime UpdatedDate { get; set; }

        public Guid? DeletedByProfessionalId { get; set; }

        public DateTime? DeletedDate { get; set; }

    }
}
