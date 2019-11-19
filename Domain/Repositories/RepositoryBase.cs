using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Linq;
using WebApi.Data.Helpers;
using WebApi.Data.Helpers.Interfaces;
using WebApi.Domain.Entities.Interfaces;
using WebApi.Domain.Repositories.Interfaces;

namespace WebApi.Domain.Repositories
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : IEntity
    {
        private readonly UnitOfWork _unitOfWork;
        public RepositoryBase(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = (UnitOfWork)unitOfWork;
        }

        protected ISession Session { get { return _unitOfWork.Session; } }

        public IEnumerable<T> GetAll()
        {
            return this.Session.Query<T>();
        }

        public T GetById(Guid id)
        {
            return this.Session.Get<T>(id);
        }

        public bool Insert(T entity)
        {
            var result = this.Session.Save(entity);

            if (result != null)
                return true;
            else
                return false;
        }

        public void Update(T entity)
        {
            this.Session.Update(entity);
        }

        public void Delete(T entity)
        {
            this.Session.Delete(entity);
        }
    }
}