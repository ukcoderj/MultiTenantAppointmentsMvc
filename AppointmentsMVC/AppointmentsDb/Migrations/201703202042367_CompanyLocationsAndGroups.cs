namespace AppointmentsDb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CompanyLocationsAndGroups : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CompanyLocationGroups",
                c => new
                    {
                        CompanyLocationGroupId = c.Guid(nullable: false),
                        CompanyLocationGroupIndex = c.Int(nullable: false, identity: true),
                        LocationGroupName = c.String(maxLength: 200),
                        CreatedDate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletedByProfessionalId = c.Guid(nullable: false),
                        DeletedDate = c.DateTime(),
                        HasProfessionalVisitsClientLocations = c.Boolean(nullable: false),
                        Company_CompanyId = c.Guid(),
                        CreatedByProfessional_ProfessionalId = c.Guid(),
                    })
                .PrimaryKey(t => t.CompanyLocationGroupId)
                .ForeignKey("dbo.Companies", t => t.Company_CompanyId)
                .ForeignKey("dbo.Professionals", t => t.CreatedByProfessional_ProfessionalId)
                .Index(t => t.Company_CompanyId)
                .Index(t => t.CreatedByProfessional_ProfessionalId);
            
            CreateTable(
                "dbo.CompanyLocations",
                c => new
                    {
                        CompanyLocationId = c.Guid(nullable: false),
                        CompanyLocationIndex = c.Int(nullable: false, identity: true),
                        IsProfessionalVisitToClientLocation = c.Boolean(nullable: false),
                        Postcode = c.String(nullable: false),
                        IsPostCodeAreaAndDistrictOnly = c.Boolean(nullable: false),
                        IsPostCodeAreaDistrictSectorOnly = c.Boolean(nullable: false),
                        AddressLine1 = c.String(),
                        AddressLine2 = c.String(),
                        TownCity = c.String(),
                        County = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletedByProfessionalId = c.Guid(nullable: false),
                        DeletedDate = c.DateTime(),
                        CompanyLocationGroup_CompanyLocationGroupId = c.Guid(),
                    })
                .PrimaryKey(t => t.CompanyLocationId)
                .ForeignKey("dbo.CompanyLocationGroups", t => t.CompanyLocationGroup_CompanyLocationGroupId)
                .Index(t => t.CompanyLocationGroup_CompanyLocationGroupId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CompanyLocationGroups", "CreatedByProfessional_ProfessionalId", "dbo.Professionals");
            DropForeignKey("dbo.CompanyLocations", "CompanyLocationGroup_CompanyLocationGroupId", "dbo.CompanyLocationGroups");
            DropForeignKey("dbo.CompanyLocationGroups", "Company_CompanyId", "dbo.Companies");
            DropIndex("dbo.CompanyLocations", new[] { "CompanyLocationGroup_CompanyLocationGroupId" });
            DropIndex("dbo.CompanyLocationGroups", new[] { "CreatedByProfessional_ProfessionalId" });
            DropIndex("dbo.CompanyLocationGroups", new[] { "Company_CompanyId" });
            DropTable("dbo.CompanyLocations");
            DropTable("dbo.CompanyLocationGroups");
        }
    }
}
