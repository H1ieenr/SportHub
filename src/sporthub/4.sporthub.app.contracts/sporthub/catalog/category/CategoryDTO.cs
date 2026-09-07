using Microsoft.AspNetCore.Http;
using Shared.Common;

namespace sporthub.app.contracts
{
    public class CategoryDTO
    {
        public long id { get; set; }
        public string name { get; private set; } = "";
        public string slug { get; private set; } = "";
        public string? description { get; private set; }
        public string? image_url { get; private set; }
        public string? image_public_id { get; private set; }
        public long? parent_id { get; private set; }
        public int display_order { get; private set; }
        public bool is_active { get; private set; } = false;

        public DateTime created_date { get; set; }
        public long created_by { get; set; } = 0;
        public DateTime? updated_date { get; set; }
        public long updated_by { get; set; } = 0;
    }
    #region Create
    public class CreateCategoryRequestDTO : BaseRequestDTO
    {
        public string name { get; set; } = "";
        public string slug { get; set; } = "";
        public IFormFile? file_image { get; set; } = null;
        public string? description { get; set; }

        public long? parent_id { get; private set; }
        public int display_order { get; private set; }
        public bool is_active { get; private set; } = false;
    }
    #endregion
    #region Update
    public class UpdateCategoryRequestDTO : CreateCategoryRequestDTO
    {
        public long id { get; set; }
    }
    #endregion
    #region Delete
    public class DeleteCategoryRequestDTO : BaseRequestDTO
    {
        public long id { get; set; }
    }
    #endregion
    #region Update Active
    public class UpdateActiveCategoryRequestDTO : BaseRequestDTO
    {
        public long id { get; set; } 
    }
    #endregion
    #region Get By Id
    public class CategoryGetByIdRequestDTO : BaseRequestDTO
    {
        public long id { get; set; }
    }
    #endregion
    #region GetPagedAsync
    public class GetCategoriesPagedRequestDTO : PaginationParams
    {
        public string? search_text { get; set; }
    }
    #endregion
}