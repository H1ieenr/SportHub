using System.Text.Json;
using Shared.Common;

namespace sporthub.app.contracts
{
    public class ProductVariantDTO
    {
        public long id { get; set; }
        public long product_id { get; set; }
        public string sku { get; set; } = "";
        public string name { get; set; } = "";
        public decimal cost_price { get; set; }
        public decimal sale_price { get; set; }
        public bool is_active { get; set; } = false;
        public JsonElement? value_json { get; set; }

        public string? option1 { get; set; }
        public string? option2 { get; set; }
        public string? option3 { get; set; }

        public ProductDTO product { get; set; } = null!;
        //public InventoryItem? inventory { get; set; }
    }
    #region Create
    public class CreateProductVariantRequestDTO : BaseRequestDTO
    {
        public long product_id { get; set; }
        public string sku { get; set; } = "";
        public string name { get; set; } = "";
        public decimal cost_price { get; set; }
        public decimal sale_price { get; set; }
        public bool is_active { get; set; } = false;
        public JsonElement? value_json { get; set; }
    }
    #endregion
    #region CreateBatchProductVariant
    public class CreateBatchProductVariantRequestDTO : BaseRequestDTO
    {
        public long product_id { get; set; }
        public List<CreateBatchProductVariantItem> product_variants { get; set; } = [];
    }
    public class CreateBatchProductVariantItem
    {
        public string sku { get; set; } = "";
        public string name { get; set; } = "";
        public decimal cost_price { get; set; }
        public decimal sale_price { get; set; }
        public bool is_active { get; set; } = false;
        public JsonElement? value_json { get; set; }
    }
    public class CreateBatchProductVariantResponseDTO
    {
        public long id { get; set; }
        public string sku { get; set; } = "";
        public string name { get; set; } = "";
        public bool success { get; set; }
        public string? message { get; set; }

        public static CreateBatchProductVariantResponseDTO Ok(string sku, string name) =>
            new() { sku = sku, name = name, success = true, message = "Tạo thành công" };

        public static CreateBatchProductVariantResponseDTO Fail(string sku, string name, string message) =>
            new() { sku = sku, name = name, success = false, message = message };
    }
    #endregion
    #region Update
    public class UpdateProductVariantRequestDTO : CreateProductVariantRequestDTO
    {
        public long id { get; set; }
    }
    #endregion
    #region Delete
    public class DeleteProductVariantRequestDTO : BaseRequestDTO
    {
        public long id { get; set; }
    }
    #endregion
    #region Update active
    public class UpdateActiveProductVariantRequestDTO : BaseRequestDTO
    {
        public long id { get; set; }
    }
    #endregion
    #region Get By Id
    public class ProductVariantGetByIdRequestDTO : BaseRequestDTO
    {
        public long id { get; set; }
    }
    #endregion
    #region GetPagedAsync
    public class GetProductVariantPagedRequestDTO : PaginationParams
    {
        public long? product_id { get; set; }
        public string? search_text { get; set; }
        public bool? is_active { get; set; }
    }
    #endregion
    #region ProductVariantGetNoPagingAsync
    public class GetProductVariantNoPagingRequestDTO : BaseRequestDTO
    {
        public long? product_id { get; set; }
        public string? search_text { get; set; }
        public bool? is_active { get; set; }
    }
    #endregion
}