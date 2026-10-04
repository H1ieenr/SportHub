using System;
using Shared.Exceptions;

namespace sporthub.domain
{
    public class OptionValue : AuditableEntity
    {
        public long option_definition_id { get; private set; }
        public string value { get; private set; } = "";
        public int display_order { get; private set; }
        public OptionDefinition option_definition { get; private set; } = null!;

        private OptionValue() { }

        public static OptionValue Create(long optionDefinitionId, string value, int displayOrder = 0)
        {
            if (optionDefinitionId <= 0)
                throw new ValidationException("optionDefinitionId không hợp lệ.");
            if (string.IsNullOrWhiteSpace(value))
                throw new ValidationException("Giá trị không được để trống.");

            return new OptionValue
            {
                option_definition_id = optionDefinitionId,
                value = value.Trim(),
                display_order = displayOrder
            };
        }

        public void Update(string value, int displayOrder)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ValidationException("Giá trị không được để trống.");
            this.value = value.Trim();
            display_order = displayOrder;
        }
    }
}