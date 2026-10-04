using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared.Common;

namespace sporthub.app.contracts
{
    public class CategoryOptionDTO
    {
        private static readonly string[] AllowedColumns = ["option1", "option2", "option3"];

        public long id { get; set; }
        public long category_id { get; set; }
        public string option_column { get; set; } = ""; // "option1" | "option2" | "option3"
        public string label { get; set; } = "";
        public int? display_order { get; set; }
        //public CategoryDTO category { get; set; } = null!;
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
        public List<CategoryOptionItemDTO> category_option_maps { get; set; } = [];
    }

    public class CategoryOptionItemDTO
    {
        public string option_column { get; set; } = ""; // "option1" | "option2" | "option3"
        public string label { get; set; } = "";
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
