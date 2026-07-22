//using azicloud.app.contracts;
//using azicloud.zaloauth;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Filters;
//using Microsoft.Extensions.DependencyInjection;
//using System;
//using System.Reflection;
//using System.Threading.Tasks;
//using demo_project.app.contracts;
//using demo_project.app.contracts;

//namespace demo_project.api
//{
//    public class ZaloAuthorizeAttribute : ActionFilterAttribute
//    {
//        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
//        {
//            // 🔥 Lấy các service cần thiết một lần
//            var serviceProvider = context.HttpContext.RequestServices;
//            var zaloApiService = serviceProvider.GetService<IZaloApiService>();
//            var zaloappService = serviceProvider.GetService<IZaloAppInternalAppService>();

//            if (zaloApiService == null || zaloappService == null)
//            {
//                context.Result = new ObjectResult(new { message = "Service not available" }) { StatusCode = 500 };
//                return;
//            }

//            // 🔥 Đọc token từ header và kiểm tra
//            if (!context.HttpContext.Request.Headers.TryGetValue("zuToken", out var tokenHeader) ||
//                string.IsNullOrEmpty(tokenHeader.ToString().Replace("Bearer ", "").Trim()))
//            {
//                context.Result = new UnauthorizedObjectResult(new { message = "Invalid or missing Zalo token" });
//                return;
//            }
//            string accessToken = tokenHeader.ToString().Replace("Bearer ", "").Trim();

//            // 🔥 Lấy `ConsumerTopupZaloConsumerCreateRequestDTO` từ context trực tiếp
//            if (!context.ActionArguments.TryGetValue("model", out var modelObj) || modelObj is not TenantZaloAppBaseRequestDTO model)
//            {
//                context.Result = new BadRequestObjectResult(new { message = "Invalid request body" });
//                return;
//            }

//            // 🔥 Đọc `lang_id` và `zaloapp_id` từ model trực tiếp
//            var langId = model.lang_id;
//            var zaloappId = model.zaloapp_id;

//            if (langId <= 0 || zaloappId <= 0)
//            {
//                context.Result = new BadRequestObjectResult(new { message = "Missing lang_id hoặc zaloapp_id" });
//                return;
//            }

//            // 🔥 Lấy thông tin `zaloapp` từ service
//            var zaloappInfo = await zaloappService.zaloappById_Row(new ZaloAppByIdRowRequestDTO
//            {
//                Id = zaloappId,
//                lang_id = langId
//            });
//            if (zaloappInfo == null)
//            {
//                context.Result = new UnauthorizedObjectResult(new { message = "Zalo User not found" });
//                return;
//            }
//            context.HttpContext.Items["ConsumerInfo"] = zaloappInfo;

//            // 🔥 Lấy thông tin user từ Zalo API
//            var (isValid, zaloUser, msg) = await zaloApiService.CheckZaloTokenAsync(accessToken, zaloappInfo.zalominiapp_id);
//            if (!isValid)
//            {
//                context.Result = new UnauthorizedObjectResult(new { message = "Invalid Zalo token" });
//                return;
//            }
//            context.HttpContext.Items["ZaloUserInfo"] = zaloUser;

//            // Tiếp tục thực thi nếu không có lỗi
//            await next();
//        }
//    }

//}
