namespace AppointmentsDb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Appointments_3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Appointments", "Telephone", c => c.String(maxLength: 12));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Appointments", "Telephone", c => c.String(nullable: false, maxLength: 12));
        }
    }
}
