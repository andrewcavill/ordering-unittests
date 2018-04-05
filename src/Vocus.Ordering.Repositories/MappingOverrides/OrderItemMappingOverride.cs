using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using Vocus.Ordering.Entities;

namespace Vocus.Ordering.Repositories.MappingOverrides
{
    public class OrderItemMappingOverride : IAutoMappingOverride<OrderItem>
    {
        public void Override(AutoMapping<OrderItem> mapping)
        {
            mapping.References(x => x.Product);
        }
    }
}