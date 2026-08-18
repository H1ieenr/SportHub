using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using sporthub.app.contracts;
using Microsoft.AspNetCore.Http;
using Shared.Common;
namespace sporthub.api
{
    [Route("api/v1/sporthub/customer/auth/")]
    public class AuthCustomerAppServiceController : ApiControllerBase
    {
        private readonly ICustomerAuthAppService _customerAuthAppService;

        public AuthCustomerAppServiceController(ICustomerAuthAppService customerAuthAppService, IHttpContextAccessor httpContextAccessor)
        {
            _customerAuthAppService = customerAuthAppService;
        }
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO model, CancellationToken cancellationToken)
        {
            return await HandleAsync(model, _customerAuthAppService.LoginAsync, cancellationToken);
        }
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterRequestDTO model, CancellationToken cancellationToken)
        {
            return await HandleAsync(model, _customerAuthAppService.RegisterAsync, cancellationToken);
        }
    }
}