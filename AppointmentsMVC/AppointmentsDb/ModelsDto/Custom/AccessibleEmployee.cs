using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentsDb.ModelsDto.Custom
{
    public class AccessibleEmployee
    {
        public AccessibleEmployee(Guid id, string forename, string surname, bool isDefault)
        {
            Id = id;
            Forename = forename;
            Surname = surname;
            IsDefault = isDefault;
        }


        public Guid Id { get; set; }

        public string Forename { get; set; }

        public string Surname { get; set; }

        /// <summary>
        /// Is the currently logged in user.
        /// </summary>
        public bool IsDefault { get; set; }
    }
}
