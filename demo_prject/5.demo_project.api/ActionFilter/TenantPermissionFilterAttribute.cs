//using azicloud.library;
//using azitask.app.contracts;
//using MassTransit;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc.Filters;
//using Microsoft.Extensions.Caching.Distributed;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace azitask.api
//{
//    public class TenantPermissionFilterAttribute : ActionFilterAttribute
//    {
//        //private readonly IAuthorizationService _authorizationService;
//        private readonly IPermissionAppService _permissionAppService;
//        private readonly IHttpContextAccessor _httpContextAccessor;

//        public TenantPermissionFilterAttribute(
//            IHttpContextAccessor httpContextAccessor,
//            IPermissionAppService permissionAppService)
//        {

//            _httpContextAccessor = httpContextAccessor;
//            _permissionAppService = permissionAppService;
//        }

//        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
//        {
//            var lang_id = Convert.ToInt64(_httpContextAccessor.HttpContext.Request.Headers["lang_id"]);
//            var user = _httpContextAccessor.HttpContext.User;
//            ITenantPermission appsPermission;
//            PermissionDTO authorizationResult = null;
//            foreach (var obj in context.ActionArguments)
//            {
//                if (obj.Value.GetType().IsAssignableTo(typeof(ITenantPermission)))
//                {
//                    appsPermission = (ITenantPermission)obj.Value;
//                    if (appsPermission.apps_id == -1)
//                    {
//                        authorizationResult = new PermissionDTO
//                        {
//                            Id = -1,
//                            message = "",
//                            result = -1
//                        };
//                        break;
//                    }

//                    var permissionTenant = await _permissionAppService.TenantByUserId_Permission(new TenantByUserIdPermissionRequestDTO
//                    {
//                        lang_id = lang_id,
//                        apps_id = appsPermission.apps_id
//                    });
//                    authorizationResult = permissionTenant;
//                    break;
//                }
//            }

//            if (authorizationResult != null)
//            {
//                if (authorizationResult.result > 0)
//                {
//                    await base.OnActionExecutionAsync(context, next);
//                }
//                else
//                {
//                    context.HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
//                }
//            }
//            else
//            {
//                await base.OnActionExecutionAsync(context, next);
//            }
//        }
//    }
//}
