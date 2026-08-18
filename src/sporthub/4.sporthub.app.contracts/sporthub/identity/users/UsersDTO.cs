using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared.Common;
namespace sporthub.app.contracts
{
    public class UsersDTO
    {
        public long id { get; set; }
        public string email { get; set; } = "";
        public string name { get; set; } = "";
        public string phone { get; set; } = "";
        public string avatar_url { get; set; } = "";
        public List<string> roles { get; set; } = new();
        public UserStatusDTO status { get; set; }
    };

    #region GetByIdAsync
    public class GetByIdAsyncRquestDTO : BaseRequestDTO
    {
        public long id {get; set;} = 0;
    }
    public class GetByIdAsyncDTO : UsersDTO
    {}
    #endregion
}