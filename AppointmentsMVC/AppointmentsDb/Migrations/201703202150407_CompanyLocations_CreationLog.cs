namespace AppointmentsDb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CompanyLocations_CreationLog : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CompanyLocations", "CreatedByProfessionalId", c => c.Guid(nullable: false));
            AddColumn("dbo.CompanyLocations", "CreatedDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CompanyLocations", "CreatedDate");
            DropColumn("dbo.CompanyLocations", "CreatedByProfessionalId");
        }
    }
}
