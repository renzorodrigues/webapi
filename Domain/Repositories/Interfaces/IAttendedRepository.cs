using System.Collections.Generic;
using WebApi.Domain.Entities;

namespace WebApi.Domain.Repositories.Interfaces
{
    public interface IAttendedRepository : IRepositoryBase<Attended>
    {
        IEnumerable<Attended> GetByName(string search);
    }
}