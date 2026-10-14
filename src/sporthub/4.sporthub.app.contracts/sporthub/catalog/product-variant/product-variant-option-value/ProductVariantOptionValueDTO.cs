using System.Text.Json;
using Shared.Common;


namespace sporthub.app.contracts
{
    public class ProductVariantOptionValueDTO
    {
        public long option_definition_id { get; set; }
        public string option_definition_name { get; set; } = "";
        public long option_value_id { get; set; }
        public string option_value { get; set; } = "";
    }

    #region CreateVariantOptionValueItemDTO
    public class CreateProductVariantOptionValueDTO
    {
        public long option_definition_id { get; set; }
        public long option_value_id { get; set; }
    }
    #endregion
}
