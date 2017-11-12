using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppointmentsMvc.ViewModels
{
    public class StatusVM
    {
        public StatusVM()
        {

        }

        public StatusVM(bool isSuccess, string successMessage, string failMessage)
        {
            IsSuccess = isSuccess;
            SuccessMessage = successMessage;
            FailMessage = failMessage;
        }

        public bool IsSuccess { get; set; }
        public string SuccessMessage { get; set; }
        public string FailMessage { get; set; }
    }
}