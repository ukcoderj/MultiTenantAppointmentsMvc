namespace AppointmentsDb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Appointments_2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Appointments",
                c => new
                    {
                        AppointmentId = c.Guid(nullable: false),
                        AppointmentIndex = c.Int(nullable: false, identity: true),
                        Forename = c.String(nullable: false, maxLength: 200),
                        Surname = c.String(nullable: false, maxLength: 200),
                        EmailAddress = c.String(maxLength: 200),
                        Telephone = c.String(nullable: false, maxLength: 12),
                        TelephoneMobile = c.String(nullable: false, maxLength: 12),
                        StartTime = c.DateTime(nullable: false),
                        DurationInMinutes = c.Int(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        CreatedById = c.Guid(nullable: false),
                        CreatedTime = c.DateTime(nullable: false),
                        IsConfirmed = c.Boolean(nullable: false),
                        IsCancelled = c.Boolean(nullable: false),
                        CancelledBy = c.Int(nullable: false),
                        CancelledById = c.Guid(nullable: false),
                        CancelledTime = c.DateTime(nullable: false),
                        ProfessionalNotes = c.String(maxLength: 2000),
                        AuditNotes = c.String(maxLength: 2000),
                        Professional_ProfessionalId = c.Guid(),
                    })
                .PrimaryKey(t => t.AppointmentId)
                .ForeignKey("dbo.Professionals", t => t.Professional_ProfessionalId)
                .Index(t => t.Professional_ProfessionalId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Appointments", "Professional_ProfessionalId", "dbo.Professionals");
            DropIndex("dbo.Appointments", new[] { "Professional_ProfessionalId" });
            DropTable("dbo.Appointments");
        }
    }
}
