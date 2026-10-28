using AutoMapper;
using sporthub.app.contracts.sporthub.inventory;
using sporthub.domain;
namespace sporthub.app
{
    public class InventoryMapperProfile : Profile
    {
        public InventoryMapperProfile()
        {
            CreateMap<InventoryItem, InventoryItemDTO>()
                .ForMember(d => d.sku, o => o.MapFrom(s => s.product_variant.sku))
                .ForMember(d => d.variant_name, o => o.MapFrom(s => s.product_variant.name))
                .ForMember(d => d.product_id, o => o.MapFrom(s => s.product_variant.product_id))
                .ForMember(d => d.product_name, o => o.MapFrom(s => s.product_variant.product.name))
                .ForMember(d => d.is_low_stock, o => o.MapFrom(s => s.available_quantity <= s.reorder_level));

            CreateMap<InventoryTransaction, InventoryTransactionDTO>();
        }
    }
}
