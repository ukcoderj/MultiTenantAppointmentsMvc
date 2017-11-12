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
    public interface IAppointmentQueries
    {
        List<AppointmentForProUiDto> GetUiDto_AppointmentsForProfessionalOrEmployee(string userIdString, string professionalIdToGetAppointmentsFor, DateTime dateTimeStartInclusive, DateTime dateTimeEndExclusive, bool includeCancelled = false);
        
        AppointmentForProUiDto GetUiDto_AppointmentByIdForProfessionalOrEmployee(string userIdString, string professionalIdToGetAppointmentFor, Guid appointmentId, bool includeCancelled = false);

        void AddOrUpdateAppointmentFromDto(string requestorUserId, string professionalIdForAppointment, AppointmentForProUiDto appointmentDto);
    }


    public class AppointmentQueries : IAppointmentQueries
    {
        IMapper _mapper;
        IUnitOfWork _unitOfWork;
        IAccessQueries _accessQueries;

        public AppointmentQueries(IUnitOfWork unitOfWork, IMapper mapper, IAccessQueries accessQueries)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _accessQueries = accessQueries;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="userIdString"></param>
        /// <param name="professionalIdToGetAppointmentsFor">Use a blank string if the user is the pro</param>
        /// <param name="dateTimeStartInclusive"></param>
        /// <param name="dateTimeEndExclusive"></param>
        /// <returns></returns>
        public List<AppointmentForProUiDto> GetUiDto_AppointmentsForProfessionalOrEmployee(string userIdString, string professionalIdToGetAppointmentsFor, DateTime dateTimeStartInclusive, DateTime dateTimeEndExclusive, bool includeCancelled = false)
        {
            Guid userId = GuidHelper.GetGuid(userIdString);
            Guid targetProfessionalId = GuidHelper.GetGuid(professionalIdToGetAppointmentsFor);
            List<Appointment> proAppointments = new List<Appointment>();
            List<AppointmentForProUiDto> returnValue = new List<AppointmentForProUiDto>();

            // Does the pro have access?
            var hasAccess = _accessQueries.DoesUserProfessionalHaveAccessToProfessional_Appointments(userId, targetProfessionalId);

            // If the user has access, get the appointments.
            if(hasAccess == true)
            {
                proAppointments = GetAppointmentsForProfessionalId(targetProfessionalId, dateTimeStartInclusive, dateTimeEndExclusive, includeCancelled);
            }

            if (proAppointments != null && proAppointments.Any())
            {
                foreach (var appt in proAppointments)
                {
                    AppointmentForProUiDto uiRow = _mapper.Map<Appointment, AppointmentForProUiDto>(appt);
                    // Remember to udpate the UI properties!
                    uiRow.FillAdditionalUiProperties();
                    uiRow.ProfessionalId = appt.Professional.ProfessionalId;
                    returnValue.Add(uiRow);
                }
            }

            return returnValue;
        }





        /// <summary>
        /// Get the appointment to view/update
        /// </summary>
        /// <param name="userIdString"></param>
        /// <param name="professionalIdToGetAppointmentFor">Use a blank string if the user is the pro</param>
        /// <param name="appointmentId"></param>
        /// <returns></returns>
        public AppointmentForProUiDto GetUiDto_AppointmentByIdForProfessionalOrEmployee(string userIdString, string professionalIdToGetAppointmentFor, Guid appointmentId, bool includeCancelled = false)
        {
            AppointmentForProUiDto uiDto = null;
            Guid userId = GuidHelper.GetGuid(userIdString);
            Guid targetProfessionalId = GuidHelper.GetGuid(professionalIdToGetAppointmentFor);

            var hasAccess = _accessQueries.DoesUserProfessionalHaveAccessToProfessional_Appointments(userId, targetProfessionalId);

            if(hasAccess)
            {
                var appointment = _unitOfWork.AppointmentsRepository.Get(i => i.Professional.ProfessionalId == targetProfessionalId && i.AppointmentId == appointmentId && (includeCancelled || i.IsCancelled == false)).FirstOrDefault();
                if (appointment != null)
                {
                    uiDto = _mapper.Map<Appointment, AppointmentForProUiDto>(appointment);
                    uiDto.ProfessionalId = appointment.Professional.ProfessionalId;
                    uiDto.FillAdditionalUiProperties();
                }
            }

            return uiDto;
        }



        /// <summary>
        /// WARNING: Never use this directly! It must be validated before use!
        /// </summary>
        /// <param name="userIdString"></param>
        /// <param name="dateTimeStartInclusive"></param>
        /// <param name="dateTimeEndExclusive"></param>
        /// <returns></returns>
        private List<Appointment> GetAppointmentsForProfessionalId(Guid professionalId, DateTime dateTimeStartInclusive, DateTime dateTimeEndExclusive, bool includeCancelled = false)
        {
            var appointments = _unitOfWork.AppointmentsRepository.Get(i => i.Professional.ProfessionalId == professionalId && i.StartTime >= dateTimeStartInclusive && i.EndTime < dateTimeEndExclusive && (includeCancelled || i.IsCancelled == false))
                .OrderBy(i => i.StartTime).ToList();

            return appointments;
        }


        /// <summary>
        /// Add Or UpdateAppointment from the Dto
        /// </summary>
        /// <param name="requestorUserId"></param>
        /// <param name="professionalIdForAppointment">Use a blank string if the user is the pro</param>
        /// <param name="appointmentDto"></param>
        public void AddOrUpdateAppointmentFromDto(string requestorUserId, string professionalIdForAppointment, AppointmentForProUiDto appointmentDto)
        {
            Guid userId = GuidHelper.GetGuid(requestorUserId);
            Guid targetProfessionalId = GuidHelper.GetGuid(professionalIdForAppointment);

            // 1. Validation
            var hasAccess = _accessQueries.DoesUserProfessionalHaveAccessToProfessional_Appointments(userId, targetProfessionalId);

            if(!hasAccess)
            {
                throw new InvalidOperationException($"The requestor {requestorUserId} does not have the privileges to update {professionalIdForAppointment}");
            }

            // 2. Update
            // update with the new values.
            appointmentDto.UpdateTimesFromAdditionalUiProperties();

            var dbAppointment = _unitOfWork.AppointmentsRepository.Get(i => i.AppointmentId == appointmentDto.AppointmentId, includeProperties: "Professional").FirstOrDefault();


            bool isInsert = false;
            var professional = _unitOfWork.ProfessionalsRepository.Get(i => i.ProfessionalId == appointmentDto.ProfessionalId).FirstOrDefault();

            // new appointment
            if (dbAppointment == null)
            {
                dbAppointment = _mapper.Map<AppointmentForProUiDto, Appointment>(appointmentDto);                
                dbAppointment.AppointmentId = Guid.NewGuid();
                dbAppointment.CreatedBy = Shared.Enums.UserType.Professional;
                dbAppointment.CreatedById = professional.ProfessionalId;
                dbAppointment.CreatedTime = DateTime.Now;
                dbAppointment.CancelledTime = new DateTime(2000, 1, 1); //datetime2 error hack.
                dbAppointment.Forename = Shared.Helpers.StringHelpers.FirstCharToUpper(dbAppointment.Forename);
                dbAppointment.Surname = Shared.Helpers.StringHelpers.FirstCharToUpper(dbAppointment.Surname);
                if (!String.IsNullOrEmpty(dbAppointment.EmailAddress))
                {
                    dbAppointment.EmailAddress = dbAppointment.EmailAddress.Trim().ToLower();
                }

                isInsert = true;
            }
            else
            {
                if (!dbAppointment.IsCancelled && appointmentDto.IsCancelled)
                {
                    dbAppointment.IsCancelled = true;
                    dbAppointment.CancelledBy = Shared.Enums.UserType.Professional;
                    dbAppointment.CancelledTime = DateTime.Now;
                    //TODO: Check need to trim if too long.
                    dbAppointment.AuditNotes = (dbAppointment.AuditNotes +
                                        "Cancelled by professional at " + DateTime.Now.ToLongTimeString());
                }

                if (!dbAppointment.IsCancelled && appointmentDto.IsCancelled)
                {
                    dbAppointment.IsCancelled = false;
                    dbAppointment.CancelledBy = Shared.Enums.UserType.NA;
                    dbAppointment.CancelledTime = new DateTime();
                    //TODO: Check need to trim if too long.
                    dbAppointment.AuditNotes = (dbAppointment.AuditNotes +
                                        "UN-Cancelled by professional at " + DateTime.Now.ToLongTimeString());
                }

                
                dbAppointment.AppointmentType = appointmentDto.AppointmentType;
                dbAppointment.Forename = Shared.Helpers.StringHelpers.FirstCharToUpper(appointmentDto.Forename);
                dbAppointment.Surname = Shared.Helpers.StringHelpers.FirstCharToUpper(appointmentDto.Surname);
                dbAppointment.StartTime = appointmentDto.StartTime;
                dbAppointment.EndTime = appointmentDto.EndTime;
                dbAppointment.DurationInMinutes = appointmentDto.DurationInMinutes;
                dbAppointment.TelephoneMobile = appointmentDto.TelephoneMobile;
                dbAppointment.Telephone = appointmentDto.Telephone;
                if (!String.IsNullOrEmpty(appointmentDto.EmailAddress))
                {
                    dbAppointment.EmailAddress = appointmentDto.EmailAddress.Trim().ToLower();
                }
                dbAppointment.ProfessionalNotes = appointmentDto.ProfessionalNotes;
            }

            // Assign the pro.
            dbAppointment.Professional = professional;

            if (isInsert)
                _unitOfWork.AppointmentsRepository.Insert(dbAppointment);
            else
                _unitOfWork.AppointmentsRepository.Update(dbAppointment);


            _unitOfWork.Save();

        }
    }
}
