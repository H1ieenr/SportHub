using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared.Common;

namespace sporthub.app.contracts
{
    public class OptionValueDTO
    {
        public long id { get; set; } = 0;
        public long option_definition_id { get; set; } = 0;
        public string value { get; set; } = "";
        public int display_order { get; set; } = 0;
    }

    #region CreateAsync 
    public class CreateOptionValueRequestDTO : BaseRequestDTO
    {
        public long option_definition_id { get; set; } = 0;
        public List<CreateOptionValueItem> create_option_values { get; set; } = [];

    }
    public class CreateOptionValueItem
    {
        public string value { get; set; } = "";
        public int display_order { get; set; } = 0;
    }
    public class CreateOptionValueResponseDTO 
    {
        public long id { get; set; } = 0;
        public string value { get; set; } = "";
        public bool success { get; set; }
        public string? message { get; set; }

        public static CreateOptionValueResponseDTO Ok(string value) =>
            new() { value = value, success = true, message = "Tạo thành công" };

        public static CreateOptionValueResponseDTO Fail(string value, string message) =>
            new() { value = value, success = false, message = message };
    }
    #endregion
    #region UpdateAsync
    public class UpdateOptionValueRequestDTO : BaseRequestDTO
    {
        public long id { get; set; } = 0;
        public string value { get; set; } = "";
        public int display_order { get; set; } = 0;
    }
    #endregion
    #region DeleteValueAsync
    public class DeleteOptionValueRequestDTO : BaseRequestDTO
    {
        public long id { get; set; } = 0;
    }
    #endregion
}