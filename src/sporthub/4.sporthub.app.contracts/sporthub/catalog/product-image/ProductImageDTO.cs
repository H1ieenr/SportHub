using Microsoft.AspNetCore.Http;
using Shared.Common;

namespace sporthub.app.contracts
{
    public class ProductImageDTO
    {
        public long id { get; set; }
        public long product_id { get; set; }
        public long? product_variant_id { get; set; }
        public string image_url { get; set; } = "";
        public int display_order { get; set; }
        public bool is_primary { get; set; }
    }

    #region Create
    public class CreateProductImageRequestDTO : BaseRequestDTO
    {
        public long product_id { get; set; }
        public long? product_variant_id { get; set; }
        public IFormFile file_image { get; set; }
        public int display_order { get; set; } = 0;
        public bool is_primary { get; set; } = false;
    }
    #endregion
    #region CreateBatchProductImage
    public class CreateBatchProductImageRequestDTO : BaseRequestDTO
    {
        public long product_id { get; set; }
        public long? product_variant_id { get; set; }
        public List<CreateBatchProductImageItem> product_images { get; set; } = [];
    }
    public class CreateBatchProductImageItem
    {
        public IFormFile file_image { get; set; }
        public int display_order { get; set; } = 0;
        public bool is_primary { get; set; } = false;
    }
    public class CreateBatchProductImageResponseDTO
    {
        public long id { get; set; }
        public string file_name { get; set; } = "";
        public string image_url { get; set; } = "";
        public bool success { get; set; }
        public string? message { get; set; }
        public static CreateBatchProductImageResponseDTO Ok(string file_name, string image_url) =>
            new() { file_name = file_name, image_url = image_url, success = true, message = "Tạo thành công" };
        public static CreateBatchProductImageResponseDTO Fail(string file_name, string message) =>
            new() { file_name = file_name, success = false, message = message };
    }
    #endregion
    #region Delete
    public class DeleteProductImageRequestDTO : BaseRequestDTO
    {
        public long id { get; set; }
    }
    #endregion
    #region DeleteBatch
    public class DeleteBatchProductImageRequestDTO : BaseRequestDTO
    {
        public List<long> id { get; set; } = [];
    }
    public class DeleteBatchProductImageResponseDTO
    {
        public long id { get; set; }
    }
    #endregion
    #region UpdatePrimary
    public class UpdatePrimaryProductImageRequestDTO : BaseRequestDTO
    {
        public long product_id { get; set; }
        public long? product_variant_id { get; set; }
        public long id { get; set; }
    }
    #endregion
    #region UpdateDisplayOrder
    public class UpdateDisplayOrderProductImageRequestDTO : BaseRequestDTO
    {
        public long product_id { get; set; }
        public long? product_variant_id { get; set; }
        public List<UpdateDisplayOrderProductImageItemDTO> items { get; set; } = [];
    }
    public class UpdateDisplayOrderProductImageItemDTO
    {
        public long id { get; set; }
        public int display_order { get; set; }
    }
    #endregion
    #region ProductImageGetNoPagingAsync
    public class GetProductImageNoPagingRequestDTO : BaseRequestDTO
    {
        public long product_id { get; set; }
        public long? product_variant_id { get; set; }
        public bool? is_primary { get; set; }
    }
    #endregion
}