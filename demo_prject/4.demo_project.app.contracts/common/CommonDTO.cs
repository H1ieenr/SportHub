using azicloud.app.contracts;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace demo_project.app.contracts
{
    //public class BaseRequestDTO
    //{
    //    [JsonIgnore]
    //    public long user_id { get; set; }

    //    [JsonIgnore]
    //    public long lang_id { get; set; }
    //}
    //public class TenantBaseRequestDTO : BaseRequestDTO
    //{
    //    public long tenant_id { get; set; }
    //}

    //public class ZaloAppBaseRequestDTO : TenantBaseRequestDTO
    //{
    //    public long zaloapp_id { get; set; }
    //}
    //public class ChannelBaseRequestDTO : TenantBaseRequestDTO
    //{
    //    public long channel_id { get; set; }
    //}

    //public class ZaloAppDeleteRequestDTO : ZaloAppBaseRequestDTO
    //{
    //    public long Id { get; set; }
    //}
    //public abstract class BaseResponseDTO
    //{
    //}

    //public class SyncMasterResult
    //{
    //    public SyncMasterResult(long id, long master_id, int result, string message)
    //    {
    //        this.id = id;
    //        this.result = result;
    //        this.master_id = master_id;
    //        this.message = message;
    //    }
    //    public SyncMasterResult()
    //    {
    //    }
    //    public long id { get; set; }
    //    public long master_id { get; set; }
    //    public int result { get; set; }
    //    public string message { get; set; }
    //}

    public class BaseOpenApiRequestDTO : TenantBaseRequestDTO
    {
        [Required]
        public long zaloapp_id { get; set; }
    }
    public class StepResultDTO
    {
        public string step { get; set; }
        public bool success { get; set; }
        public string? message { get; set; }
        public object? data { get; set; }
    }

}
