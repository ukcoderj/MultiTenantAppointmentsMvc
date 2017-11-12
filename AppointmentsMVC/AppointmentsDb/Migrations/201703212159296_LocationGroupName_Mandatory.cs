namespace AppointmentsDb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LocationGroupName_Mandatory : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CompanyLocationGroups", "LocationGroupName", c => c.String(nullable: false, maxLength: 200));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CompanyLocationGroups", "LocationGroupName", c => c.String(maxLength: 200));
        }
    }
}
