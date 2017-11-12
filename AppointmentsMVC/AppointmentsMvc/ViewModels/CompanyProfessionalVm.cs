using AppointmentsDb.ModelsDto;
using AppointmentsDb.Pattern;
using AppointmentsDb.Queries;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppointmentsMvc.ViewModels
{
    public class CompanyProfessionalVm
    {
        public ProfessionalUiDto Professional;
        public CompanyUiDto Company;


        IProfessionalQueries _professionalQueries;
        ICompanyQueries _companyQueries;


        public CompanyProfessionalVm(IProfessionalQueries professionalQueries, ICompanyQueries companyQueries, string userId)
        {
            _professionalQueries = professionalQueries;
            _companyQueries = companyQueries;

            Professional = _professionalQueries.GetUiDto_ProfessionalFromUserGuid(userId);
            Company = _companyQueries.GetUiDto_CompanyFromUserGuid(userId);
        }

        public Guid? GetProfessionalId()
        {
            if (Professional != null)
                return Professional.ProfessionalId;
            else
                return (Guid?)null;
        }

        public bool IsProfessionalValid()
        {
            if (Professional == null ||
                (Professional != null &&
                (String.IsNullOrWhiteSpace(Professional.Forename) ||
                String.IsNullOrWhiteSpace(Professional.EmailAddress) ||
                String.IsNullOrWhiteSpace(Professional.Surname))))
            {
                return false;
            }
            return true;
        }

        public bool IsCompanyInfoValid()
        {
            if (Company == null ||
                (Company != null &&
                (String.IsNullOrWhiteSpace(Company.CompanyName) ||
                String.IsNullOrWhiteSpace(Company.MainContactEmail) ||
                String.IsNullOrWhiteSpace(Company.MainContactTel))))
            {
                return false;
            }
            return true;
        }
    }
}