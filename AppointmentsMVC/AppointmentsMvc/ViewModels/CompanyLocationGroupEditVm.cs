using AppointmentsDb.ModelsDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppointmentsMvc.ViewModels
{
    public class CompanyLocationGroupEditVm : ValidationMsg
    {
        public CompanyLocationGroupUiDto CompanyLocationGroupUiDto { get; set; }

    }
}