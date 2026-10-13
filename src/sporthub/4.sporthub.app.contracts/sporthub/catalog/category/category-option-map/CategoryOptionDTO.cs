using Shared.Common;

namespace sporthub.app.contracts
{
    public class CategoryOptionDTO
    {
        public long id { get; set; }
        public long category_id { get; set; }
        public long option_definition_id { get; set; }
        public int? display_order { get; set; }
        public OptionDefinitionDTO option_definition { get; set; } = null!;
    }

    #region CategoryOptionGetNoPagingAsync
    public class CategoryOptionGetNoPagingRequestDTO : BaseRequestDTO
    {
        public string? search_text { get; set; }
        public long? category_id { get; set; }
    }
    #endregion
    #region Get By Id
    public class CategoryOptionGetByIdRequestDTO : BaseRequestDTO
    {
        public long id { get; set; }
    }
    #endregion
    #region ReplaceAllAsync
    public class ReplaceAllAsyncRequestDTO : BaseRequestDTO
    {
        public long category_id { get; set; }
        public List<CategoryOptionItemDTO> category_options { get; set; } = [];
    }

    public class CategoryOptionItemDTO
    {
        public long option_definition_id { get; set; }
        public int? display_order { get; set; }
    }
    public class ReplaceAllAsyncResponseDTO
    {
        public long id { get; set; }
        public string label { get; set; } = "";
        public bool success { get; set; }
        public string? message { get; set; }

        public static ReplaceAllAsyncResponseDTO Ok(string label) =>
            new()
            {
                label = label,
                success = true,
                message = "Tạo thành công",
            };

        public static ReplaceAllAsyncResponseDTO Fail(string label, string message) =>
            new()
            {
                label = label,
                success = false,
                message = message,
            };
    }
    #endregion
}
