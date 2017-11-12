using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AppointmentsDb.Models;
using AppointmentsDb.Queries;
using AutoMapper;
using AppointmentsDb.Pattern;
using Microsoft.AspNet.Identity;
using AppointmentsDb.ModelsDto;
using AppointmentsMvc.ViewModels;
using Shared.Enums;

namespace AppointmentsMvc.Controllers
{
    [Authorize]
    public class CompanyLocationGroupsController : Controller
    {
        AppointmentsDbContext _context;
        IMapper _mapper;
        IUnitOfWork _unitOfWork;
        IAccessQueries _accessQueries;
        ICompanyQueries _companyQueries;
        ICompanyLocationGroupQueries _companyLocationGroupQueries;


        public CompanyLocationGroupsController()
        {
            _context = new AppointmentsDb.Models.AppointmentsDbContext();
            _unitOfWork = new AppointmentsDb.Pattern.UnitOfWork(_context);
            _mapper = new AppointmentsDb.MapperStart.Automapper_Startup().StartAutomapper();

            _companyQueries = new CompanyQueries(_unitOfWork, _mapper);
            _accessQueries = new AccessQueries(_unitOfWork, _companyQueries, _mapper);
            
            _companyLocationGroupQueries = new CompanyLocationGroupQueries(_unitOfWork, _mapper, _accessQueries, _companyQueries);
        }


        // GET: CompanyLocationGroups
        public async Task<ActionResult> Index()
        {
            AuthorizationState authState = AuthorizationState.NotAllowed;
            var companyLocationGroupUiDtos = await Task.Run(() => _companyLocationGroupQueries.GetUiDto_CompanyLocationGroups(User.Identity.GetUserId(), out authState));

            if (authState == AuthorizationState.NotAllowed)
            {
                // Insufficient rights, bounce them.
                return RedirectToAction("Index", "UserCannotAccess");
            }

            if (companyLocationGroupUiDtos != null && companyLocationGroupUiDtos.Any())
            {
                companyLocationGroupUiDtos = companyLocationGroupUiDtos.OrderBy(i => !i.IsDeleted).ThenByDescending(i => i.CreatedDate).ToList();
            }

            return View(companyLocationGroupUiDtos);
        }



        public async Task<ActionResult> AddEdit(Guid? id)
        {
            CompanyLocationGroupEditVm returnValue = new CompanyLocationGroupEditVm();

            if(id == null)
            {
                //create a starting point here.                
                returnValue.CompanyLocationGroupUiDto = new CompanyLocationGroupUiDto();
                returnValue.CompanyLocationGroupUiDto.CompanyLocationUiDtos = new List<CompanyLocationUiDto>();
                CompanyLocationUiDto cluiDto = new CompanyLocationUiDto();
                returnValue.CompanyLocationGroupUiDto.CompanyLocationUiDtos.Add(cluiDto);                              
                AuthorizationState authState = AuthorizationState.NotAllowed;
                bool isCompanyOwner = false;
                // check for some form of rights before showing the form.
                var company = _accessQueries.GetAuthorization_ForCompanyAdmin_IfCompanyIdSelectedByUserId(Guid.Parse(User.Identity.GetUserId()), false, out authState, out isCompanyOwner);
                returnValue.CompanyLocationGroupUiDto.AuthState_OnlyTrustOnGeneration = authState;

            }
            else
            {
                returnValue.CompanyLocationGroupUiDto = _companyLocationGroupQueries.GetUiDto_CompanyLocationGroupById(User.Identity.GetUserId(), (Guid)id);
            }

            if(returnValue.CompanyLocationGroupUiDto == null)
            {
                return RedirectToAction("Index", "CompanyLocationGroups");
            }

            return View(returnValue);
        }

        //TODO: Review Overposting Risk [Bind(Include = "CompanyLocationGroupId,CompanyLocationGroupIndex,LocationGroupName,CreatedDate,IsDeleted,DeletedByProfessionalId,DeletedDate,HasProfessionalVisitsClientLocations")] 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddEdit(CompanyLocationGroupEditVm companyLocationGroupUiDtoVm)
        {
            if (ModelState.IsValid)
            {
                // Check row validations.
                companyLocationGroupUiDtoVm.ValidationMessage = "";
                int locationNum = 1;
                foreach(var row in companyLocationGroupUiDtoVm.CompanyLocationGroupUiDto.CompanyLocationUiDtos)
                {
                    var rowMsg = row.Validate();
                    if(!String.IsNullOrWhiteSpace(rowMsg))
                    {
                        companyLocationGroupUiDtoVm.ValidationMessage = companyLocationGroupUiDtoVm.ValidationMessage + "Location " + locationNum + ": " + rowMsg + "\r\n";
                    }
                    locationNum++;
                }

                if(!String.IsNullOrWhiteSpace(companyLocationGroupUiDtoVm.ValidationMessage))
                {
                    return View(companyLocationGroupUiDtoVm);
                }

                // Add or update
                _companyLocationGroupQueries.FromUiDto_AddOrUpdateCompanyLocationGroup(User.Identity.GetUserId(), companyLocationGroupUiDtoVm.CompanyLocationGroupUiDto);

                return RedirectToAction("Index", "CompanyLocationGroups");
            }
            else
            {
                return View(companyLocationGroupUiDtoVm);
            }

            
        }

        public PartialViewResult AddCompanyLocation()
        {
            var model = new CompanyLocationUiDto();
            return PartialView("_companyLocationPartial", model);
        }


        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
