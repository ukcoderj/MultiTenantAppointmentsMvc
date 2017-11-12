using AppointmentsDb.Helpers;
using AppointmentsDb.Models;
using AppointmentsDb.ModelsDto;
using AppointmentsDb.Pattern;
using AutoMapper;
using Shared.Enums;
using Shared.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentsDb.Queries
{
    public interface ICompanyLocationGroupQueries
    {
        List<CompanyLocationGroup> GetCompanyLocationGroups(string userIdString, out AuthorizationState authState);
        List<CompanyLocationGroupUiDto> GetUiDto_CompanyLocationGroups(string userIdString, out AuthorizationState authState);
        CompanyLocationGroupUiDto GetUiDto_CompanyLocationGroupById(string userIdString, Guid companyLocationGroupId);
        void FromUiDto_AddOrUpdateCompanyLocationGroup(string userIdString, CompanyLocationGroupUiDto companyLocationGroupUiDto);
    }


    public class CompanyLocationGroupQueries : ICompanyLocationGroupQueries
    {
        IMapper _mapper;
        IUnitOfWork _unitOfWork;
        IAccessQueries _accessQueries;
        ICompanyQueries _companyQueries;

        public CompanyLocationGroupQueries(IUnitOfWork unitOfWork, IMapper mapper, IAccessQueries accessQueries, ICompanyQueries companyQueries)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _accessQueries = accessQueries;
            _companyQueries = companyQueries;
        }


        public List<CompanyLocationGroupUiDto> GetUiDto_CompanyLocationGroups(string userIdString, out AuthorizationState authState)
        {
            var companyLocationGroups = GetCompanyLocationGroups(userIdString, out authState);
            List<CompanyLocationGroupUiDto> returnValue = new List<CompanyLocationGroupUiDto>();

            if (companyLocationGroups != null)
            {
                foreach (var companyLocationGroup in companyLocationGroups)
                {                    
                    var row = ConvertCompanyLocationGroupToUiDto(companyLocationGroup, authState);
                    row.AuthState_OnlyTrustOnGeneration = authState;
                    if (row != null)
                    {
                        returnValue.Add(row);
                    }
                }
            }

            return returnValue;
        }


        public List<CompanyLocationGroup> GetCompanyLocationGroups(string userIdString, out AuthorizationState authState)
        {
            List<CompanyLocationGroup> returnValue = new List<CompanyLocationGroup>();
            bool isCompanyOwner = false;

            // do a separate check for access. This is the correct pattern.
            Company company = _accessQueries.GetAuthorization_ForCompanyAdmin_IfCompanyIdSelectedByUserId(Guid.Parse(userIdString), false, out authState, out isCompanyOwner);

            if (company != null)
            {
                returnValue = _unitOfWork.CompanyLocationGroupsRepository.Get(i => i.Company.CompanyId == company.CompanyId, includeProperties: "CompanyLocations").ToList();
            }

            return returnValue;
        }

        /// <summary>
        /// WARNING: This could return null;
        /// </summary>
        /// <param name="userIdString"></param>
        /// <param name="companyLocationGroupId"></param>
        /// <returns></returns>
        public CompanyLocationGroupUiDto GetUiDto_CompanyLocationGroupById(string userIdString, Guid companyLocationGroupId)
        {
            AuthorizationState authState = AuthorizationState.NotAllowed;
            bool isCompanyOwner = false;
            var companyLocationGroup = GetCompanyLocationGroupById(userIdString, companyLocationGroupId, out authState, out isCompanyOwner);
            CompanyLocationGroupUiDto returnValue = ConvertCompanyLocationGroupToUiDto(companyLocationGroup, authState);
            return returnValue;
        }


        private CompanyLocationGroup GetCompanyLocationGroupById(string userIdString, Guid companyLocationGroupId, out AuthorizationState authState, out bool isCompanyOwner)
        {
            Guid userId = GuidHelper.GetGuid(userIdString);
            CompanyLocationGroup returnValue = new CompanyLocationGroup();

            // companyLocation will be gained with the access query to save double dipping the db.
            returnValue = _accessQueries.GetAuthorization_ForACompanyLocationGroup(userId, companyLocationGroupId, out authState, out isCompanyOwner);

            return returnValue;
        }


        public void FromUiDto_AddOrUpdateCompanyLocationGroup(string userIdString, CompanyLocationGroupUiDto companyLocationGroupUiDto)
        {
            if (companyLocationGroupUiDto != null)
            {
                var companyLocationGroup = _mapper.Map<CompanyLocationGroupUiDto, CompanyLocationGroup>(companyLocationGroupUiDto);

                // convert the sub rows of data.
                List<CompanyLocation> companyLocationsList = new List<CompanyLocation>();
                if(companyLocationGroupUiDto.CompanyLocationUiDtos != null && companyLocationGroupUiDto.CompanyLocationUiDtos.Any())
                {
                    foreach(var locationDto in companyLocationGroupUiDto.CompanyLocationUiDtos)
                    {
                        var row = _mapper.Map<CompanyLocationUiDto, CompanyLocation>(locationDto);
                        companyLocationsList.Add(row);
                    }
                }

                companyLocationGroup.CompanyLocations = companyLocationsList;

                AddOrUpdateCompanyLocationGroup(userIdString, companyLocationGroup);
            }
        }


        private void AddOrUpdateCompanyLocationGroup(string userIdString, CompanyLocationGroup companyLocationGroup)
        {
            Guid userId = GuidHelper.GetGuid(userIdString);
            AuthorizationState authState = AuthorizationState.NotAllowed;
            bool isCompanyOwner = false;
            List<CompanyLocationGroup> returnValue = new List<CompanyLocationGroup>();
            var now = DateTime.Now;
            bool isInsert = false;

            // do a separate check for access. This is the correct pattern.
            var existingClg = _accessQueries.GetAuthorization_ForACompanyLocationGroup(userId, companyLocationGroup.CompanyLocationGroupId, out authState, out isCompanyOwner);

            if (authState >= AuthorizationState.CreateReadUpdate)
            {
                // Is the CLG new?
                //var existingClg = _unitOfWork.CompanyLocationGroupsRepository.Get(i => i.CompanyLocationGroupId == companyLocationGroup.CompanyLocationGroupId,
                //                                                                                                    includeProperties: "CompanyLocations").FirstOrDefault();

                var submittingPro = _unitOfWork.ProfessionalsRepository.Get(i => i.ProfessionalUserId == userId).FirstOrDefault();

                if (existingClg == null)
                {
                    // New
                    isInsert = true;
                    var company = _companyQueries.GetCompanyFromOwnerUserGuid(userIdString);
                    companyLocationGroup.Company = company;
                    companyLocationGroup.CreatedByProfessional = submittingPro;
                    companyLocationGroup.CreatedDate = DateTime.Now;                    
                    companyLocationGroup.CompanyLocationGroupId = Guid.NewGuid();

                    if (companyLocationGroup.CompanyLocations != null)
                    {
                        foreach (var companyLocation in companyLocationGroup.CompanyLocations)
                        {
                            companyLocation.CreatedByProfessionalId = submittingPro.ProfessionalId;
                            companyLocation.CreatedDate = now;
                            companyLocation.UpdatedByProfessionalId = submittingPro.ProfessionalId;
                            companyLocation.UpdatedDate = now;
                            companyLocation.CompanyLocationId = Guid.NewGuid();
                        }
                    }
                }
                else
                {
                    //UPDATE THE EXISTING CLG
                    existingClg.LocationGroupName = companyLocationGroup.LocationGroupName;

                    // CLG - has the deletion flag changed
                    if (!existingClg.IsDeleted && companyLocationGroup.IsDeleted)
                    {
                        existingClg.IsDeleted = companyLocationGroup.IsDeleted;
                        existingClg.DeletedByProfessionalId = submittingPro.ProfessionalId;
                        existingClg.DeletedDate = DateTime.Now;
                    }
                    else if (existingClg.IsDeleted && !companyLocationGroup.IsDeleted)
                    {
                        existingClg.IsDeleted = companyLocationGroup.IsDeleted;
                        existingClg.DeletedByProfessionalId = null;
                        existingClg.DeletedDate = null;
                    }

                    // UPDATE EACH CL
                    if (companyLocationGroup.CompanyLocations != null)
                    {
                        // stop null ref exceptions later.
                        if (existingClg.CompanyLocations == null)
                        {
                            existingClg.CompanyLocations = new List<CompanyLocation>();
                        }

                        // there will always be more than or equal num locations incoming than in the db as we don't allow deletions.
                        foreach (var companyLocation in companyLocationGroup.CompanyLocations)
                        {
                            // is the CL new?
                            var existingCLRow = existingClg.CompanyLocations.FirstOrDefault(i => i.CompanyLocationId == companyLocation.CompanyLocationId);
                            if (existingCLRow == null)
                            {
                                companyLocation.CompanyLocationId = Guid.NewGuid();
                                existingClg.CompanyLocations.Add(companyLocation);
                                companyLocation.CreatedByProfessionalId = submittingPro.ProfessionalId;
                                companyLocation.CreatedDate = now;
                                companyLocation.UpdatedByProfessionalId = submittingPro.ProfessionalId;
                                companyLocation.UpdatedDate = now;
                            }
                            else
                            {
                                // update
                                existingCLRow.UpdatedByProfessionalId = submittingPro.ProfessionalId;
                                existingCLRow.UpdatedDate = now;
                                existingCLRow.IsProfessionalVisitToClientLocation = companyLocation.IsProfessionalVisitToClientLocation;
                                existingCLRow.Postcode = companyLocation.Postcode;
                                existingCLRow.IsPostCodeAreaAndDistrictOnly = AddressHelper.IsPostCodeAreaAndDistrict(companyLocation.Postcode);
                                existingCLRow.IsPostCodeAreaDistrictSectorOnly = AddressHelper.IsPostCodeAreaDistrictSector(companyLocation.Postcode);
                                existingCLRow.AddressLine1 = companyLocation.AddressLine1;
                                existingCLRow.AddressLine2 = companyLocation.AddressLine2;
                                existingCLRow.TownCity = companyLocation.TownCity;
                                existingCLRow.County = companyLocation.County;

                                // have the CL deletion flags changed?
                                if (!existingCLRow.IsDeleted && companyLocation.IsDeleted)
                                {
                                    existingCLRow.IsDeleted = companyLocation.IsDeleted;
                                    existingCLRow.DeletedByProfessionalId = submittingPro.ProfessionalId;
                                    existingCLRow.DeletedDate = DateTime.Now;
                                }
                                else if (existingCLRow.IsDeleted && !companyLocation.IsDeleted)
                                {
                                    existingCLRow.IsDeleted = companyLocation.IsDeleted;
                                    existingCLRow.DeletedByProfessionalId = null;
                                    existingCLRow.DeletedDate = null;
                                }
                            }
                        }
                    }
                }

                // Do any of the locations have a mobile location?
                companyLocationGroup.HasProfessionalVisitsClientLocations = companyLocationGroup.CompanyLocations.FirstOrDefault(i => i.IsProfessionalVisitToClientLocation) != null;

                if(isInsert)
                {
                    _unitOfWork.CompanyLocationGroupsRepository.Insert(companyLocationGroup);
                }
                else
                {
                    //_unitOfWork.CompanyLocationGroupsRepository.Update(companyLocationGroup);
                }

                _unitOfWork.Save();

            }
        }


        private CompanyLocationGroupUiDto ConvertCompanyLocationGroupToUiDto(CompanyLocationGroup companyLocationGroup, AuthorizationState authState)
        {
            CompanyLocationGroupUiDto returnValue = null;

            if (companyLocationGroup != null)
            {
                returnValue = _mapper.Map<CompanyLocationGroup, CompanyLocationGroupUiDto>(companyLocationGroup);
                returnValue.AuthState_OnlyTrustOnGeneration = authState;
                companyLocationGroup.CompanyLocations = companyLocationGroup.CompanyLocations ?? new List<CompanyLocation>();
                returnValue.CompanyLocationUiDtos = returnValue.CompanyLocationUiDtos ?? new List<CompanyLocationUiDto>();

                foreach (var cLocation in companyLocationGroup.CompanyLocations)
                {
                    CompanyLocationUiDto cLocationUiDto = _mapper.Map<CompanyLocation, CompanyLocationUiDto>(cLocation);
                    cLocationUiDto.AuthState_OnlyTrustOnGeneration = authState;
                    returnValue.CompanyLocationUiDtos.Add(cLocationUiDto);
                }
            }

            return returnValue;
        }

    }
}
