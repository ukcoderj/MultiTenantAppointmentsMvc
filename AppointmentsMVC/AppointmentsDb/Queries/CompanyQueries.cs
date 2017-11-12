using AppointmentsDb.Helpers;
using AppointmentsDb.Models;
using AppointmentsDb.ModelsDto;
using AppointmentsDb.ModelsDto.Custom;
using AppointmentsDb.Pattern;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentsDb.Queries
{
    public interface ICompanyQueries
    {
        CompanyUiDto GetUiDto_CompanyFromUserGuid(string userIdString);
        Company GetCompanyFromOwnerUserGuid(string userIdString);
        Company GetCompanyAndThisEmployeeFromEmployeeProfessionalUserId(string userIdString);
        Company GetCompanyAndAllEmployeesFromOwnerProfessionalUserId(Guid userId);
        void AddOrUpdateCompanyFromDto(string requestorUserId, CompanyUiDto companyDto);
        List<AccessibleEmployee> GetAccessibleEmployeesForProfessional(string requestorUserId, bool includeOwner, int ownerIndex);
    }

    public class CompanyQueries : ICompanyQueries
    {
        IMapper _mapper;
        IUnitOfWork _unitOfWork;

        public CompanyQueries(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        /// <summary>
        /// Get the company for the user, be they an employee or an owner
        /// </summary>
        /// <param name="userIdString"></param>
        /// <returns></returns>
        public CompanyUiDto GetUiDto_CompanyFromUserGuid(string userIdString)
        {
            Guid userId = GuidHelper.GetGuid(userIdString);
            CompanyUiDto returnValue = null;

            var companyFromOwnerUserId = GetCompanyFromOwnerUserGuid(userIdString);
            var companyFromEmployeeUserId = GetCompanyAndThisEmployeeFromEmployeeProfessionalUserId(userIdString);

            Company dbCompany = null;

            if (companyFromEmployeeUserId != null)
                dbCompany = companyFromEmployeeUserId;
            else if (companyFromOwnerUserId != null)
                dbCompany = companyFromOwnerUserId;

            if (dbCompany != null)
                returnValue = _mapper.Map<Company, CompanyUiDto>(dbCompany);

            if (companyFromOwnerUserId != null)
                returnValue.UILoadOnly_IsUserCompanyOwner = true;

            return returnValue;
        }

        /// <summary>
        /// Returns a company if the users owns it, else null.
        /// </summary>
        /// <param name="userIdString"></param>
        /// <returns></returns>
        public Company GetCompanyFromOwnerUserGuid(string userIdString)
        {
            Guid userId = GuidHelper.GetGuid(userIdString);
            var company = _unitOfWork.CompaniesRepository.Get(i => i.Owner.ProfessionalUserId == userId).FirstOrDefault();
            return company;
        }

        /// <summary>
        /// Returns the company the professional is employed by. Returns null for owners
        /// </summary>
        /// <param name="userIdString"></param>
        /// <returns></returns>
        public Company GetCompanyAndThisEmployeeFromEmployeeProfessionalUserId(string userIdString)
        {
            Guid userId = GuidHelper.GetGuid(userIdString);
            Company returnValue = GetCompanyAndAllProfessionalsFromEmployeeProfessionalUserId(userId);
            if(returnValue != null && returnValue.Professionals != null)
            {
                returnValue.Professionals = returnValue.Professionals.Where(i => i.ProfessionalUserId == userId).ToList();
            }
            return returnValue;
        }


        /// <summary>
        /// Gets a company and all professionals from the employee ID.
        /// WARNING: This could be a security risk if used directly. Never use directly. Call from another method
        ///          and manipulate the data accordingly.
        /// </summary>
        /// <param name="userId">The user ID as a Guid (intentionally a guid, so inhibit direct calls)</param>
        /// <returns></returns>
        private Company GetCompanyAndAllProfessionalsFromEmployeeProfessionalUserId(Guid userId)
        {
            Company returnValue = null;

            // WARNING: Be careful if editing. Alterations to this can/WILL make the query very slow!
            var companyToReturn = _unitOfWork.CompaniesRepository.Get(i => i.Professionals.Any(x => x.ProfessionalUserId == userId), includeProperties: "Professionals").FirstOrDefault();

            if (companyToReturn != null)
            {
                returnValue = companyToReturn;
            }

            //var relevantProfessional = _unitOfWork.ProfessionalsRepository.Get(i => i.ProfessionalUserId == userId).FirstOrDefault();
            //if(relevantProfessional != null)
            //{
            //    var companyToReturn = _unitOfWork.CompaniesRepository.Get(i => i.Professionals.FirstOrDefault(p => p.ProfessionalId == relevantProfessional.ProfessionalId) != null, includeProperties: "Professionals").FirstOrDefault();
            //}

            return returnValue;
        }


        /// <summary>
        /// From the owners professionalId, get the company with all employees (this will include the professional themselves in the owner section!)
        /// WARNING: This could be a security risk if used directly. Be very careful with it.
        /// </summary>
        /// <param name="userId">The owners user id</param>
        /// <returns></returns>
        public Company GetCompanyAndAllEmployeesFromOwnerProfessionalUserId(Guid userId)
        {
            var returnValue = _unitOfWork.CompaniesRepository.Get(i => i.Owner.ProfessionalUserId == userId, includeProperties: "Professionals").FirstOrDefault();

            return returnValue;
        }


        public void AddOrUpdateCompanyFromDto(string requestorUserId, CompanyUiDto companyDto)
        {
            Guid userId = GuidHelper.GetGuid(requestorUserId);

            //1. Make sure the user has the rights to do this. Are the company owner, 
            //   or is there no company for this pro?
            var dbCompany = GetCompanyFromOwnerUserGuid(requestorUserId);
            var companyByCompanyId = _unitOfWork.CompaniesRepository.GetByGuid(companyDto.CompanyId);

            // validate (make sure company id's haven't changed, which would sneakily switch ownership)
            if (dbCompany != null && companyByCompanyId != null &&
                dbCompany.CompanyId != companyByCompanyId.CompanyId)
            {
                throw new InvalidOperationException("Only the company owner can update company details.");
            }

            bool isInsert = false;
            if (dbCompany == null)
            {
                // new company setup, add it AND ADD OWNERSHIP!
                dbCompany = new Company();
                dbCompany.CompanyId = Guid.NewGuid();
                dbCompany.Owner = _unitOfWork.ProfessionalsRepository.Get(i => i.ProfessionalUserId == userId).FirstOrDefault();
                isInsert = true;
            }

            dbCompany.CompanyName = companyDto.CompanyName;
            dbCompany.AddressLine1 = companyDto.AddressLine1;
            dbCompany.AddressLine2 = companyDto.AddressLine2;
            dbCompany.TownCity = companyDto.TownCity;
            dbCompany.County = companyDto.County;
            dbCompany.Postcode = companyDto.Postcode;
            dbCompany.MainContactName = companyDto.MainContactName;
            dbCompany.MainContactEmail = companyDto.MainContactEmail;
            dbCompany.MainContactTel = companyDto.MainContactTel;
            dbCompany.SecondaryContactName = companyDto.SecondaryContactName;
            dbCompany.SecondaryContactEmail = companyDto.SecondaryContactEmail;
            dbCompany.SecondaryContactTel = companyDto.SecondaryContactTel;

            if (isInsert)
                _unitOfWork.CompaniesRepository.Insert(dbCompany);
            else
                _unitOfWork.CompaniesRepository.Update(dbCompany);

            _unitOfWork.Save();


        }



        /// <summary>
        /// Return a list of all (employees) professional Id's and names that the requesting professional can access.
        /// The professional themselves will be included if flagged
        /// </summary>
        /// <param name="requestorUserId">The user ID of the requesting pro</param>
        /// <param name="includeOwner">should the owner be included in the return list</param>
        /// <param name="ownerIndex">Where to put the owner in the resulting list (usually zero)</param>
        /// <returns></returns>
        public List<AccessibleEmployee> GetAccessibleEmployeesForProfessional(string requestorUserId, bool includeOwner, int ownerIndex = 0)
        {
            Guid userId = GuidHelper.GetGuid(requestorUserId);
            List<AccessibleEmployee> returnList = new List<AccessibleEmployee>();

            // make sure we validate!
            // for now, only a company owner has access to other professionals (employees)
            // this could change in the future.
            var company = GetCompanyAndAllEmployeesFromOwnerProfessionalUserId(userId);

            if(company != null && company.Professionals != null && company.Professionals.Any())
            {
                foreach(var pro in company.Professionals)
                {
                    AccessibleEmployee employee = new AccessibleEmployee(pro.ProfessionalId, pro.Forename, pro.Surname, false);
                    returnList.Add(employee);
                }

                returnList = returnList.OrderBy(i => i.Forename).ToList();
            }

            if (includeOwner && company != null && company.Owner != null)
            {
                var owner = new AccessibleEmployee(company.Owner.ProfessionalId, company.Owner.Forename, company.Owner.Surname, true);
                if (ownerIndex < returnList.Count())
                {
                    returnList.Insert(ownerIndex, owner);
                }
                else
                {
                    returnList.Insert(0, owner);
                }

            }

            // hack to save going back to the db. Assume if there aren't any rows, 
            // it's an employee/ sole trader and we need their data.
            if(returnList == null || !returnList.Any())
            {
                var pro = _unitOfWork.ProfessionalsRepository.Get(i => i.ProfessionalUserId == userId).FirstOrDefault();
                if (pro != null)
                {
                    var thisEmployee = new AccessibleEmployee(pro.ProfessionalId, pro.Forename, pro.Surname, true);
                    returnList.Insert(0, thisEmployee);
                }
            }

            return returnList;
        }

    }
}
