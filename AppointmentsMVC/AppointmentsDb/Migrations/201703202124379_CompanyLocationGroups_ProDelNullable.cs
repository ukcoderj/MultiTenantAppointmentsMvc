namespace AppointmentsDb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CompanyLocationGroups_ProDelNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CompanyLocationGroups", "DeletedByProfessionalId", c => c.Guid());
            AlterColumn("dbo.CompanyLocations", "DeletedByProfessionalId", c => c.Guid());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CompanyLocations", "DeletedByProfessionalId", c => c.Guid(nullable: false));
            AlterColumn("dbo.CompanyLocationGroups", "DeletedByProfessionalId", c => c.Guid(nullable: false));
        }
    }
}
