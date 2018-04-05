using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using Vocus.Ordering.Entities;

namespace Vocus.Ordering.Repositories.MappingOverrides
{
    public class OrderMappingOverride : IAutoMappingOverride<Order>
    {
        public void Override(AutoMapping<Order> mapping)
        {
            mapping.References(x => x.Brand);
            mapping.HasMany(x => x.OrderItems).Inverse().Cascade.AllDeleteOrphan();
        }
    }
}