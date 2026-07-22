#nullable disable
using azicloud.app.contracts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace demo_project.app.contracts
{
    #region Request
    public class MarketingBaseRequestDTO : TenantBaseRequestDTO
    {
    }

    public class MarketingDeleteRequestDTO : MarketingBaseRequestDTO
    {
        public long Id { get; set; }
    }
    #endregion
    #region Response


    /// <summary>
    /// result, id, message
    /// </summary>
    public abstract class ResultMessageResponseDTO : BaseResponseDTO
    {
        public int result { get; set; }
        public long id { get; set; }
        public string message { get; set; }
    }

    /// <inheritdoc/>
    public class CreateResponseDTO : ResultMessageResponseDTO
    {
    }

    /// <inheritdoc/>
    public class UpdatedResponseDTO : ResultMessageResponseDTO
    {
    }

    /// <inheritdoc/>
    public class DeletedResponseDTO : ResultMessageResponseDTO
    {
    }

    public class CheckResponseDTO : ResultMessageResponseDTO
    {
    }
    #endregion


    //public class BaseRequestDTO
    //{
    //    //[JsonIgnore]
    //    [FromHeaderAttribute]
    //    public long user_id { get; set; }

    //    //[JsonIgnore]
    //    [FromHeaderAttribute]
    //    public long lang_id { get; set; }
    //}
    //public class TenantBaseRequestDTO : BaseRequestDTO
    //{
    //    [Required]
    //    public long tenant_id { get; set; }
    //}

    public class ZaloAppBaseRequestDTO : TenantBaseRequestDTO
    {
        [Required]
        public long zaloapp_id { get; set; }
    }
    public class ChannelBaseRequestDTO : TenantBaseRequestDTO
    {
        public long channel_id { get; set; }
    }

    public class ZaloAppDeleteRequestDTO : ZaloAppBaseRequestDTO
    {
        public long Id { get; set; }
    }
    public abstract class BaseResponseDTO
    {
    }

    public class SyncMasterRequest
    {
        public long id { get; set; } = 0;
        public long master_id { get; set; } = 0;
        public long zaloapp_id { get; set; } = 0;
        public bool is_replace { get; set; }
        public long currency_id { get; set; }
        public long tenant_id { get; set; }
        public long user_id { get; set; }
        public long group_id { get; set; }
        public long lang_id { get; set; }
    }

    public class SyncMultipleMasterRequestDTO : TenantBaseRequestDTO
    {
        public List<long> list_id { get; set; } = new List<long>();
        public long zaloapp_id { get; set; } = 0;
        public bool is_replace { get; set; }
        public long currency_id { get; set; }
        public long group_id { get; set; }
    }

    public class SyncMasterResult
    {
        public SyncMasterResult(long id, long master_id, int result, string message)
        {
            this.id = id;
            this.result = result;
            this.master_id = master_id;
            this.message = message;
        }
        public SyncMasterResult()
        {
        }
        public long id { get; set; }
        public long master_id { get; set; }
        public int result { get; set; }
        public string message { get; set; }
    }




    //#region create multi store
    //public abstract class ActionResultDTO
    //{
    //    /// <summary>
    //    /// Loại Action
    //    /// </summary>
    //    public abstract string type { get; set; }
    //    /// <summary>
    //    /// Id của đối tượng
    //    /// </summary>
    //    public long id { get; set; }
    //    /// <summary>
    //    /// Mã kết quả 
    //    /// </summary>
    //    public int result { get; set; }
    //    /// <summary>
    //    /// Thông báo trả về
    //    /// </summary>
    //    public string message { get; set; } = "";
    //}

    //public class CreateActionResultDTO : ActionResultDTO
    //{
    //    /// <summary>
    //    /// Loại Action
    //    /// </summary>
    //    public override string type { get; set; } = "create";
    //}
    //public class DeleteActionResultDTO : ActionResultDTO
    //{
    //    public override string type { get; set; } = "delete";
    //}
    //public class VerifyActionResultDTO : ActionResultDTO
    //{
    //    public override string type { get; set; } = "verify";
    //}
    //#endregion

    public class ZaloAppImageDeleteRequestDTO : ZaloAppDeleteRequestDTO
    {
        public string image { get; set; }
    }

    public static class DecimalUtil
    {
        public static decimal PlusZeroPointZero(this decimal value)
        {
            return value + 0.00m;
        }
    }

    public class ZaloAppPageBaseFilterRequestDTO : ZaloAppBaseRequestDTO
    {
        /// <summary>
        /// Page hiện tại
        /// </summary>
        /// <example>1</example>
        public int page { get; set; } = 0;
        /// <summary>
        /// Số lượng item trong 1 page
        /// </summary>
        /// <example>10</example>
        public int page_size { get; set; } = 0;
        /// <summary>
        /// Loại sắp xếp (Tăng (asc) hay giảm (desc) )
        /// </summary>
        /// <example>asc</example>
        public string sort_type { get; set; } = "";
        /// <summary>
        /// Field cần sắp xếp
        /// </summary>
        /// <example>id</example>
        public string sort_option { get; set; } = "";
        /// <summary>
        /// Text Search (Truyền "" lấy hết)
        /// </summary>
        /// <example>Text Search</example>
        public string search_text { get; set; } = "";

    }
}
