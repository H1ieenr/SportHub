using Shared.Exceptions;

namespace sporthub.domain
{
    public class CategoryOption : AuditableEntity
    {
        public long category_id { get; private set; }
        public long option_definition_id { get; private set; }
        public int? display_order { get; private set; }
        public Category category { get; private set; } = null!;
        public OptionDefinition option_definition { get; private set; } = null!;


        private CategoryOption() { }

        public static CategoryOption Create(long categoryId, long optionDefinitionId, int? displayOrder = null)
        {
            if (categoryId == null || categoryId < 0)
                throw new BadRequestException("categoryId không được để trống");


            return new CategoryOption
            {
                category_id = categoryId,
                option_definition_id = optionDefinitionId,
                display_order = displayOrder
            };
        }

        public void Update(long categoryId, int? displayOrder = null)
        {
            category_id = categoryId;
            display_order = displayOrder;
        }
    }
}