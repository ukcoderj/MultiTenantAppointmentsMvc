namespace AppointmentsDb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProCo_Property_Updates : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Professionals", "Telephone", c => c.String(maxLength: 12));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Professionals", "Telephone", c => c.String(nullable: false, maxLength: 12));
        }
    }
}
