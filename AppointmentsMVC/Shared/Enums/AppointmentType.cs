using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Enums
{
    /// <summary>
    /// The type of appointment this is.
    /// </summary>
    public enum AppointmentType
    {
        // REMEMBER TO ADD HEX --> format http://stackoverflow.com/a/25140769/852806

        // TBD is a get out value, aka unknown
        [Display(Name = "TBD")]
        TBD = 0x1,
        [Display(Name = "Appointment Type 1 e.g. Mens Hair Cut")]
        AppointmentType1 = 0x2,
        [Display(Name = "Appointment Type 2 e.g. Cut and dry")]
        AppointmentType2 = 0x4,
        [Display(Name = "Appointment Type 3 ...")]
        AppointmentType3 = 0x8,
        [Display(Name = "Appointment Type 4 ...")]
        AppointmentType4 = 0x10,
    }
}
