using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using WebApi.Domain.Entities;

namespace WebApi.Data.MappingOverrides
{
    public class ContactMap : IAutoMappingOverride<Contact>
    {
        public void Override(AutoMapping<Contact> mapping) { }
    }
}