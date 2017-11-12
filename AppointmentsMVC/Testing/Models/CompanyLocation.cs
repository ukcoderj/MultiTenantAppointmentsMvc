namespace Testing.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CompanyLocation
    {
        [Key]
        [Column(Order = 0)]
        public Guid CompanyId { get; set; }

        [Key]
        [Column(Order = 1)]
        public Guid LocationId { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public virtual Company Company { get; set; }

        public virtual Location Location { get; set; }
    }
}
