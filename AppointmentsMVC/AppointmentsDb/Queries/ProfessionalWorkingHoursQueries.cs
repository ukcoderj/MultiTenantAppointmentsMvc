using AppointmentsDb.ModelsDto;
using AppointmentsDb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AppointmentsDb.Pattern;
using AppointmentsDb.Helpers;
using Shared.Enums;

namespace AppointmentsDb.Queries
{
    public interface IProfessionalWorkingHoursQueries
    {
        List<ProfessionalWorkingHourUiDto> GetUiDto_ProfessionalWorkingHoursFromUserGuid(string userIdString);
        List<ProfessionalWorkingHourUiDto> GetUiDto_AllProfessionalWorkingHoursInCompanyFromUserGuid(string userIdString);
        void FromUiDto_AddOrUpdateProfessionalWorkingHours(string userIdString, List<ProfessionalWorkingHourUiDto> professionalWorkingHoursUiDto);
        void AddOrUpdateProfessionalWorkingHours(string userIdString, List<ProfessionalWorkingHour> professionalWorkingHoursToUpdate, List<Guid> workingHoursProfessionalIds);
    }


    public class ProfessionalWorkingHoursQueries : IProfessionalWorkingHoursQueries
    {
        // Might need access queries (at least for is user the professional)
        // Only pro's can update their own availability.
        IMapper _mapper;
        IUnitOfWork _unitOfWork;
        IAccessQueries _accessQueries;
        ICompanyQueries _companyQueries;
        ICompanyLocationGroupQueries _companyLocationGroupQueries;

        public ProfessionalWorkingHoursQueries(IUnitOfWork unitOfWork, IMapper mapper, IAccessQueries accessQueries, ICompanyQueries companyQueries, ICompanyLocationGroupQueries companyLocationGroupQueries)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _accessQueries = accessQueries;
            _companyQueries = companyQueries;
            _companyLocationGroupQueries = companyLocationGroupQueries;
        }

        public List<ProfessionalWorkingHourUiDto> GetUiDto_ProfessionalWorkingHoursFromUserGuid(string userIdString)
        {
            Guid userId = GuidHelper.GetGuid(userIdString);
            List<ProfessionalWorkingHourUiDto> returnList = new List<ProfessionalWorkingHourUiDto>();
            // No access query needed, we just get the pro direct.            
            Professional professional = _unitOfWork.ProfessionalsRepository.Get(i => i.ProfessionalUserId == userId, includeProperties: "ProfessionalWorkingHours,ProfessionalWorkingHours.CompanyLocationGroup").FirstOrDefault();

            if (professional != null && professional.ProfessionalWorkingHours != null && professional.ProfessionalWorkingHours.Any())
            {
                foreach (var row in professional.ProfessionalWorkingHours.Where(r => !r.IsDeleted))
                {
                    ProfessionalWorkingHourUiDto uiRow = _mapper.Map<ProfessionalWorkingHour, ProfessionalWorkingHourUiDto>(row);
                    uiRow.CompanyLocationGroupId = row.CompanyLocationGroup.CompanyLocationGroupId;
                    uiRow.ProfessionalId = professional.ProfessionalId;
                    uiRow.ProfessionalName = professional.GetFullName();
                    returnList.Add(uiRow);
                }
            }

            return returnList;
        }

        public List<ProfessionalWorkingHourUiDto> GetUiDto_AllProfessionalWorkingHoursInCompanyFromUserGuid(string userIdString)
        {
            List<ProfessionalWorkingHourUiDto> returnList = new List<ProfessionalWorkingHourUiDto>();
            AuthorizationState authState = AuthorizationState.NotAllowed;
            bool isCompanyOwner = false;

            // Use AccessQueries to Check if the user has access (currently only the company owner)
            // Get all the professional Id's for the company while we're at it.
            Company company = _accessQueries.GetAuthorization_ForCompanyAdmin_IfCompanyIdSelectedByUserId(Guid.Parse(userIdString), true, out authState, out isCompanyOwner);

            if (company != null && authState > AuthorizationState.NotAllowed && company.Professionals != null && company.Professionals.Any())
            {
                // Get the working hours for each professionalId.
                var proIds = company.Professionals.Select(i => i.ProfessionalId).ToList();
                proIds.Add(company.Owner.ProfessionalId);

                // maybe take the any() check out here?
                var prosWithHours = _unitOfWork.ProfessionalsRepository.Get(i => proIds.Contains(i.ProfessionalId) && i.ProfessionalWorkingHours.Any(), includeProperties: "ProfessionalWorkingHours").ToList();

                // create rows for all the working hours.
                if (prosWithHours != null && prosWithHours.Any())
                {
                    foreach (var professionalRow in prosWithHours)
                    {
                        if (professionalRow.ProfessionalWorkingHours != null && professionalRow.ProfessionalWorkingHours.Any())
                        {
                            foreach (var workingHourRow in professionalRow.ProfessionalWorkingHours.Where(r => !r.IsDeleted))
                            {
                                if (workingHourRow.CompanyLocationGroup == null)
                                    continue;

                                ProfessionalWorkingHourUiDto uiRow = _mapper.Map<ProfessionalWorkingHour, ProfessionalWorkingHourUiDto>(workingHourRow);
                                uiRow.CompanyLocationGroupId = workingHourRow.CompanyLocationGroup.CompanyLocationGroupId;
                                uiRow.ProfessionalId = professionalRow.ProfessionalId;
                                uiRow.ProfessionalName = professionalRow.GetFullName();
                                returnList.Add(uiRow);
                            }

                        }
                    }

                    returnList = returnList.OrderBy(i => i.ProfessionalName).ToList();
                }
            }

            return returnList;
        }




        public void FromUiDto_AddOrUpdateProfessionalWorkingHours(string userIdString, List<ProfessionalWorkingHourUiDto> professionalWorkingHoursUiDto)
        {
            List<ProfessionalWorkingHour> professionalWorkingHours = new List<ProfessionalWorkingHour>();

            // Validation - only allow update of one professional at a time.
            List<Guid> proIds = professionalWorkingHoursUiDto.Select(i => i.ProfessionalId).ToList();

            // Need a list of companyLocationGroups for the udpate.
            AuthorizationState authState;
            List<CompanyLocationGroup> companyLocationGroups = _companyLocationGroupQueries.GetCompanyLocationGroups(userIdString, out authState);

            // Convert to db model.
            foreach (var row in professionalWorkingHoursUiDto)
            {
                ProfessionalWorkingHour pwh = _mapper.Map<ProfessionalWorkingHourUiDto, ProfessionalWorkingHour>(row);

                var matchingLocationGroup = companyLocationGroups.FirstOrDefault(i => i.CompanyLocationGroupId == row.CompanyLocationGroupId);
                if (matchingLocationGroup == null)
                {
                    throw new InvalidOperationException("Invalid company location group found");
                }

                pwh.CompanyLocationGroup = matchingLocationGroup;
                professionalWorkingHours.Add(pwh);
            }

            // Add / Update
            AddOrUpdateProfessionalWorkingHours(userIdString, professionalWorkingHours, proIds);
        }



        /// <summary>
        /// This should handle any authorised person updating the working hours for the professional.
        /// </summary>
        /// <param name="userIdString"></param>
        /// <param name="professionalWorkingHoursToUpdate"></param>
        /// <param name="workingHoursProfessionalId">workingHoursProfessionalId must match the correspoinding index of professionalWorkingHoursToUpdate</param>
        public void AddOrUpdateProfessionalWorkingHours(string userIdString, List<ProfessionalWorkingHour> professionalWorkingHoursToUpdate, List<Guid> workingHoursProfessionalIds)
        {
            //TODO: ****Null checks at top across the board!***
            AuthorizationState authState = AuthorizationState.NotAllowed;
            DateTime now = DateTime.Now;

            if (String.IsNullOrWhiteSpace(userIdString) || professionalWorkingHoursToUpdate == null || !professionalWorkingHoursToUpdate.Any())
                return;

            Guid userId = GuidHelper.GetGuid(userIdString);

            // Get all professionals that we could theoretically update.
            List<Professional> professionalsToUpdate = _accessQueries.GetAuthorization_ProfessionalWorkingHours_ReturnProsToUpdate(userId, workingHoursProfessionalIds, out authState);

            if (authState != AuthorizationState.CreateReadUpdate)
                return;

            // Note the submitting professional 
            Professional submittingPro = professionalsToUpdate.FirstOrDefault(i => i.ProfessionalUserId == userId);

            if (submittingPro == null)
                return;

            // we are not allowing deletions, so don't need to consider that case.
            int index = 0;
            foreach (var professionalWorkingHourToUpdate in professionalWorkingHoursToUpdate)
            {
                var idx = index;
                var matchingProfessional = professionalsToUpdate.FirstOrDefault(i => i.ProfessionalId == workingHoursProfessionalIds[idx]);

                if(matchingProfessional == null)
                {
                    index = index + 1;
                    continue;
                }

                ProfessionalWorkingHour matchingDbRow = null;

                if (matchingProfessional != null && matchingProfessional.ProfessionalWorkingHours != null)
                {
                    matchingDbRow = matchingProfessional.ProfessionalWorkingHours
                        .FirstOrDefault(i => i.ProfessionalWorkingHourId != new Guid() && 
                                            i.ProfessionalWorkingHourId == professionalWorkingHourToUpdate.ProfessionalWorkingHourId);
                }

                bool isInsert = false;
                if (matchingDbRow == null)
                {
                    // insert
                    isInsert = true;
                    matchingDbRow = new ProfessionalWorkingHour();

                    matchingDbRow.ProfessionalWorkingHourId = Guid.NewGuid();
                    matchingDbRow.CreatedDate = now;
                    if (userId == matchingProfessional.ProfessionalUserId)
                        matchingDbRow.CreatedByProfessionalId = matchingProfessional.ProfessionalId;
                    else
                        matchingDbRow.CreatedByProfessionalId = _unitOfWork.ProfessionalsRepository.Get(i => i.ProfessionalUserId == userId).First().ProfessionalId;
                }

                matchingDbRow.DayOfWeek = professionalWorkingHourToUpdate.DayOfWeek;
                matchingDbRow.StartTime = professionalWorkingHourToUpdate.StartTime;
                matchingDbRow.EndTime = professionalWorkingHourToUpdate.EndTime;
                matchingDbRow.IncludeBankHolidays = professionalWorkingHourToUpdate.IncludeBankHolidays;

                //delete + logging logic.
                if (!matchingDbRow.IsDeleted && professionalWorkingHourToUpdate.IsDeleted)
                {
                    matchingDbRow.IsDeleted = professionalWorkingHourToUpdate.IsDeleted;
                    matchingDbRow.DeletedByProfessionalId = submittingPro.ProfessionalId;
                    matchingDbRow.DeletedDate = now;
                }
                else if (matchingDbRow.IsDeleted && !professionalWorkingHourToUpdate.IsDeleted)
                {
                    matchingDbRow.IsDeleted = professionalWorkingHourToUpdate.IsDeleted;
                    matchingDbRow.DeletedByProfessionalId = null;
                    matchingDbRow.DeletedDate = null;
                }

                matchingDbRow.UpdatedByProfessionalId = submittingPro.ProfessionalId;
                matchingDbRow.UpdatedDate = now;

                matchingDbRow.CompanyLocationGroup = professionalWorkingHourToUpdate.CompanyLocationGroup;

                if(isInsert)
                    _unitOfWork.ProfessionalWorkingHoursRepository.Insert(matchingDbRow);
                else
                    _unitOfWork.ProfessionalWorkingHoursRepository.Update(matchingDbRow);

                index = index + 1;

            }//end foreach

            _unitOfWork.Save();
        }
    }
}
