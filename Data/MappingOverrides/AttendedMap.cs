using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using WebApi.Domain.Entities;

namespace WebApi.Data.MappingOverrides
{
    public class AttendedMap : IAutoMappingOverride<Attended>
    {
        public void Override(AutoMapping<Attended> mapping)
        {
            mapping.Map(x => x.Name).Not.Nullable();
            mapping.Map(x => x.RegistrationNumber).Not.Nullable().Unique();
            mapping.Map(x => x.Gender).Not.Nullable();
            mapping.Map(x => x.BirthDate).Not.Nullable();
            mapping.Map(x => x.RegistrationDate).Not.Nullable();
            mapping.References(x => x.Contact).Not.LazyLoad().Cascade.All();
            mapping.References(x => x.Tutor).Not.LazyLoad().Cascade.All();
        }
    }
}