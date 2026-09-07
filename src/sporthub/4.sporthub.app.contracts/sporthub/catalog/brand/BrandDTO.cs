using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Shared.Common;

namespace sporthub.app.contracts
{
    public class BrandDTO
    {
        public long id { get; set; }
        public string name { get; set; } = "";
        public string slug { get; set; } = "";
        public string? logo_url { get; set; }
        public string? description { get; set; }
        public bool is_active { get; set; }
        
        public DateTime created_date { get; set; }
        public long created_by { get; set; } = 0;
        public DateTime? updated_date { get; set; }
        public long updated_by { get; set; } = 0;
    }
    #region Create
    public class CreateBrandRequestDTO : BaseRequestDTO
    {
        public string name { get; set; } = "";
        public string slug { get; set; } = "";
        public IFormFile? file_logo { get; set; } = null;
        public string? description { get; set; }
        public bool is_active { get; set; }
    }
    #endregion
    #region Update
    public class UpdateBrandRequestDTO : CreateBrandRequestDTO
    {
        public long id { get; set; }
    }
    #endregion
    #region Delete
    public class DeleteBrandRequestDTO : BaseRequestDTO
    {
        public long id { get; set; }
    }
    #endregion
    #region Update Active
    public class UpdateActiveBrandRequestDTO : BaseRequestDTO
    {
        public long id { get; set; }
    }
    #endregion
    #region Get By Id
    public class BrandGetByIdRequestDTO : BaseRequestDTO
    {
        public long id { get; set; }
    }
    #endregion
    #region GetPagedAsync
    public class GetBrandsPagedRequestDTO : PaginationParams
    {
        public string? search_text { get; set; }
    }
    #endregion
}