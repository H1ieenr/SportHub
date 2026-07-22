namespace Shared.Common
{
    public class BaseRequestDTO
    {
        public string user_id { get; set; } = string.Empty;
        public long? lang_id { get; set; }
    }
}