using Shared.Common;
using sporthub.domain;

namespace sporthub.app.contracts
{
    public class ProductDTO
    {
        public long id { get; set; }
        public long category_id { get; set; }
        public long? brand_id { get; set; }
        public string name { get; set; } = "";
        public string slug { get; set; } = "";
        public string? description { get; set; } = "";
        public decimal base_price { get; set; }
        public ProductStatus status { get; set; } = ProductStatus.Draft;
        public bool is_featured { get; set; } = false;

        public string? category_name { get; set; }
        public string? category_slug { get; set; }
        public string? brand_name { get; set; }
        //public CategoryDTO category { get; set; } = null!;
        //public BrandDTO? brand { get; set; }

        public List<ProductVariantDTO> variants { get; set; } = new();
        public List<ProductImageDTO> images { get; set; } = new(); 

        public DateTime created_date { get; set; }
        public long created_by { get; set; } = 0;
        public DateTime? updated_date { get; set; }
        public long updated_by { get; set; } = 0;
    }
    #region Create
    public class CreateProductRequestDTO : BaseRequestDTO
    {
        public long category_id { get; set; }
        public long? brand_id { get; set; }
        public string name { get; set; } = "";
        public string slug { get; set; } = "";
        public string? description { get; set; } = "";
        public decimal base_price { get; set; }
        public ProductStatus status { get; set; } = ProductStatus.Draft;
        public bool is_featured { get; set; } = false;
    }
    #endregion
    #region Update
    public class UpdateProductRequestDTO : CreateProductRequestDTO
    {
        public long id { get; set; }
    }
    #endregion
    #region Delete
    public class DeleteProductRequestDTO : BaseRequestDTO
    {
        public long id { get; set; }
    }
    #endregion
    #region Update status
    public class UpdateStatusProductRequestDTO : BaseRequestDTO
    {
        public ProductStatus status { get; set; } = ProductStatus.Draft;
    }
    #endregion
    #region Update featured
    public class UpdateFeaturedProductRequestDTO : BaseRequestDTO
    {
        public long id { get; set; }
    }
    #endregion
    #region Get By Id
    public class ProductGetByIdRequestDTO : BaseRequestDTO
    {
        public long id { get; set; }
    }
    public class ProductGetByIdResponseDTO : ProductDTO
    {
        public List<ProductOptionGroupDTO> options { get; set; } = new();
    }
    public class ProductOptionGroupDTO
    {
        public long option_definition_id { get; set; }
        public string name { get; set; } = "";
        public List<ProductOptionValueItemDTO> values { get; set; } = new();
    }

    public class ProductOptionValueItemDTO
    {
        public long option_value_id { get; set; }
        public string value { get; set; } = "";
    }
    #endregion
    #region GetPagedAsync
    public class GetProductPagedRequestDTO : PaginationParams
    {
        public ProductStatus? status { get; set; }
        public string? search_text { get; set; }
        public bool? is_featured { get; set; }
    }
    #endregion
    #region ProductGetNoPagingAsync
    public class GetProductNoPagingRequestDTO : BaseRequestDTO
    {
        public ProductStatus? status { get; set; }
        public string? search_text { get; set; }
        public bool? is_featured { get; set; }
    }
    #endregion
}