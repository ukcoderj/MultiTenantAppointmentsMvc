using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentsDb.Models
{
    /// <summary>
    /// Audit information about the data
    /// Do not create a DTO for this as we want it kept away from the users.
    /// </summary>
    public class CrudState
    {
        public Guid CreatedByProfessionalId { get; set; }

        public DateTime CreatedDate { get; set; }


        public Guid? UpdatedByProfessionalId { get; set; }

        public DateTime? UpdatedDate { get; set; }


        [Display(Name = "Deleted")]
        public bool IsDeleted { get; set; }

        public Guid? DeletedByProfessionalId { get; set; }

        public DateTime? DeletedDate { get; set; }

        
    }
}
