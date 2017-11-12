namespace Testing.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AvailableAppointment
    {
        public Guid AvailableAppointmentId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AvailableAppointmentIndex { get; set; }

        public DateTime EndTime { get; set; }

        public bool IsDeleted { get; set; }

        public Guid? LocationId { get; set; }

        public Guid? ProfessionalId { get; set; }

        public Guid SetByAppOrCompanyId { get; set; }

        public DateTime StartTime { get; set; }

        public virtual Location Location { get; set; }

        public virtual Professional Professional { get; set; }
    }
}
