using Microsoft.IdentityModel.Tokens;
using WebApi.Domain.Entities;

namespace WebApi.Domain.Services.Interfaces
{
    public interface IAuthService
    {
         string Authenticate(User credentials);
         bool Register(User credentials);
    }
}