namespace AppointmentsDb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AppointmentType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Appointments", "AppointmentType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Appointments", "AppointmentType");
        }
    }
}
