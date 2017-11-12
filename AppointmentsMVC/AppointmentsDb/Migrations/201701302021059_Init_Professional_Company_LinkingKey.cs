namespace AppointmentsDb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init_Professional_Company_LinkingKey : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Companies",
                c => new
                    {
                        CompanyId = c.Guid(nullable: false),
                        CompanyIndex = c.Int(nullable: false, identity: true),
                        CompanyName = c.String(nullable: false, maxLength: 200),
                        AddressLine1 = c.String(nullable: false, maxLength: 200),
                        AddressLine2 = c.String(maxLength: 200),
                        TownCity = c.String(nullable: false, maxLength: 200),
                        County = c.String(maxLength: 200),
                        Postcode = c.String(nullable: false),
                        MainContactName = c.String(nullable: false, maxLength: 200),
                        MainContactEmail = c.String(nullable: false, maxLength: 200),
                        MainContactTel = c.String(nullable: false, maxLength: 12),
                        SecondaryContactName = c.String(maxLength: 200),
                        SecondaryContactEmail = c.String(maxLength: 200),
                        SecondaryContactTel = c.String(maxLength: 12),
                        IsApproved = c.Boolean(nullable: false),
                        ApprovedDate = c.DateTime(),
                        BannedDate = c.DateTime(),
                        BannedReason = c.String(),
                        Notes = c.String(maxLength: 2000),
                        ApiLiveKey = c.String(),
                        ApiTestKey = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        Owner_ProfessionalId = c.Guid(),
                    })
                .PrimaryKey(t => t.CompanyId)
                .ForeignKey("dbo.Professionals", t => t.Owner_ProfessionalId)
                .Index(t => t.Owner_ProfessionalId);
            
            CreateTable(
                "dbo.Professionals",
                c => new
                    {
                        ProfessionalId = c.Guid(nullable: false),
                        ProfessionalIndex = c.Int(nullable: false, identity: true),
                        ProfessionalUserId = c.Guid(nullable: false),
                        Honorific = c.Int(nullable: false),
                        Forename = c.String(nullable: false, maxLength: 200),
                        MiddleName = c.String(maxLength: 200),
                        Surname = c.String(nullable: false, maxLength: 200),
                        Suffix = c.String(maxLength: 200),
                        Gender = c.Int(nullable: false),
                        EmailAddress = c.String(nullable: false, maxLength: 200),
                        Telephone = c.String(nullable: false, maxLength: 12),
                        TelephoneMobile = c.String(nullable: false, maxLength: 12),
                        IsAvailableForAppointments = c.Boolean(nullable: false),
                        CreatedDateTime = c.DateTime(nullable: false),
                        IsApproved = c.Boolean(nullable: false),
                        ApprovalDate = c.DateTime(),
                        BannedDate = c.DateTime(),
                        BannedReason = c.String(maxLength: 500),
                        Notes = c.String(maxLength: 1000),
                        IsDeleted = c.Boolean(nullable: false),
                        Company_CompanyId = c.Guid(),
                    })
                .PrimaryKey(t => t.ProfessionalId)
                .ForeignKey("dbo.Companies", t => t.Company_CompanyId)
                .Index(t => t.Company_CompanyId);
            
            CreateTable(
                "dbo.LinkingKeys",
                c => new
                    {
                        LinkingKeyId = c.Int(nullable: false, identity: true),
                        CreatedDateTime = c.DateTime(nullable: false),
                        AvailableForEmailAddress = c.String(maxLength: 200),
                        FromTableName = c.String(maxLength: 200),
                        FromTablePK = c.String(maxLength: 200),
                        ToTableName = c.String(maxLength: 200),
                        ToTablePK = c.String(maxLength: 200),
                        SpecialKey = c.String(nullable: false, maxLength: 2000),
                        ExpiryDateTime = c.DateTime(nullable: false),
                        UsedDateTime = c.DateTime(),
                        UsedByProfessionalId = c.Guid(nullable: false),
                        Owner_ProfessionalId = c.Guid(),
                    })
                .PrimaryKey(t => t.LinkingKeyId)
                .ForeignKey("dbo.Professionals", t => t.Owner_ProfessionalId)
                .Index(t => t.Owner_ProfessionalId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LinkingKeys", "Owner_ProfessionalId", "dbo.Professionals");
            DropForeignKey("dbo.Professionals", "Company_CompanyId", "dbo.Companies");
            DropForeignKey("dbo.Companies", "Owner_ProfessionalId", "dbo.Professionals");
            DropIndex("dbo.LinkingKeys", new[] { "Owner_ProfessionalId" });
            DropIndex("dbo.Professionals", new[] { "Company_CompanyId" });
            DropIndex("dbo.Companies", new[] { "Owner_ProfessionalId" });
            DropTable("dbo.LinkingKeys");
            DropTable("dbo.Professionals");
            DropTable("dbo.Companies");
        }
    }
}
