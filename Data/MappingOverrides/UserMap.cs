using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using WebApi.Domain.Entities;

namespace WebApi.Data.MappingOverrides
{
    public class UserMap : IAutoMappingOverride<User>
    {
        public void Override(AutoMapping<User> mapping)
        {
            mapping.Map(x => x.Name).Not.Nullable();
            mapping.Map(x => x.Email).Not.Nullable().Unique();
            mapping.Map(x => x.PasswordHash).Not.Nullable();
            mapping.Map(x => x.PasswordSalt).Not.Nullable();
        }
    }
}