using System;
using System.Collections.Generic;
using WebApi.Domain.Entities;

namespace WebApi.Domain.Services.Interfaces
{
    public interface IAttendedService
    {
        IEnumerable<Attended> GetAll();
        Attended GetById(Guid id);
        bool Insert(Attended attended);
        bool Update(Guid id, Attended attended);
        void Delete(Guid id);
        IEnumerable<Attended> GetByName(string name);
        IEnumerable<Attended> Pagination(int rowNumber, int page);
    }
}