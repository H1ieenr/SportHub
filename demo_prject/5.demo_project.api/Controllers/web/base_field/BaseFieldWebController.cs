using azicloud.app.contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using demo_project.app.contracts;

namespace demo_project.api
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiversion}/mini-form/base-field")]
    public class BaseFieldWebController : BaseAppController
    {
        private readonly IBaseFieldWebAppService _baseFieldWebAppService;
        public BaseFieldWebController(
            IBaseFieldWebAppService baseFieldWebAppService,
            IHttpContextAccessor httpContextAccessor,
            ILogger<BaseFieldWebController> logger)
            : base(httpContextAccessor, logger)
        {
            _baseFieldWebAppService = baseFieldWebAppService;
        }
        [HttpPost("create")]
        public async Task<IActionResult> fn_base_field_web_create(
            [FromBody] FnBaseFieldWebCreateRequestDTO model)
        {
            return await ReturnOk(async () =>
            {
                var response = new ResponseDTO();

                var result = await _baseFieldWebAppService.fn_base_field_web_create(model);
                if (result.result > 0)
                {
                    return ResponseDTO.Success(result);
                }
                else
                {
                    return ResponseDTO.Error(null, result.result, result.message, result.message);
                }
            }, model);
        }

        [HttpPost("delete")]
        public async Task<IActionResult> fn_base_field_web_delete(
            [FromBody] FnBaseFieldWebDeleteRequestDTO model)
        {
            return await ReturnOk(async () =>
            {
                var response = new ResponseDTO();

                var result = await _baseFieldWebAppService.fn_base_field_web_delete(model);
                if (result.result > 0)
                {
                    return ResponseDTO.Success(result);
                }
                else
                {
                    return ResponseDTO.Error(null, result.result, result.message, result.message);
                }
            }, model);
        }
        [HttpPost("view")]
        public async Task<IActionResult> fn_base_field_web_get_by_id(
            [FromBody] FnBaseFieldWebGetByIdRequestDTO model)
        {
            return await ReturnOk(async () =>
            {
                var response = new ResponseDTO();

                var result = await _baseFieldWebAppService.fn_base_field_web_get_by_id(model);
                if (result!=null && result.id>0)
                {
                    return ResponseDTO.Success(result);
                }
                else
                {
                    return ResponseDTO.Error(null, -404, "Item Not Found", "Item Not Found");
                }
            }, model);
        }
    }
}


[ApiController]
[Route("api/test")]
public class TestController : ControllerBase
{
    [HttpGet]
    public string Get() => "ok";
}