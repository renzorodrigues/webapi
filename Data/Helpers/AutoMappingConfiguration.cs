using System;
using FluentNHibernate.Automapping;
using WebApi.Domain.Entities.Interfaces;

namespace WebApi.Data.Helpers
{
    public class AutoMappingConfiguration : DefaultAutomappingConfiguration
    {
        public override bool ShouldMap(Type type)
        {
            return type.GetInterface(typeof(IEntity).FullName) != null;
        }
    }
}