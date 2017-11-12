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
    public interface IProfessionalQueries
    {
        ProfessionalUiDto GetUiDto_ProfessionalFromUserGuid(string userIdString);
        Professional GetProfessionalFromUserGuid(string userIdString);
        void AddOrUpdateProfessional(string requestorUserId, Professional professional);
        void AddOrUpdateProfessionalFromDto(string requestorUserId, ProfessionalUiDto professionalDto);
    }


    public class ProfessionalQueries : IProfessionalQueries
    {
        IMapper _mapper;
        IUnitOfWork _unitOfWork;

        public ProfessionalQueries(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public ProfessionalUiDto GetUiDto_ProfessionalFromUserGuid(string userIdString)
        {
            Guid userId = GuidHelper.GetGuid(userIdString);
            ProfessionalUiDto returnValue = null;

            var pro = GetProfessionalFromUserGuid(userIdString);

            // strip back to the info we want.
            if (pro != null)
            {
                returnValue = _mapper.Map<Professional, ProfessionalUiDto>(pro);
            }

            return returnValue;
        }

        public Professional GetProfessionalFromUserGuid(string userIdString)
        {
            Guid userId = GuidHelper.GetGuid(userIdString);
            return _unitOfWork.ProfessionalsRepository.Get(i => i.ProfessionalUserId == userId).FirstOrDefault();
        }


        public void AddOrUpdateProfessionalFromDto(string userIdString, ProfessionalUiDto professionalDto)
        {
            Guid userId = GuidHelper.GetGuid(userIdString);

            var dbPro = GetProfessionalFromUserGuid(userIdString);

            // Validate!
            if (dbPro != null && (professionalDto.ProfessionalUserId != userId || professionalDto.ProfessionalId != dbPro.ProfessionalId))
            {
                throw new InvalidOperationException("Only the owner can set their professional details.");
            }

            bool isInsert = false;
            if (dbPro == null)
            {
                dbPro = new Professional();
                dbPro.CreatedDateTime = DateTime.Now;
                dbPro.ProfessionalId = Guid.NewGuid();
                isInsert = true;
            }

            dbPro.ProfessionalUserId = professionalDto.ProfessionalUserId;
            dbPro.Honorific = professionalDto.Honorific;
            dbPro.Forename = professionalDto.Forename;
            dbPro.MiddleName = professionalDto.MiddleName;
            dbPro.Surname = professionalDto.Surname;
            dbPro.Suffix = professionalDto.Suffix;
            dbPro.Gender = professionalDto.Gender;
            dbPro.EmailAddress = professionalDto.EmailAddress;
            dbPro.Telephone = professionalDto.Telephone;
            dbPro.TelephoneMobile = professionalDto.TelephoneMobile;
            dbPro.IsAvailableForAppointments = professionalDto.IsAvailableForAppointments;

            if (isInsert)
                _unitOfWork.ProfessionalsRepository.Insert(dbPro);
            else
                _unitOfWork.ProfessionalsRepository.Update(dbPro);

            _unitOfWork.Save();
        }


        public void AddOrUpdateProfessional(string userIdString, Professional professional)
        {
            Guid userId = GuidHelper.GetGuid(userIdString);

            var dbPro = GetProfessionalFromUserGuid(userIdString);

            // Validate!
            if (dbPro != null && (professional.ProfessionalUserId != userId || professional.ProfessionalId != dbPro.ProfessionalId))
            {
                throw new InvalidOperationException("Only the owner can set their professional details.");
            }

            if (dbPro == null)
            {
                dbPro = professional;
                _unitOfWork.ProfessionalsRepository.Insert(dbPro);
            }
            else
            {
                _unitOfWork.ProfessionalsRepository.Update(dbPro);
            }

            _unitOfWork.Save();

        }
    }
}
