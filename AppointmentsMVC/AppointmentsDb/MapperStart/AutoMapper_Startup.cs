using AppointmentsDb.Models;
using AppointmentsDb.ModelsDto;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentsDb.MapperStart
{
    public class Automapper_Startup
    {

        public IMapper StartAutomapper()
        {
            var config = new MapperConfiguration(cfg => {
                // Trouble with lists!
                // TRY TO GET THIS SO WE DON'T NEED TO FILL SEPARATELY (IF ONLY FOR NORMAL CLASSES IF NOT LISTS
                //.ForMember(dest => dest.Blog, opt => opt.MapFrom(src => src.Blog));


                cfg.CreateMap<Appointment, AppointmentForProUiDto>()
                    .ForMember(p => p.StartTime_Date, opt => opt.Ignore())
                    .ForMember(p => p.StartTime_Hour, opt => opt.Ignore())
                    .ForMember(p => p.StartTime_Minutes, opt => opt.Ignore())
                    .ForMember(p => p.AppointmentTypeAsDisplay, opt => opt.Ignore())
                    .ForMember(p => p.ProfessionalId, opt => opt.Ignore());
                cfg.CreateMap<AppointmentForProUiDto, Appointment>()
                    .ForMember(p => p.AuditNotes, opt => opt.Ignore())
                    .ForMember(p => p.Professional, opt => opt.Ignore());

                // Map to viewmodels here.
                cfg.CreateMap<Company, CompanyUiDto>()
                    .ForMember(p => p.UILoadOnly_IsUserCompanyOwner, opt => opt.Ignore());
                cfg.CreateMap<CompanyUiDto, Company>()
                    .ForMember(p => p.IsApproved, opt => opt.Ignore())
                    .ForMember(p => p.ApprovedDate, opt => opt.Ignore())
                    .ForMember(p => p.BannedDate, opt => opt.Ignore())
                    .ForMember(p => p.BannedReason, opt => opt.Ignore())
                    .ForMember(p => p.Notes, opt => opt.Ignore())
                    .ForMember(p => p.ApiLiveKey, opt => opt.Ignore())
                    .ForMember(p => p.ApiTestKey, opt => opt.Ignore())
                    .ForMember(p => p.IsDeleted, opt => opt.Ignore())
                    .ForMember(p => p.Owner, opt => opt.Ignore())
                    .ForMember(p => p.Professionals, opt => opt.Ignore());

                cfg.CreateMap<CompanyLocation, CompanyLocationUiDto>()
                    .ForMember(p => p.AuthState_OnlyTrustOnGeneration, opt => opt.Ignore());
                cfg.CreateMap<CompanyLocationUiDto, CompanyLocation>()
                    .ForMember(p => p.CreatedByProfessionalId, opt => opt.Ignore())
                    .ForMember(p => p.UpdatedByProfessionalId, opt => opt.Ignore())
                    .ForMember(p => p.UpdatedDate, opt => opt.Ignore())
                    .ForMember(p => p.DeletedByProfessionalId, opt => opt.Ignore())
                    .ForMember(p => p.DeletedDate, opt => opt.Ignore());

                cfg.CreateMap<CompanyLocationGroup, CompanyLocationGroupUiDto>()
                    .ForMember(p => p.AuthState_OnlyTrustOnGeneration, opt => opt.Ignore())
                    .ForMember(p => p.CompanyLocationUiDtos, opt => opt.Ignore());
                cfg.CreateMap<CompanyLocationGroupUiDto, CompanyLocationGroup>()
                    .ForMember(p => p.Company, opt => opt.Ignore())
                    .ForMember(p => p.CompanyLocations, opt => opt.Ignore())
                    .ForMember(p => p.CreatedByProfessional, opt => opt.Ignore())
                    .ForMember(p => p.DeletedByProfessionalId, opt => opt.Ignore())
                    .ForMember(p => p.DeletedDate, opt => opt.Ignore());


                cfg.CreateMap<LinkingKey, LinkingKeyUiDto>();
                cfg.CreateMap<LinkingKeyUiDto, LinkingKey>()
                    .ForMember(p => p.Owner, opt => opt.Ignore());

                cfg.CreateMap<Professional, ProfessionalUiDto>();
                cfg.CreateMap<ProfessionalUiDto, Professional>()
                    .ForMember(p => p.IsApproved, opt => opt.Ignore())
                    .ForMember(p => p.ApprovalDate, opt => opt.Ignore())
                    .ForMember(p => p.BannedDate, opt => opt.Ignore())
                    .ForMember(p => p.BannedReason, opt => opt.Ignore())
                    .ForMember(p => p.Notes, opt => opt.Ignore())
                    .ForMember(p => p.IsDeleted, opt => opt.Ignore())
                    .ForMember(p => p.CreatedDateTime, opt => opt.Ignore())
                    .ForMember(p => p.ProfessionalWorkingHours, opt => opt.Ignore());

                // TODO: MAP IGNORES!

                // REMEMBER THE DB HAS NOT BEEN UPDATED YET. Check the query string first!
                cfg.CreateMap<ProfessionalWorkingHour, ProfessionalWorkingHourUiDto>()
                    .ForMember(p => p.CompanyLocationGroupId, opt => opt.Ignore())
                    .ForMember(p => p.ProfessionalId, opt => opt.Ignore())
                    .ForMember(p => p.ProfessionalName, opt => opt.Ignore());
                cfg.CreateMap<ProfessionalWorkingHourUiDto, ProfessionalWorkingHour>()
                    .ForMember(p => p.CompanyLocationGroup, opt => opt.Ignore())
                    .ForMember(p => p.CreatedByProfessionalId, opt => opt.Ignore())
                    .ForMember(p => p.CreatedDate, opt => opt.Ignore())
                    .ForMember(p => p.UpdatedByProfessionalId, opt => opt.Ignore())
                    .ForMember(p => p.UpdatedDate, opt => opt.Ignore())
                    .ForMember(p => p.DeletedByProfessionalId, opt => opt.Ignore())
                    .ForMember(p => p.DeletedDate, opt => opt.Ignore());


            });

            config.AssertConfigurationIsValid();
            IMapper mapper = config.CreateMapper();

            return mapper;
        }

    }
}
