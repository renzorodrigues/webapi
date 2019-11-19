using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Domain.Entities.Interfaces;

namespace WebApi.Domain.Repositories.Interfaces
{
    public interface IRepositoryBase<T> where T : IEntity
    {
        IEnumerable<T> GetAll();
        T GetById(Guid id);
        bool Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}