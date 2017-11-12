using Shared.Enums;
using Shared.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentsDb.ModelsDto
{
    public class CompanyLocationUiDto : AuthorizationStateBase
    {
        public Guid CompanyLocationId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CompanyLocationIndex { get; set; }

        /// <summary>
        /// Is this an address where the professional will visit the client (at a place of the clients choosing)
        /// NOTE: The more details that are added, the more restrictive it will be.
        ///       So the 'Plaza Hotel' could be a static location, or a client visit location if ticked.
        /// </summary>
        [Display(Name ="Visiting Clients")]
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


        // Creation user/deletion logs omitted.
        public DateTime CreatedDate { get; set; }


        public string Validate()
        {
            string returnValue = "";

            if (IsProfessionalVisitToClientLocation)
            {
                if (String.IsNullOrWhiteSpace(Postcode))
                {
                    returnValue = "Postcode required.";
                }
                else
                {
                    if (!AddressHelper.IsValidMobilePostcode(Postcode))
                    {
                        returnValue = "Postcode format invalid.";
                    }
                    else
                    {
                        IsPostCodeAreaAndDistrictOnly = AddressHelper.IsPostCodeAreaAndDistrict(Postcode);
                        IsPostCodeAreaDistrictSectorOnly = AddressHelper.IsPostCodeAreaDistrictSector(Postcode);
                    }
                }
            }
            else
            {
                if (String.IsNullOrWhiteSpace(Postcode))
                {
                    returnValue += "Postcode required. ";
                }
                if (!AddressHelper.IsValidPostCode(Postcode))
                {
                    returnValue += "A full postcode is required unless you're visiting clients (check 'Visiting Clients'). ";
                }

                if (String.IsNullOrWhiteSpace(AddressLine1))
                {
                    returnValue += "Address Line 1 required. ";
                }

                if (String.IsNullOrWhiteSpace(TownCity))
                {
                    returnValue += "Town/city required. ";
                }

                if (String.IsNullOrWhiteSpace(County))
                {
                    returnValue += "County required. ";
                }
            }

            return returnValue;
        }
    }
}
