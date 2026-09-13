using Shared.Common;
using sporthub.domain;

namespace sporthub.app.contracts
{
    public class ProductDTO
    {

    }
    #region Create
    public class CreateProductRequestDTO : BaseRequestDTO
    {
        public long category_id { get; private set; }
        public long? brand_id { get; private set; }
        public string name { get; private set; } = "";
        public string slug { get; private set; } = "";
        public string? description { get; private set; } = "";
        public decimal base_price { get; private set; }
        public ProductStatus status { get; private set; } = ProductStatus.Draft;
        public bool is_featured { get; private set; } = false;
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