using System;
using WebApi.Domain.Entities.Enums;
using WebApi.Domain.Entities.Interfaces;

namespace WebApi.Domain.Entities
{
    public class Tutor : IEntity
    {
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
        public virtual TutorType? TutorType { get; set; }
    }
}