namespace Testing.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ProfessionalLocation
    {
        [Key]
        [Column(Order = 0)]
        public Guid ProfessionalId { get; set; }

        [Key]
        [Column(Order = 1)]
        public Guid LocationId { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public virtual Location Location { get; set; }

        public virtual Professional Professional { get; set; }
    }
}
