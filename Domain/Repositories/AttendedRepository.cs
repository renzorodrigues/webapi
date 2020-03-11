using System.Collections.Generic;
using System.Linq;
using ReflectionIT.Mvc.Paging;
using WebApi.Data.Helpers;
using WebApi.Data.Helpers.Interfaces;
using WebApi.Domain.Entities;
using WebApi.Domain.Repositories.Interfaces;

namespace WebApi.Domain.Repositories
{
    public class AttendedRepository : RepositoryBase<Attended>, IAttendedRepository
    {
        private readonly UnitOfWork _unitOfWork;
        public AttendedRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            this._unitOfWork = (UnitOfWork)unitOfWork;
        }
        public IEnumerable<Attended> GetByName(string search)
        {
            var attendeds = this._unitOfWork.Session.Query<Attended>()
            .Where(x => x.Name.Contains(search) || x.RegistrationNumber.ToString() == search);
            return attendeds;
        }

        // TODO
        public IEnumerable<Attended> Pagination(int rowNumber, int page)
        {
            var attendeds = this._unitOfWork.Session.Query<Attended>()
            .OrderBy(x => x.Name);
            return PagingList.Create(attendeds, rowNumber, page);
        }
    }
}