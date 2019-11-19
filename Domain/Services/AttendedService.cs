using System;
using System.Collections.Generic;
using System.Linq;
using WebApi.Data.Helpers.Interfaces;
using WebApi.Domain.Entities;
using WebApi.Domain.Repositories.Interfaces;
using WebApi.Domain.Services.Interfaces;

namespace WebApi.Domain.Services
{
    public class AttendedService : IAttendedService
    {
        private readonly IRepositoryBase<Attended> _repository;
        private readonly IAttendedRepository _attendedRepository;
        private readonly IRepositoryBase<Contact> _contactRepository;
        private readonly IRepositoryBase<Tutor> _tutorRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AttendedService(
            IRepositoryBase<Attended> repository,
            IAttendedRepository attendedRepository,
            IRepositoryBase<Contact> contactRepository,
            IRepositoryBase<Tutor> tutorRepository,
            IUnitOfWork unitOfWork)
        {
            this._repository = repository;
            this._attendedRepository = attendedRepository;
            this._contactRepository = contactRepository;
            this._tutorRepository = tutorRepository;
            this._unitOfWork = unitOfWork;
        }

        public IEnumerable<Attended> GetAll()
        {
            return this._repository.GetAll().ToList();
        }

        public Attended GetById(Guid id)
        {
            return this._repository.GetById(id);
        }

        public IEnumerable<Attended> GetByName(string search)
        {
            return this._attendedRepository.GetByName(search);
        }

        public bool Insert(Attended attended)
        {
            if (attended != null && attended.Contact != null && attended.Tutor != null)
            {
                this._unitOfWork.BeginTransaction();
                
                attended.Contact.Id = Guid.NewGuid();
                var contactReturned = this._contactRepository.Insert(attended.Contact);

                attended.Tutor.Id = Guid.NewGuid();
                var tutorReturned = this._tutorRepository.Insert(attended.Tutor);

                attended.Id = Guid.NewGuid();
                var attendedReturned = this._repository.Insert(attended);

                this._unitOfWork.Commit();

                if (contactReturned && tutorReturned && attendedReturned)
                    return true;
            }

            return false;
        }

        public bool Update(Guid id, Attended newAttended)
        {
            this._unitOfWork.BeginTransaction();
            
            Attended attended = this._repository.GetById(id);
            
            if (this.UpdateData(attended, newAttended))
            {
                this._repository.Update(attended);
                this._unitOfWork.Commit();
                return true;
            }
            else
                return false;     
        }

        public void Delete(Guid id)
        {
            this._unitOfWork.BeginTransaction();

            Attended attended = this._repository.GetById(id);

            this._repository.Delete(attended);

            this._unitOfWork.Commit();
        }

        private bool UpdateData(Attended attended, Attended newAttended)
        {
            if (attended != null)
            {
                attended.Name = newAttended.Name;
                attended.RegistrationNumber = newAttended.RegistrationNumber;
                attended.Gender = newAttended.Gender;
                attended.BirthDate = newAttended.BirthDate;
                attended.RegistrationDate = newAttended.RegistrationDate;
            }
            
            if (attended.Contact != null)
            {
                attended.Contact.TelephoneNumber = newAttended.Contact.TelephoneNumber;
                attended.Contact.MobileNumber = newAttended.Contact.MobileNumber;
                attended.Contact.Email = newAttended.Contact.Email;
            }

            if (attended.Tutor != null)
            {
                attended.Tutor.Name = newAttended.Tutor.Name;
                attended.Tutor.TutorType = newAttended.Tutor.TutorType;
            }

            return this.ValidateField(attended);
        }

        private bool ValidateField(Attended attended)
        {
            if (string.IsNullOrEmpty(attended.Name))
                return false;
            if ( attended.RegistrationNumber == null)
                return false;
            if (!attended.BirthDate.HasValue)
                return false;
            if (!attended.RegistrationDate.HasValue)
                return false;
            if (attended.Gender == null)
                return false;

            return true;
        }
    }
}