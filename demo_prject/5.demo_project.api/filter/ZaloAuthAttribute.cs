//using Microsoft.AspNetCore.Mvc.Filters;
//using Microsoft.AspNetCore.Mvc;
//using System;
//using System.Threading.Tasks;
//using demo_project.app.contracts;
//using Microsoft.Extensions.DependencyInjection;
//using azicloud.zaloauth;
//using azicloud.grpc.client;

//namespace demo_project.api
//{
//    public class ZaloAuthAttribute : Attribute, IAsyncActionFilter
//    {
//        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
//        {
//            // 🔥 Lấy các service cần thiết một lần
//            var serviceProvider = context.HttpContext.RequestServices;
//            var zaloApiService = serviceProvider.GetService<IZaloApiService>();
//            var tenantGrpcService = serviceProvider.GetService<ITenantGrpcClientAppService>();

//            if (zaloApiService == null || tenantGrpcService == null)
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

//            if (!context.ActionArguments.TryGetValue("model", out var modelObj) || modelObj is not MiniformBaseRequestDTO model)
//            {
//                context.Result = new BadRequestObjectResult(new { message = "Invalid request body" });
//                return;
//            }

//            // 🔥 Đọc `lang_id` và `miniform_id` từ model trực tiếp
//            var langId = model.lang_id;
//            var miniapp_id = model.miniapp_id;
//            var tenant_public_id = model.tenant_public_id;


//            if (langId <= 0 || string.IsNullOrEmpty(miniapp_id) || tenant_public_id==null)
//            {
//                context.Result = new BadRequestObjectResult(new { message = "Missing lang_id hoặc miniapp_id hoặc tenant_public_id" });
//                return;
//            }
//            // 🔥 Lấy thông tin `miniform` từ service
//            var tenantByPublicId = await tenantGrpcService.TenantRpcClient().tenantByPublicId_Row(new azicloud.grpc.TenantWebByPublicIdRowRpcRequest
//            {
//                public_id = model.tenant_public_id,
//                lang_id= langId,
//            });
//            var tenant_id = tenantByPublicId.id;
//            var miniformInfo = await tenantGrpcService.AppConfigClient().app_configByObjectId_Row(new azicloud.grpc.AppConfigByObjectIdRowRpcRequest
//            {
//                module_category_code = "apps.miniform",
//                object_id = miniapp_id,
//                object_key = "zalominiapp",
//                tenant_id = tenant_id,
//                lang_id = langId
//            });
//            if (miniformInfo == null)
//            {
//                context.Result = new UnauthorizedObjectResult(new { message = "Consumer not found" });
//                return;
//            }
//            context.HttpContext.Items["MiniformInfo"] = miniformInfo;

//            // 🔥 Lấy thông tin user từ Zalo API

//            //var zaloUser = await zaloApiService.CheckZaloTokenAsync(accessToken, miniformInfo.zalominiapp_id);
//            var (isValid, zaloUser, message) = await zaloApiService.CheckZaloTokenAsync(accessToken, miniformInfo.object_id);
//            if (zaloUser == null || string.IsNullOrEmpty(zaloUser.id))
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
