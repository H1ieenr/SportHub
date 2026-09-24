
using Shared.Common;

namespace sporthub.app.contracts
{
    public class EnumItemDTO
    {
        public int value { get; set; }
        public string name { get; set; } = "";
        public string label { get; set; } = "";
    } 
    public class EnumRequestDTO : BaseRequestDTO
    {
        public string type {get; set;} = "";
    }
}