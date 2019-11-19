using WebApi.Domain.Entities;

namespace WebApi.Domain.Repositories.Interfaces
{
    public interface IAuthRepository : IRepositoryBase<User>
    {
         User Authenticate(User credentials);
    }
}