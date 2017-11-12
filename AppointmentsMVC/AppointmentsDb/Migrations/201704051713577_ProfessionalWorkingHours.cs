namespace AppointmentsDb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProfessionalWorkingHours : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProfessionalWorkingHours",
                c => new
                    {
                        ProfessionalWorkingHourId = c.Guid(nullable: false),
                        ProfessionalWorkingHourIndex = c.Int(nullable: false, identity: true),
                        DayOfWeek = c.Int(nullable: false),
                        StartTime = c.Time(nullable: false, precision: 7),
                        EndTime = c.Time(nullable: false, precision: 7),
                        IncludeBankHolidays = c.Boolean(nullable: false),
                        CreatedByProfessionalId = c.Guid(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedByProfessionalId = c.Guid(),
                        UpdatedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletedByProfessionalId = c.Guid(),
                        DeletedDate = c.DateTime(),
                        CompanyLocationGroup_CompanyLocationGroupId = c.Guid(nullable: false),
                        Professional_ProfessionalId = c.Guid(),
                    })
                .PrimaryKey(t => t.ProfessionalWorkingHourId)
                .ForeignKey("dbo.CompanyLocationGroups", t => t.CompanyLocationGroup_CompanyLocationGroupId, cascadeDelete: true)
                .ForeignKey("dbo.Professionals", t => t.Professional_ProfessionalId)
                .Index(t => t.CompanyLocationGroup_CompanyLocationGroupId)
                .Index(t => t.Professional_ProfessionalId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProfessionalWorkingHours", "Professional_ProfessionalId", "dbo.Professionals");
            DropForeignKey("dbo.ProfessionalWorkingHours", "CompanyLocationGroup_CompanyLocationGroupId", "dbo.CompanyLocationGroups");
            DropIndex("dbo.ProfessionalWorkingHours", new[] { "Professional_ProfessionalId" });
            DropIndex("dbo.ProfessionalWorkingHours", new[] { "CompanyLocationGroup_CompanyLocationGroupId" });
            DropTable("dbo.ProfessionalWorkingHours");
        }
    }
}
