using AppointmentsDb.Helpers;
using AppointmentsDb.Models;
using AppointmentsDb.ModelsDto;
using AppointmentsDb.Pattern;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentsDb.Queries
{
    public interface ILinkingKeyQueries
    {
        List<LinkingKeyUiDto> GetLinkingKeysForProfessional(string userIdString);
        LinkingKeyUiDto CreateLinkingKey_AddProfessionalToCompany(string userIdString, string linkToEmailAddress, DateTime expiry);
        bool UseLinkingKey_AddProfessionalToCompany(string userIdString, string specialKey);
        CompanyUiDto LinkingKey_GetCompany(string specialKey);
    }

    public class LinkingKeyQueries
    {
        IMapper _mapper;
        IUnitOfWork _unitOfWork;
        ProfessionalQueries _professionalQueries;
        CompanyQueries _companyQueries;

        public LinkingKeyQueries(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _professionalQueries = new ProfessionalQueries(_unitOfWork, _mapper);
            _companyQueries = new CompanyQueries(_unitOfWork, mapper);
        }


        public List<LinkingKeyUiDto> GetLinkingKeysForProfessional(string userIdString)
        {
            Guid userId = GuidHelper.GetGuid(userIdString);
            var professional = _professionalQueries.GetProfessionalFromUserGuid(userIdString);
            // RULE: Return empty lists rather than null lists.
            List<LinkingKeyUiDto> returnValue = new List<LinkingKeyUiDto>();

            var keys = _unitOfWork.LinkingKeyRepository.Get(
                                i => i.Owner != null && 
                                i.Owner.ProfessionalId == professional.ProfessionalId).ToList();

            if (keys != null)
                returnValue = _mapper.Map<List<LinkingKey>, List<LinkingKeyUiDto>>(keys);

            return returnValue;
        }



        #region "Adding Professionals To Companies"

        public LinkingKeyUiDto CreateLinkingKey_AddProfessionalToCompany(string userIdString, string linkToEmailAddress, DateTime expiry)
        {
            LinkingKeyUiDto returnValue = null;
            var companyOwner = _companyQueries.GetCompanyFromOwnerUserGuid(userIdString);
            var professional = _professionalQueries.GetProfessionalFromUserGuid(userIdString);

            if (companyOwner == null)
            {
                throw new InvalidOperationException("Only a company owner can add a key");
            }

            LinkingKey linkingKey = new LinkingKey();
            linkingKey.CreatedDateTime = DateTime.Now;
            linkingKey.AvailableForEmailAddress = linkToEmailAddress.ToLower().Trim();
            linkingKey.ExpiryDateTime = expiry;

            linkingKey.Owner = professional;
            linkingKey.FromTableName = "Professional";
            linkingKey.FromTablePK = ""; /* they might not have signed up to our service yet. */

            linkingKey.ToTableName = "Company";
            linkingKey.ToTablePK = companyOwner.CompanyId.ToString();

            linkingKey.SpecialKey = Helpers.SpecialKeyGenerator.CreateSpecialKey();


            _unitOfWork.LinkingKeyRepository.Insert(linkingKey);
            _unitOfWork.Save();


            returnValue = _mapper.Map<LinkingKey, LinkingKeyUiDto>(linkingKey);

            return returnValue;
        }


        /// <summary>
        /// In the case that we have used the key to get a company (it could be used for other things)
        /// Get the company from the Key.
        /// </summary>
        /// <param name="specialKey"></param>
        /// <returns></returns>
        public CompanyUiDto GetUiDto_LinkingKey_GetCompany(string specialKey)
        {
            CompanyUiDto returnValue = null;
            Company company = LinkingKey_GetCompany(specialKey);

            if (company != null)
            {
                returnValue = _mapper.Map<Company, CompanyUiDto>(company);
            }

            return returnValue;
        }


        /// <summary>
        /// In the case that we have used the key to get a company (it could be used for other things)
        /// Get the company from the Key.
        /// </summary>
        /// <param name="specialKey"></param>
        /// <returns></returns>
        public Company LinkingKey_GetCompany(string specialKey)
        {
            Company returnValue = null;

            var linkingKeyRow = _unitOfWork.LinkingKeyRepository.Get(i => i.SpecialKey == specialKey &&
                                                i.UsedDateTime == null &&
                                                i.ExpiryDateTime > DateTime.Now).FirstOrDefault();
            
            if (linkingKeyRow != null && linkingKeyRow.ToTableName == "Company")
            {
                Guid key = GuidHelper.GetGuid(linkingKeyRow.ToTablePK);
                var company = _unitOfWork.CompaniesRepository.GetByGuid(key);
                returnValue = company;
            }

            return returnValue;
        }



        /// <summary>
        /// If this user is able to implement the special key (that would link the professional to the company), do so.
        /// </summary>
        /// <param name="userIdString"></param>
        /// <param name="specialKey"></param>
        /// <returns></returns>
        public bool UseLinkingKey_AddProfessionalToCompany(string userIdString, string specialKey)
        {
            bool isSuccessful = false;

            var professional = _professionalQueries.GetProfessionalFromUserGuid(userIdString);
            var specialKeyRow = _unitOfWork.LinkingKeyRepository.Get(i => i.SpecialKey == specialKey).FirstOrDefault();            

            if (specialKeyRow != null)
            {
                // is it for this professional?
                if (professional.EmailAddress.Trim().ToLower() == specialKeyRow.AvailableForEmailAddress.Trim().ToLower()
                    && DateTime.Now < specialKeyRow.ExpiryDateTime
                    && (specialKeyRow.UsedByProfessionalId == null || specialKeyRow.UsedByProfessionalId == new Guid())
                    && (specialKeyRow.UsedDateTime == null || specialKeyRow.UsedDateTime == new DateTime())
                    && specialKeyRow.ToTableName == "Company")
                {
                    var companyGuid = Helpers.GuidHelper.GetGuid(specialKeyRow.ToTablePK);
                    var newCompanyToLinkTo = _unitOfWork.CompaniesRepository.GetByGuid(companyGuid, includeProperties: "Owner");

                    // if we're going to try to link to a real company AND WE DONT RE-LINK OWNERS! They can only be owners.
                    if (newCompanyToLinkTo != null && newCompanyToLinkTo.Owner.ProfessionalId != professional.ProfessionalId)
                    {
                        // check there aren't links to another company. If so, delete them and add a note.
                        var existingCompanyLink = _companyQueries.GetCompanyAndThisEmployeeFromEmployeeProfessionalUserId(userIdString);
                        if (existingCompanyLink != null)
                        {
                            professional.Notes = professional.Notes + "Left company " + existingCompanyLink.CompanyId + " on " + DateTime.Now + ".";

                            var rowToRemove = existingCompanyLink.Professionals.FirstOrDefault(i => i.ProfessionalId == professional.ProfessionalId);
                            if (rowToRemove != null)
                            {
                                existingCompanyLink.Professionals.Remove(rowToRemove);
                                _unitOfWork.CompaniesRepository.Update(existingCompanyLink);
                            }
                        }

                        specialKeyRow.UsedByProfessionalId = professional.ProfessionalId;
                        specialKeyRow.UsedDateTime = DateTime.Now;
                        newCompanyToLinkTo.Professionals.Add(professional);
                        professional.Notes = professional.Notes +"Joined company " + newCompanyToLinkTo.CompanyId + " on " + DateTime.Now + ". ";
                        _unitOfWork.CompaniesRepository.Update(newCompanyToLinkTo);

                        _unitOfWork.Save();

                        isSuccessful = true;
                    }
                }
            }

            // TODO: Check existing connections are deleted.

            return isSuccessful;

        }

        #endregion
    }
}
