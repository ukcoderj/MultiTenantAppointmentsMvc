using AppointmentsDb.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentsDb.Pattern
{
    public interface IUnitOfWork
    {
        // Generic repos
        GenericRepository<LinkingKey> LinkingKeyRepository { get; }
        // Guid override repos
        AppointmentsRepository AppointmentsRepository { get; }
        CompaniesRepository CompaniesRepository { get; }
        CompanyLocationsRepository CompanyLocationsRepository { get; }
        CompanyLocationGroupsRepository CompanyLocationGroupsRepository { get; }
        ProfessionalsRepository ProfessionalsRepository { get; }
        ProfessionalWorkingHoursRepository ProfessionalWorkingHoursRepository { get; }
        void Save();
    }


    public class UnitOfWork : IUnitOfWork
    {
        private AppointmentsDbContext _context;
        private AppointmentsRepository _appointmentsRepository;
        private CompaniesRepository _companiesRepository;
        private CompanyLocationsRepository _companyLocationsRepository;
        private CompanyLocationGroupsRepository _companyLocationGroupsRepository;
        private GenericRepository<LinkingKey> _linkingKeyRepository;
        private ProfessionalsRepository _professionalsRepository;
        private ProfessionalWorkingHoursRepository _professionalWorkingHoursRepository;

        public UnitOfWork(AppointmentsDbContext context)
        {
            _context = context;
        }


        public AppointmentsRepository AppointmentsRepository
        {
            get
            {
                if (this._appointmentsRepository == null)
                {
                    this._appointmentsRepository = new AppointmentsRepository(_context);
                }
                return _appointmentsRepository;
            }
        }

        public CompaniesRepository CompaniesRepository
        {
            get
            {
                if (this._companiesRepository == null)
                {
                    this._companiesRepository = new CompaniesRepository(_context);
                }
                return _companiesRepository;
            }
        }

        public CompanyLocationsRepository CompanyLocationsRepository
        {
            get
            {
                if (this._companyLocationsRepository == null)
                {
                    this._companyLocationsRepository = new CompanyLocationsRepository(_context);
                }
                return _companyLocationsRepository;
            }
        }

        public CompanyLocationGroupsRepository CompanyLocationGroupsRepository
        {
            get
            {
                if (this._companyLocationGroupsRepository == null)
                {
                    this._companyLocationGroupsRepository = new CompanyLocationGroupsRepository(_context);
                }
                return _companyLocationGroupsRepository;
            }
        }

        public GenericRepository<LinkingKey> LinkingKeyRepository
        {
            get
            {
                if (this._linkingKeyRepository == null)
                {
                    this._linkingKeyRepository = new GenericRepository<LinkingKey>(_context);
                }
                return _linkingKeyRepository;
            }
        }

        public ProfessionalsRepository ProfessionalsRepository
        {
            get
            {
                if (this._professionalsRepository == null)
                {
                    this._professionalsRepository = new ProfessionalsRepository(_context);
                }
                return _professionalsRepository;
            }
        }

        public ProfessionalWorkingHoursRepository ProfessionalWorkingHoursRepository
        {
            get
            {
                if (this._professionalWorkingHoursRepository == null)
                {
                    this._professionalWorkingHoursRepository = new ProfessionalWorkingHoursRepository(_context);
                }
                return _professionalWorkingHoursRepository;
            }
        }



        #region "Save/Dispose"

        public void Save()
        {
            try
            {
                // Your code...
                // Could also be before try if you know the exception occurs in SaveChanges

                _context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw e;
            }
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
