using System;

namespace WebApi.Domain.Entities.Interfaces
{
    public interface IEntity
    {
        Guid Id { get; set; }
    }
}