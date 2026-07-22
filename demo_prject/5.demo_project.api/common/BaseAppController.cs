using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using demo_project.app.contracts;
using azicloud.app.contracts;
using System.Data.SqlClient;
using azicloud.common;

namespace demo_project.api
{
    public abstract class BaseAppController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        protected readonly ILogger _logger;

        public BaseAppController(IHttpContextAccessor httpContextAccessor,
            ILogger logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public BaseAppController(ILogger logger)
        {
            _logger = logger;
        }


        protected async Task<OkObjectResult> ReturnOk(Func<Task<ResponseDTO>> func, BaseRequestDTO model)
        {
            //using var activity = this.activitySource.StartActivity($"controller.{func.Method.Name}");
            try
            {
                model.user_id = GetCurrentAziCloudUserId();
                model.lang_id = GetCurrentAziCloudLangId();

                var result = await func();
                //if (activity != null)
                //{
                //    activity.AddTag("user.id", model.user_id);
                //    activity.AddTag("lang.id", model.lang_id);
                //    activity.AddTag("result", JsonSerializer.Serialize(result));
                //}
                return Ok(result);

            }
            catch (RepositoryException ex)
            {

                //_logger.LogError(ex, logMessage);
                //activity.AddTag("exception", ex.ToString());
                //activity.SetStatus(ActivityStatusCode.Error);
                return Ok(ResponseDTO.Error(null, ex.code, ex.Message, ex.Message));
            }
            catch (SqlException ex)
            {
                return Ok(ResponseDTO.Error(null, 101, ex.Message, ex.Message));
            }
            catch (Exception ex)
            {

                return Ok(ResponseDTO.Error(null, -100, ex.Message, ex.Message));
            }
        }
        protected async Task<OkObjectResult> ReturnByConsumerOk(Func<Task<ResponseDTO>> func, TenantConsumerBaseRequestDTO model)
        {
            //using var activity = this.activitySource.StartActivity($"controller.{func.Method.Name}");
            try
            {
                model.user_id = GetCurrentAziCloudUserId();
                model.consumer_id = GetCurrentAziCloudUserId();
                model.zuId = GetZuId();
                model.zuToken = GetZuToken();
                if (model.lang_id <= 0)
                {
                    model.lang_id = 1;
                }

                var result = await func();
                return Ok(result);

            }
            catch (RepositoryException ex)
            {
                return Ok(ResponseDTO.Error(null, ex.code, ex.Message, ex.Message));
            }
            catch (Exception ex)
            {
                return Ok(ResponseDTO.Error(null, -100, ex.Message, ex.Message));
            }
        }

        //protected async Task<OkObjectResult> ReturnTenantZaloAppOk(Func<Task<ResponseDTO>> func, TenantZaloAppBaseRequestDTO model)
        //{
        //    //using var activity = this.activitySource.StartActivity($"controller.{func.Method.Name}");
        //    try
        //    {
        //        model.user_id = GetCurrentAziCloudUserId();
        //        model.lang_id = GetCurrentAziCloudLangId();

        //        var result = await func();
        //        //if (activity != null)
        //        //{
        //        //    activity.AddTag("user.id", model.user_id);
        //        //    activity.AddTag("lang.id", model.lang_id);
        //        //    activity.AddTag("result", JsonSerializer.Serialize(result));
        //        //}
        //        return Ok(result);

        //    }
        //    catch (RepositoryException ex)
        //    {
        //        var logMessage = new LogMessage
        //        {
        //            service = LogService.Notification,
        //            value = ex.Message,

        //            error_code = ex.code,
        //            time = DateTime.UtcNow,
        //            type = LogType.Repository,
        //        };
        //        logMessage = GetFromHttp(logMessage);
        //        //_logger.LogError(ex, logMessage);
        //        //activity.AddTag("exception", ex.ToString());
        //        //activity.SetStatus(ActivityStatusCode.Error);
        //        return Ok(ResponseDTO.Error(null, ex.code, ex.Message, ex.Message));
        //    }
        //    catch (SqlException ex)
        //    {
        //        return Ok(ResponseDTO.Error(null, 101, ex.Message, ex.Message));
        //    }
        //    catch (Exception ex)
        //    {
        //        var logMessage = new LogMessage
        //        {
        //            service = LogService.Notification,
        //            value = ex.Message,
        //            error_code = -101,
        //            time = DateTime.UtcNow,
        //            type = LogType.Repository,
        //        };
        //        logMessage = GetFromHttp(logMessage);
        //        //_logger.LogError(ex, logMessage);
        //        //activity.AddTag("exception", ex.ToString());
        //        //activity.SetStatus(ActivityStatusCode.Error);
        //        return Ok(ResponseDTO.Error(null, -100, ex.Message, ex.Message));
        //    }
        //}

        //protected async Task<OkObjectResult> ReturnCustomerZaloAppOk(Func<Task<ResponseDTO>> func, CustomerZaloAppBaseRequestDTO model)
        //{
        //    //using var activity = this.activitySource.StartActivity($"controller.{func.Method.Name}");
        //    try
        //    {
        //        model.customer_id = GetCurrentAziCloudCustomerId();
        //        model.lang_id = GetCurrentAziCloudLangId();

        //        var result = await func();
        //        //if (activity != null)
        //        //{
        //        //    activity.AddTag("user.id", model.user_id);
        //        //    activity.AddTag("lang.id", model.lang_id);
        //        //    activity.AddTag("result", JsonSerializer.Serialize(result));
        //        //}
        //        return Ok(result);

        //    }
        //    catch (RepositoryException ex)
        //    {
        //        var logMessage = new LogMessage
        //        {
        //            service = LogService.Notification,
        //            value = ex.Message,

        //            error_code = ex.code,
        //            time = DateTime.UtcNow,
        //            type = LogType.Repository,
        //        };
        //        logMessage = GetFromHttp(logMessage);
        //        //_logger.LogError(ex, logMessage);
        //        //activity.AddTag("exception", ex.ToString());
        //        //activity.SetStatus(ActivityStatusCode.Error);
        //        return Ok(ResponseDTO.Error(null, ex.code, ex.Message, ex.Message));
        //    }
        //    catch (SqlException ex)
        //    {
        //        return Ok(ResponseDTO.Error(null, 101, ex.Message, ex.Message));
        //    }
        //    catch (Exception ex)
        //    {
        //        var logMessage = new LogMessage
        //        {
        //            service = LogService.Notification,
        //            value = ex.Message,
        //            error_code = -101,
        //            time = DateTime.UtcNow,
        //            type = LogType.Repository,
        //        };
        //        logMessage = GetFromHttp(logMessage);
        //        //_logger.LogError(ex, logMessage);
        //        //activity.AddTag("exception", ex.ToString());
        //        //activity.SetStatus(ActivityStatusCode.Error);
        //        return Ok(ResponseDTO.Error(null, -100, ex.Message, ex.Message));
        //    }
        //}
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
        protected long GetCurrentAziCloudCustomerId()
        {
            var user = _httpContextAccessor.HttpContext.User;
            long uId;
            try
            {
                uId = long.Parse((user.Claims.SingleOrDefault(x => x.Type == "CustomerId").Value));
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
        protected string GetZuId()
        {
            var user = _httpContextAccessor.HttpContext.User;
            var zuId = _httpContextAccessor.HttpContext.Request.Headers["zuId"];
            return zuId;
        }
        protected string GetZuToken()
        {
            var user = _httpContextAccessor.HttpContext.User;
            var zuToken = _httpContextAccessor.HttpContext.Request.Headers["zuToken"];
            return zuToken;
        }

    }
}
