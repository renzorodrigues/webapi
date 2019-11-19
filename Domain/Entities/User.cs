using System;
using WebApi.Domain.Entities.Interfaces;

namespace WebApi.Domain.Entities
{
    public class User : IEntity
    {
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Email { get; set; }
        public virtual string Password { get; set; }
        public virtual byte[] PasswordHash { get; set; }
        public virtual byte[] PasswordSalt { get; set; } 
    }
}