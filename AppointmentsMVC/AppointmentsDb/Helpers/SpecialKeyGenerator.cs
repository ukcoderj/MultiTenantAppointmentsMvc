using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentsDb.Helpers
{
    public static class SpecialKeyGenerator
    {
        public static string CreateSpecialKey()
        {
            var g1 = Guid.NewGuid();
            var g2 = Guid.NewGuid();
            return g1.ToString().Replace("-", "") + g2.ToString().Replace("-", "");
        }
    }
}
