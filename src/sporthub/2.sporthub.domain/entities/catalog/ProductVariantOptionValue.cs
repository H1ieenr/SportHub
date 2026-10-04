using System;
using Shared.Exceptions;

namespace sporthub.domain
{
    public class ProductVariantOptionValue : AuditableEntity
    {
        public long product_variant_id { get; private set; }
        public long option_definition_id { get; private set; }
        public long option_value_id { get; private set; }

        public ProductVariant product_variant { get; private set; } = null!;
        public OptionDefinition option_definition { get; private set; } = null!;
        public OptionValue option_value { get; private set; } = null!;

        private ProductVariantOptionValue() { }

        public static ProductVariantOptionValue Create(long productVariantId, long optionDefinitionId, long optionValueId)
        {
            if (productVariantId <= 0 || optionDefinitionId <= 0 || optionValueId <= 0)
                throw new ValidationException("Dữ liệu option của biến thể không hợp lệ.");

            return new ProductVariantOptionValue
            {
                product_variant_id = productVariantId,
                option_definition_id = optionDefinitionId,
                option_value_id = optionValueId
            };
        }
    }
}