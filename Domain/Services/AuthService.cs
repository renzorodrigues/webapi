using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using WebApi.Data.Helpers.Interfaces;
using WebApi.Domain.Entities;
using WebApi.Domain.Repositories.Interfaces;
using WebApi.Domain.Services.Interfaces;

namespace WebApi.Domain.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IRepositoryBase<User> _userRepository;
        private readonly IConfiguration _config;
        private readonly IUnitOfWork _unitOfWork;

        public AuthService(
            IAuthRepository authRepository,
            IRepositoryBase<User> userRepository,
            IConfiguration config,
            IUnitOfWork unitOfWork)
        {
            this._authRepository = authRepository;
            this._userRepository = userRepository;
            this._config = config;
            this._unitOfWork = unitOfWork;
        }

        public string Authenticate(User credentials)
        {
            User obj = this._authRepository.Authenticate(credentials);

            if (obj != null)
            {
                var hashedPassword = this.CreateHash(credentials.Password, obj.PasswordSalt);

                if (Convert.ToBase64String(hashedPassword) == Convert.ToBase64String(obj.PasswordHash))
                    return this.GenerateToken(obj);
            }

            return null;
        }

        public bool Register(User credentials)
        {
            if (credentials != null)
            {
                credentials.Id = Guid.NewGuid();

                byte[] salt = this.CreateSalt(10);
                byte[] hash = this.CreateHash(credentials.Password, salt);

                credentials.PasswordSalt = salt;
                credentials.PasswordHash = hash;
                credentials.Password = null;

                this._unitOfWork.BeginTransaction();

                var user = this._userRepository.Insert(credentials);

                this._unitOfWork.Commit();

                if (user)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        private byte[] CreateSalt(int size)
        {
            byte[] salt = new byte[size];

            using(var rgb = RNGCryptoServiceProvider.Create())
            {
                rgb.GetBytes(salt);
            }

            return salt;
        }

        private byte[] CreateHash(string password, byte[] salt)
        {
            byte[] hash;
            string passwordSalted = password + salt;

            using(var hmac = new SHA256CryptoServiceProvider())
            {
                hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(passwordSalted));
            }

            return hash;
        }

        private string GenerateToken(User credentials)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, credentials.Id.ToString()),
                new Claim(ClaimTypes.Name, credentials.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("ApplicationSettings:JWT_Secret").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }
    }
}