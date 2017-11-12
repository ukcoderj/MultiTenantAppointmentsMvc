using AppointmentsDb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentsDb.Pattern
{

    public class AppointmentsRepository : GenericRepository<Appointment>
    {
        public AppointmentsRepository(AppointmentsDbContext context) : base(context) { }

        public Appointment GetByGuid(Guid id, string includeProperties = "")
        {
            return Get(i => i.AppointmentId == id, includeProperties: includeProperties).FirstOrDefault();
        }


        public override void Delete(object id)
        {
            Appointment entityToDelete = dbSet.Find(id);
            entityToDelete.IsCancelled = true;
            entityToDelete.CancelledTime = DateTime.Now;
        }
    }


    public class CompaniesRepository : GenericRepository<Company>
    {
        public CompaniesRepository(AppointmentsDbContext context) : base(context) { }

        public Company GetByGuid(Guid id, string includeProperties = "")
        {
            return Get(i => i.CompanyId == id, includeProperties: includeProperties).FirstOrDefault();
        }


        public override void Delete(object id)
        {
            Company entityToDelete = dbSet.Find(id);
            entityToDelete.IsDeleted = true;
        }
    }


    public class CompanyLocationsRepository : GenericRepository<CompanyLocation>
    {
        public CompanyLocationsRepository(AppointmentsDbContext context) : base(context) { }

        public CompanyLocation GetByGuid(Guid id, string includeProperties = "")
        {
            return Get(i => i.CompanyLocationId == id, includeProperties: includeProperties).FirstOrDefault();
        }


        public override void Delete(object id)
        {
            CompanyLocation entityToDelete = dbSet.Find(id);
            entityToDelete.IsDeleted = true;
        }
    }


    public class CompanyLocationGroupsRepository : GenericRepository<CompanyLocationGroup>
    {
        public CompanyLocationGroupsRepository(AppointmentsDbContext context) : base(context) { }

        public CompanyLocationGroup GetByGuid(Guid id, string includeProperties = "")
        {
            return Get(i => i.CompanyLocationGroupId == id, includeProperties: includeProperties).FirstOrDefault();
        }


        public override void Delete(object id)
        {
            CompanyLocationGroup entityToDelete = dbSet.Find(id);
            entityToDelete.IsDeleted = true;
        }
    }



    public class ProfessionalsRepository : GenericRepository<Professional>
    {
        public ProfessionalsRepository(AppointmentsDbContext context) : base(context) { }

        public Professional GetByGuid(Guid id, string includeProperties = "")
        {
            return Get(i => i.ProfessionalId == id, includeProperties: includeProperties).FirstOrDefault();
        }


        public override void Delete(object id)
        {
            Professional entityToDelete = dbSet.Find(id);
            entityToDelete.IsDeleted = true;
        }
    }


    public class ProfessionalWorkingHoursRepository : GenericRepository<ProfessionalWorkingHour>
    {
        public ProfessionalWorkingHoursRepository(AppointmentsDbContext context) : base(context) { }

        public ProfessionalWorkingHour GetByGuid(Guid id, string includeProperties = "")
        {
            return Get(i => i.ProfessionalWorkingHourId == id, includeProperties: includeProperties).FirstOrDefault();
        }


        public override void Delete(object id)
        {
            ProfessionalWorkingHour entityToDelete = dbSet.Find(id);
            entityToDelete.IsDeleted = true;
        }
    }



}
