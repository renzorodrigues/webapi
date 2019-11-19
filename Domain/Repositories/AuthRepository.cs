using System;
using System.Collections.Generic;
using System.Linq;
using WebApi.Data.Helpers;
using WebApi.Data.Helpers.Interfaces;
using WebApi.Domain.Entities;
using WebApi.Domain.Repositories.Interfaces;

namespace WebApi.Domain.Repositories
{
    public class AuthRepository : RepositoryBase<User>, IAuthRepository
    {
        private readonly UnitOfWork _unitOfWork;

        public AuthRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            this._unitOfWork = (UnitOfWork) unitOfWork;
        }
        public User Authenticate(User credentials)
        {
            var result = this._unitOfWork.Session.Query<User>()
            .Where(x => x.Email == credentials.Email);
            
            if (result.Any())
            {
                return result.FirstOrDefault();
            } 
            else
                return null;
        }
    }
}