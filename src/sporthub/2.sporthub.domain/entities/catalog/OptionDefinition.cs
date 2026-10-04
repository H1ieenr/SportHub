using System;
using Shared.Exceptions;

namespace sporthub.domain
{
    public class OptionDefinition : AuditableEntity
    {
        public string code { get; private set; } = "";
        public string name { get; private set; } = "";
        public bool is_active { get; private set; } = true;

        private readonly List<OptionValue> _values = new();
        public IReadOnlyCollection<OptionValue> values => _values;

        private readonly List<CategoryOption> _category_options = new();
        public IReadOnlyCollection<CategoryOption> category_options => _category_options;
        private OptionDefinition() { }

        public static OptionDefinition Create(string code, string name)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new ValidationException("code không được để trống.");
            if (string.IsNullOrWhiteSpace(name))
                throw new ValidationException("Tên option không được để trống.");

            return new OptionDefinition
            {
                code = code.Trim().ToLowerInvariant(),
                name = name.Trim()
            };
        }

        public void Update(string name, bool isActive)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ValidationException("Tên option không được để trống.");
            this.name = name.Trim();
            is_active = isActive;
        }
    }
}