using AppointmentsDb.MapperStart;
using AppointmentsDb.Models;
using AutoMapper;
using Microsoft.Samples.EntityDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace AppointmentsMvc
{
    public class MvcApplication : System.Web.HttpApplication
    {
        IMapper _mapper;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);


            _mapper = new Automapper_Startup().StartAutomapper();


            using (var dc = new AppointmentsDb.Models.AppointmentsDbContext())
            {
                var prof = dc.Professionals.FirstOrDefault();
                var company = dc.Companies.FirstOrDefault();
            }

                //// - BULK DATA TESTING 
                //// For Professional, you need to change enums to ints!
                //try
                //{
                //    List<Company> companyList = new List<Company>();
                //    List<Professional> professionalList = new List<Professional>();
                //    string con = WebConfigurationManager.ConnectionStrings["AppointmentsDbContext"].ConnectionString;

                //    using (var dc = new AppointmentsDb.Models.AppointmentsDbContext())
                //    {
                //        for (var i = 1; i < 1000000; i++)
                //        {
                //            Professional p = new Professional();
                //            p.ProfessionalId = Guid.NewGuid();
                //            p.ProfessionalUserId = Guid.NewGuid();
                //            p.Honorific = 0;
                //            p.Forename = "Forename-" + i;
                //            p.MiddleName = "rrr";
                //            p.Surname = "Surname-" + i;
                //            p.Suffix = "rrr";
                //            p.Gender = 0;
                //            p.EmailAddress = "test" + i + "@test.com";
                //            p.Telephone = "07987 777777";
                //            p.TelephoneMobile = "07987777777";
                //            p.IsAvailableForAppointments = true;
                //            p.CreatedDateTime = DateTime.Now;
                //            p.IsApproved = true;
                //            p.ApprovalDate = DateTime.Now;
                //            p.Notes = "rrr";
                //            p.IsDeleted = false;
                //            p.BannedReason = "rrr";

                //            professionalList.Add(p);

                //            //dc.Professionals.Add(p);

                //            Company c = new Company();
                //            c.CompanyId = Guid.NewGuid();
                //            c.Owner = p;
                //            c.CompanyName = "Company-" + i;
                //            c.AddressLine1 = "a1";
                //            c.AddressLine2 = "a2";
                //            c.TownCity = "c1";
                //            c.County = "c2";
                //            c.Postcode = "PO88nn";
                //            c.MainContactName = "Mr " + i;
                //            c.MainContactEmail = "test@test.com";
                //            c.MainContactTel = "07987777777";
                //            c.SecondaryContactName = "";
                //            c.SecondaryContactEmail = "";
                //            c.SecondaryContactTel = "";
                //            c.IsApproved = true;
                //            c.ApprovedDate = DateTime.Now;

                //            c.BannedReason = "";
                //            c.Notes = "";
                //            c.ApiLiveKey = "";
                //            c.ApiTestKey = "";
                //            c.IsDeleted = false;

                //            companyList.Add(c);





                //            // SQLBULKCOPY Does not maintain FK references, 
                //            // but is lightning quick!
                //            // May wish to mix the two.
                //            if (i % 5000 == 0)
                //            {
                //                var bulkCopy = new SqlBulkCopy(con);

                //                bulkCopy.BulkCopyTimeout = 120; //LongTimeout was actually causing more deadlock issues.
                //                bulkCopy.DestinationTableName = "Professionals";
                //                bulkCopy.WriteToServer(professionalList.AsDataReader());



                //                bulkCopy = new SqlBulkCopy(con);
                //                bulkCopy.BulkCopyTimeout = 120; //LongTimeout was actually causing more deadlock issues.
                //                bulkCopy.DestinationTableName = "Companies";
                //                bulkCopy.WriteToServer(companyList.AsDataReader());


                //                //dc.Companies.AddRange(companyList);
                //                //dc.SaveChanges();
                //                companyList = new List<Company>();
                //                professionalList = new List<Professional>();
                //            }

                //        }
                //    }
                //}
                //catch (DbEntityValidationException e)
                //{
                //    foreach (var eve in e.EntityValidationErrors)
                //    {
                //        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                //            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                //        foreach (var ve in eve.ValidationErrors)
                //        {
                //            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                //                ve.PropertyName, ve.ErrorMessage);
                //        }
                //    }
                //    throw;
                //}
                //catch (Exception ex)
                //{

                //}


        }
    }


}




//Professional p = new Professional();
//p.ProfessionalId = Guid.NewGuid();
//p.ProfessionalUserId = Guid.NewGuid();
//p.Honorific = Shared.Enums.Honorific.Mr;
//p.Forename = "Forename-" + i;
//p.MiddleName = "";
//p.Surname = "Surname-" + i;
//p.Suffix = "";
//p.Gender = Shared.Enums.Gender.Male;
//p.EmailAddress = "test" + i + "@test.com";
//p.Telephone = "07987 777777";
//p.TelephoneMobile = "07987777777";
//p.IsAvailableForAppointments = true;
//p.CreatedDateTime = DateTime.Now;
//p.IsApproved = true;
//p.ApprovalDate = DateTime.Now;
//p.Notes = "";
//p.IsDeleted = false;

//dc.Professionals.Add(p);

//Company c = new Company();
//c.CompanyId = Guid.NewGuid();
//                        c.Owner = new Professional()
//{
//    ProfessionalId = Guid.NewGuid(),
//                            ProfessionalUserId = Guid.NewGuid(),
//                            Honorific = Shared.Enums.Honorific.Mr,
//                            Forename = "Forename-" + i,
//                            MiddleName = "",
//                            Surname = "Surname-" + i,
//                            Suffix = "",
//                            Gender = Shared.Enums.Gender.Male,
//                            EmailAddress = "test" + i + "@test.com",
//                            Telephone = "07987 777777",
//                            TelephoneMobile = "07987777777",
//                            IsAvailableForAppointments = true,
//                            CreatedDateTime = DateTime.Now,
//                            IsApproved = true,
//                            ApprovalDate = DateTime.Now,
//                            Notes = "",
//                            IsDeleted = false,
//                        };
//c.CompanyName = "Company-" + i;
//                        c.AddressLine1 = "a1";
//                        c.AddressLine2 = "a2";
//                        c.TownCity = "c1";
//                        c.County = "c2";
//                        c.Postcode = "PO88nn";
//                        c.MainContactName = "Mr " + i;
//                        c.MainContactEmail = "test@test.com";
//                        c.MainContactTel = "07987777777";
//                        c.SecondaryContactName = "";
//                        c.SecondaryContactEmail = "";
//                        c.SecondaryContactTel = "";
//                        c.IsApproved = true;
//                        c.ApprovedDate = DateTime.Now;

//                        c.BannedReason = "";
//                        c.Notes = "";
//                        c.ApiLiveKey = "";
//                        c.ApiTestKey = "";
//                        c.IsDeleted = false;

//                        companyList.Add(c);