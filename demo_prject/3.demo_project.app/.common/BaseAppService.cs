
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using azicloud.app.contracts;
using azicloud.common;

namespace demo_project.app
{
    public abstract class BaseAppService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        protected readonly ILogger _logger;

        public BaseAppService(IHttpContextAccessor httpContextAccessor,
            ILogger logger,
            IServiceProvider serviceProvider)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public BaseAppService(IHttpContextAccessor httpContextAccessor,
            ILogger logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public BaseAppService(ILogger logger)
        {
            _logger = logger;
        }

        protected T Invoke<T>(Func<T> func)
        {
            try
            {
                var result = func();
                return result;
            }
            catch (RepositoryException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }




        protected long GetCurrentAziCloudUserId()
        {
            var user = _httpContextAccessor.HttpContext.User;
            long uId;
            try
            {
                uId = long.Parse((user.Claims.SingleOrDefault(x => x.Type == "Id").Value));
            }
            catch (Exception)
            {
                uId = -1;
            }
            return uId;
        }

        protected long GetCurrentAziCloudLangId()
        {
            var user = _httpContextAccessor.HttpContext.User;
            var lang_id = Convert.ToInt64(_httpContextAccessor.HttpContext.Request.Headers["lang_id"]);
            return lang_id;
        }
    }
}
