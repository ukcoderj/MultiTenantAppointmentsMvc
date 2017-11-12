namespace AppointmentsDb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CompanyLocationGroups_UpdateInfo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CompanyLocations", "UpdatedByProfessionalId", c => c.Guid(nullable: false));
            AddColumn("dbo.CompanyLocations", "UpdatedDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CompanyLocations", "UpdatedDate");
            DropColumn("dbo.CompanyLocations", "UpdatedByProfessionalId");
        }
    }
}
