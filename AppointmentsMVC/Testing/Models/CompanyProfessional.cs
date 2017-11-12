namespace Testing.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CompanyProfessional")]
    public partial class CompanyProfessional
    {
        [Key]
        [Column(Order = 0)]
        public Guid ProfessionalId { get; set; }

        [Key]
        [Column(Order = 1)]
        public Guid CompanyId { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public virtual Company Company { get; set; }

        public virtual Professional Professional { get; set; }
    }
}
