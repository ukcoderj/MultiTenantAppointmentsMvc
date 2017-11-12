using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentsDb.Helpers
{
    public static class GuidHelper
    {
        public static Guid GetGuid(string guidString)
        {
            Guid returnValue;
            Guid.TryParse(guidString, out returnValue);
            return returnValue;
        }
    }
}
