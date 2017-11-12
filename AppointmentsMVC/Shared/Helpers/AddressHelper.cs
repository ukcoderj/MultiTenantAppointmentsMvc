using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Shared.Helpers
{
    public class AddressHelper
    {
        static public bool IsValidPostCode(string postcode)
        {
            // This was an answer on StackOverflow for all UK postcodes.
            var googledMatch = (
                Regex.IsMatch(postcode, "(^[A-PR-UWYZa-pr-uwyz][0-9][ ]*[0-9][ABD-HJLNP-UW-Zabd-hjlnp-uw-z]{2}$)") ||
                Regex.IsMatch(postcode, "(^[A-PR-UWYZa-pr-uwyz][0-9][0-9][ ]*[0-9][ABD-HJLNP-UW-Zabd-hjlnp-uw-z]{2}$)") ||
                Regex.IsMatch(postcode, "(^[A-PR-UWYZa-pr-uwyz][A-HK-Ya-hk-y][0-9][ ]*[0-9][ABD-HJLNP-UW-Zabd-hjlnp-uw-z]{2}$)") ||
                Regex.IsMatch(postcode, "(^[A-PR-UWYZa-pr-uwyz][A-HK-Ya-hk-y][0-9][0-9][ ]*[0-9][ABD-HJLNP-UW-Zabd-hjlnp-uw-z]{2}$)") ||
                Regex.IsMatch(postcode, "(^[A-PR-UWYZa-pr-uwyz][0-9][A-HJKS-UWa-hjks-uw][ ]*[0-9][ABD-HJLNP-UW-Zabd-hjlnp-uw-z]{2}$)") ||
                Regex.IsMatch(postcode, "(^[A-PR-UWYZa-pr-uwyz][A-HK-Ya-hk-y][0-9][A-Za-z][ ]*[0-9][ABD-HJLNP-UW-Zabd-hjlnp-uw-z]{2}$)") ||
                Regex.IsMatch(postcode, "(^[Gg][Ii][Rr][]*0[Aa][Aa]$)")
                );

            // Back up with a simple version of our own. It's a bit loosey goosey.
            var ownRegexMatch = Regex.IsMatch(postcode, "^[A-z]{1,2}[1-9][ ]?[1-9][A-z]{2}$");

            return googledMatch || ownRegexMatch;
        }


        /// <summary>
        /// DESIGN DECISION. THE MOBILE POSTCODE MUST INCLUDE THE DISTRICT TO AVOID TYPOS CREATING LONG JOURNEYS.
        /// Good -> E16, PE12, PE12 6, E16 1, E16 1XL
        /// </summary>
        /// <param name="postcode"></param>
        /// <returns></returns>
        public static bool IsValidMobilePostcode(string postcode)
        {
            var isPcAreaDistrict = IsPostCodeAreaAndDistrict(postcode);
            var isPcAreaDistrictSector = IsPostCodeAreaDistrictSector(postcode);
            var isValidPostCode = IsValidPostCode(postcode);
            var returnValue = isPcAreaDistrict || isPcAreaDistrictSector || isValidPostCode;
            return returnValue;
        }

        /// <summary>
        /// The postcode district
        /// e.g. B1, or BT14
        /// </summary>
        /// <param name="postcode"></param>
        /// <returns></returns>
        public static bool IsPostCodeAreaAndDistrict(string postcode)
        {
            if(String.IsNullOrWhiteSpace(postcode))
            {
                return false;
            }

            // DO NOT remove the $ from the end of the regex!
            return Regex.IsMatch(postcode, "[A-z]{1,2}[1-9]{1,2}[ ]?$");
        }

        /// <summary>
        /// The postcode district and sector
        /// e.g. B1 5, or BT14 6
        /// </summary>
        /// <param name="postcode"></param>
        /// <returns></returns>
        public static bool IsPostCodeAreaDistrictSector(string postcode)
        {
            if (String.IsNullOrWhiteSpace(postcode))
            {
                return false;
            }

            // DO NOT remove the $ from the end of the regex!
            return Regex.IsMatch(postcode, "[A-z]{1,2}[1-9]{1,2}[ ]?[1-9]$");
        }




    }
}
