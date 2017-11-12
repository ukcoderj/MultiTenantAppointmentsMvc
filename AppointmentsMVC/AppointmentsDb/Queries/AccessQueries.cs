using AppointmentsDb.Helpers;
using AppointmentsDb.Models;
using AppointmentsDb.Pattern;
using AutoMapper;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentsDb.Queries
{

    public interface IAccessQueries
    {
        /// <summary>
        /// Main query for establishing whether one user can access another.
        /// </summary>
        /// <param name="professionalUserId"></param>
        /// <param name="professionalId"></param>
        /// <returns></returns>
        bool DoesUserProfessionalHaveAccessToProfessional_Appointments(Guid professionalUserId, Guid professionalId);

        /// <summary>
        /// Get a company location group along with authorization for what the user can do with it.
        /// Returns null if the user has no access.
        /// </summary>
        /// <param name="professionalUserId"></param>
        /// <returns></returns>
        CompanyLocationGroup GetAuthorization_ForACompanyLocationGroup(Guid professionalUserId, Guid companyLocationGroupId, out AuthorizationState authState, out bool isCompanyOwner);

        /// <summary>
        /// Get the company for the user along with Authorization info for what they can do with that company.
        /// For now we will assume: 1. Company Owner can do everything. 2. The employee can read, but not edit. 3. Everyone else is forbidden.
        /// This will only be used when the company is to be inferred from the user.
        /// </summary>
        /// <param name="professionalUserId"></param>
        /// <returns></returns>
        Company GetAuthorization_ForCompanyAdmin_IfCompanyIdSelectedByUserId(Guid professionalUserId, bool includeAllEmployees, out AuthorizationState authState, out bool isCompanyOwner);

        /// <summary>
        /// Get the company for the user along with Authorization info for what they can do with that company.
        /// For now we will assume: 1. Company Owner can do everything. 2. The employee can read, but not edit. 3. Everyone else is forbidden.
        /// This cross checks with the supplied company Id to make sure the request is ok.
        /// </summary>
        /// <param name="professionalUserId"></param>
        /// <returns></returns>
        Company GetAuthorization_ForCompanyAdmin_IfCompanyIdProvided(Guid professionalUserId, Guid? companyId, bool includeAllEmployees, out AuthorizationState authState, out bool isCompanyOwner);


        /// <summary>
        /// Is the user a match for the professionalId provided (i.e. a check for someone tampering with the data, to gain elevated privileges)
        /// </summary>
        /// <param name="professionalUserId"></param>
        /// <param name="professionalId"></param>
        /// <returns></returns>
        bool IsUserProfessionalIdMatch(Guid professionalUserId, Guid professionalId);

        /// <summary>
        /// Is the user the company owner for the professional they are trying to access.
        /// </summary>
        /// <param name="professionalUserId"></param>
        /// <param name="professionalId"></param>
        /// <returns>True if the user is the employer</returns>
        bool IsUserCompanyOwnerForProfessionalId(Guid professionalUserId, Guid professionalId);

        /// <summary>
        /// Is a professional allowed to udpate working hours for a given pro ids;
        /// Returns the professionals being updated (not necessarily the user in future)
        /// </summary>
        List<Professional> GetAuthorization_ProfessionalWorkingHours_ReturnProsToUpdate(Guid professionalUserId, List<Guid> professionalIdsToUpdate, out AuthorizationState authState);
    }

    public class AccessQueries : IAccessQueries
    {
        IMapper _mapper;
        IUnitOfWork _unitOfWork;
        ICompanyQueries _companyQueries;

        public AccessQueries(IUnitOfWork unitOfWork, ICompanyQueries companyQueries, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _companyQueries = companyQueries;
        }


        /// <summary>
        /// Under any circumstances, can the user making the request access the professional they want to access.
        /// </summary>
        /// <param name="professionalUserId"></param>
        /// <param name="professionalId"></param>
        /// <returns></returns>
        public bool DoesUserProfessionalHaveAccessToProfessional_Appointments(Guid professionalUserId, Guid professionalId)
        {
            // 1. The user is themselves.            
            if (IsUserProfessionalIdMatch(professionalUserId, professionalId))
            {
                return true;
            }

            // 2. If the user is the company owner and the pro is an employee.
            if (IsUserCompanyOwnerForProfessionalId(professionalUserId, professionalId))
            {
                return true;
            }

            return false;
        }



        /// <summary>
        /// Is the professionalID just themself?
        /// </summary>
        /// <param name="professionalUserId"></param>
        /// <param name="professionalId"></param>
        /// <returns></returns>
        public bool IsUserProfessionalIdMatch(Guid professionalUserId, Guid professionalId)
        {
            var userProfessional = _unitOfWork.ProfessionalsRepository.Get(i => i.ProfessionalUserId == professionalUserId).FirstOrDefault();
            return userProfessional != null && userProfessional.ProfessionalId == professionalId;
        }


        /// <summary>
        /// Is the user a company owner, and the professional ID is that of an employee?
        /// </summary>
        /// <param name="professionalUserId"></param>
        /// <param name="professionalId"></param>
        /// <returns></returns>
        public bool IsUserCompanyOwnerForProfessionalId(Guid professionalUserId, Guid professionalId)
        {
            var companyAndPros = _unitOfWork.CompaniesRepository.Get(i => i.Owner.ProfessionalUserId == professionalUserId, includeProperties: "Professionals").FirstOrDefault();
            if (companyAndPros != null && companyAndPros.Professionals != null && companyAndPros.Professionals.Any())
            {
                foreach (var row in companyAndPros.Professionals)
                {
                    if (row.ProfessionalId == professionalId)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// From the info provided, return the companyLocationGroup information along with the authorization on that element.
        /// For now we will assume: 1. Company Owner can do everything. 2. The employee can read, but not edit. 3. Everyone else is forbidden.
        /// </summary>
        /// <param name="professionalUserId"></param>
        /// <param name="companyLocationGroupId"></param>
        /// <param name="authState"></param>
        /// <param name="isCompanyOwner"></param>
        /// <returns></returns>
        public CompanyLocationGroup GetAuthorization_ForACompanyLocationGroup(Guid professionalUserId, Guid companyLocationGroupId, out AuthorizationState authState, out bool isCompanyOwner)
        {
            authState = AuthorizationState.NotAllowed;
            isCompanyOwner = false;
            Company company;
            CompanyLocationGroup companyLocationGroup = null;

            company = _companyQueries.GetCompanyFromOwnerUserGuid(professionalUserId.ToString());

            if (company != null)
            {
                // Owner - If it's the company owner, they have full rights.
                companyLocationGroup = _unitOfWork.CompanyLocationGroupsRepository.Get(i => i.CompanyLocationGroupId == companyLocationGroupId, includeProperties: "Company,CompanyLocations").FirstOrDefault();
                if(companyLocationGroup == null)
                {
                    companyLocationGroup = null;
                    authState = AuthorizationState.CreateReadUpdate;
                }
                else if (companyLocationGroup != null && companyLocationGroup.Company != null && companyLocationGroup.Company.CompanyId == company.CompanyId)
                {
                    isCompanyOwner = true;
                    authState = AuthorizationState.CreateReadUpdate;
                }
                else
                {
                    companyLocationGroup = null;
                    authState = AuthorizationState.NotAllowed;
                }
            }
            else
            {
                // Employee - If it's an employee, they can view.
                company = _companyQueries.GetCompanyAndThisEmployeeFromEmployeeProfessionalUserId(professionalUserId.ToString());
                if (company != null)
                {
                    companyLocationGroup = _unitOfWork.CompanyLocationGroupsRepository.Get(i => i.CompanyLocationGroupId == companyLocationGroupId, includeProperties: "Company,CompanyLocations").FirstOrDefault();
                    if (companyLocationGroup != null && companyLocationGroup.Company != null && companyLocationGroup.Company.CompanyId == company.CompanyId)
                    {
                        authState = AuthorizationState.ReadOnly;
                    }
                    else
                    {
                        companyLocationGroup = null;
                        authState = AuthorizationState.NotAllowed;
                    }
                }
            }

            // If it's anyone else, they can bugger off!
            return companyLocationGroup;
        }



        /// <summary>
        /// Get the company for the user along with Authorization info for what they can do with that company.
        /// For now we will assume: 1. Company Owner can do everything. 2. The employee can read, but not edit. 3. Everyone else is forbidden.
        /// This will only be used when the company is to be inferred from the user.
        /// </summary>
        /// <param name="professionalUserId"></param>
        /// <param name="includeAllEmployees">Do we return all employees with the company (will apply to company owners only for now)</param>
        /// <param name="authState"></param>
        /// <param name="isCompanyOwner"></param>
        /// <returns></returns>
        public Company GetAuthorization_ForCompanyAdmin_IfCompanyIdSelectedByUserId(Guid professionalUserId, bool includeAllEmployees, out AuthorizationState authState, out bool isCompanyOwner)
        {
            authState = AuthorizationState.NotAllowed;
            isCompanyOwner = false;
            Company company = null;

            if (includeAllEmployees)
                company = _companyQueries.GetCompanyAndAllEmployeesFromOwnerProfessionalUserId(professionalUserId);
            else
                company = _companyQueries.GetCompanyFromOwnerUserGuid(professionalUserId.ToString());

            if (company != null)
            {
                // Owner - If it's the company owner, they have full rights.
                authState = AuthorizationState.CreateReadUpdate;
                isCompanyOwner = true;
                return company;
            }
            else
            {
                // Employee - If it's an employee, they can view.
                company = _companyQueries.GetCompanyAndThisEmployeeFromEmployeeProfessionalUserId(professionalUserId.ToString());
                if (company != null)
                {
                    authState = AuthorizationState.ReadOnly;
                    isCompanyOwner = false;
                    return company;
                }
            }

            // If it's anyone else, they can bugger off!
            return company;
        }


        /// <summary>
        /// Get the company for the user along with Authorization info for what they can do with that company.
        /// For now we will assume: 1. Company Owner can do everything. 2. The employee can read, but not edit. 3. Everyone else is forbidden.
        /// This cross checks with the supplied company Id to make sure the request is ok.
        /// </summary>
        /// <param name="professionalUserId"></param>
        /// <param name="companyId"></param>
        /// <param name="includeAllEmployees">Do we return all employees with the company (will apply to company owners only for now)</param>
        /// <param name="authState"></param>        
        /// <param name="isCompanyOwner"></param>
        /// <returns></returns>
        public Company GetAuthorization_ForCompanyAdmin_IfCompanyIdProvided(Guid professionalUserId, Guid? companyId, bool includeAllEmployees, out AuthorizationState authState, out bool isCompanyOwner)
        {
            authState = AuthorizationState.NotAllowed;
            isCompanyOwner = false;
            Company company = null;

            if(includeAllEmployees)
                company = _companyQueries.GetCompanyAndAllEmployeesFromOwnerProfessionalUserId(professionalUserId);
            else
                company = _companyQueries.GetCompanyFromOwnerUserGuid(professionalUserId.ToString());

            if (company != null)
            {
                if (company.CompanyId == companyId)
                {
                    // Owner - If it's the company owner, they have full rights.
                    authState = AuthorizationState.CreateReadUpdate;
                    isCompanyOwner = true;
                    return company;
                }
                else
                {
                    // They own another company, not this one
                    authState = AuthorizationState.NotAllowed;
                    isCompanyOwner = false;
                    return company;
                }
            }
            else
            {
                // Employee - If it's an employee, they can view.
                company = _companyQueries.GetCompanyAndThisEmployeeFromEmployeeProfessionalUserId(professionalUserId.ToString());
                if (company != null)
                {
                    if (company.CompanyId == companyId)
                    {
                        // employee .
                        authState = AuthorizationState.ReadOnly;
                        isCompanyOwner = false;
                        return company;
                    }
                    else
                    {
                        // They belong to another company
                        authState = AuthorizationState.NotAllowed;
                        isCompanyOwner = false;
                        return company;
                    }
                }
            }

            return company;
        }




        public List<Professional> GetAuthorization_ProfessionalWorkingHours_ReturnProsToUpdate(Guid professionalUserId, List<Guid> professionalIdsToUpdate, out AuthorizationState authState)
        {
            bool isCompanyOwner = false;
            var company = GetAuthorization_ForCompanyAdmin_IfCompanyIdSelectedByUserId(professionalUserId, true, out authState, out isCompanyOwner);
            List<Guid> allProIds = new List<Guid>();
            List<Professional> returnProfessionals = new List<Professional>();


            if(company != null && company.Owner != null && company.Owner.ProfessionalUserId == professionalUserId)
            {
                authState = AuthorizationState.CreateReadUpdate;                
            }


            if(authState > AuthorizationState.NotAllowed)
            {
                List<Guid> proIds = company.Professionals.Select(i => i.ProfessionalId).ToList();
                proIds.Add(company.Owner.ProfessionalId);


                var hasBadIds = professionalIdsToUpdate.FirstOrDefault(i => proIds.Contains(i)) != null;
                if(hasBadIds)
                {
                    authState = AuthorizationState.NotAllowed;
                }
                else
                {
                    returnProfessionals = _unitOfWork.ProfessionalsRepository.Get(i => allProIds.Contains(i.ProfessionalUserId), includeProperties: "ProfessionalWorkingHours").ToList();
                }
            }

            return returnProfessionals;

        }




        /// Keep this at the bottom.
        /// WARNING: Just because someone is A company onwer, does not mean they are THE company owner!!!
        ///          i.e. you must check if they are in charge of the company info you want to update!




    }
}
