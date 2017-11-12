using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentsDb.Models
{
    public class ProfessionalWorkingHour : CrudState
    {
        /// <summary>
        /// The PK but non-clustered!
        /// </summary>
        public Guid ProfessionalWorkingHourId { get; set; }
        /// <summary>
        /// The clustered index to avoid db fragmentation.
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProfessionalWorkingHourIndex { get; set; }

        [Required]
        [Display(Name ="Day")]        
        public DayOfWeek DayOfWeek { get; set; }

        [Display(Name = "Start Time")]
        [Required]
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = @"{0:hh\:mm}")]
        public TimeSpan StartTime { get; set; }

        [Display(Name = "End Time")]
        [Required]
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = @"{0:hh\:mm}")]
        public TimeSpan EndTime { get; set; }

        /// <summary>
        /// NOTE: This includes christmas, boxing day and new years day
        ///       If the professional has this checked (true) then 
        ///       they must book a holiday for this not to be shown as available.
        /// </summary>
        [Required]
        [Display(Name = "Bank Hols (inc Xmas/NY)")]
        public bool IncludeBankHolidays { get; set; }

        [Required]
        public CompanyLocationGroup CompanyLocationGroup { get; set; }

        // Pro linking done from the pro side.

        // Deletion info in CrudState

        // Always mark as deleted rather than properly deleting.

    }
}
