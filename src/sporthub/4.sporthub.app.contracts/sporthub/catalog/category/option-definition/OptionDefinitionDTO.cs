using Microsoft.AspNetCore.Http;
using Shared.Common;


namespace sporthub.app.contracts
{
    public class OptionDefinitionDTO
    {
        public long id { get; set; } = 0;
        public string code { get; set; } = "";
        public string name { get; set; } = "";
        public bool is_active { get; set; } = false;
        public List<OptionValueDTO> option_values { get; set; } = new List<OptionValueDTO>();
    }
    #region CreateAsync
    public class CreateOptionDefinitionRequestDTO : BaseRequestDTO
    {
        public string code { get; set; } = "";
        public string name { get; set; } = "";
        public bool is_active { get; set; } = false;
        public List<CreateOptionValueItem>? create_option_values { get; set; } = [];
    }
    #endregion
    #region UpdateAsync
    public class UpdateOptionDefinitionRequestDTO : CreateOptionDefinitionRequestDTO
    {
        public long id { get; set; } = 0;
    }
    #endregion
    #region DeleteAsync
    public class DeleteOptionDefinitionRequestDTO : BaseRequestDTO
    {
        public long id { get; set; } = 0;
    }
    #endregion
    #region GetNoPagingAsync
    public class GetOptionDefinitionsNoPagingRequestDTO : BaseRequestDTO
    {
        public string? search_text { get; set; }
        public string? code { get; set; }
        public List<long>? ids { get; set; }
    }
    #endregion 
}