using Dapper;
using Microsoft.Extensions.Configuration;
using demo_project.domain;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace demo_project.repository
{
    public class BaseFieldWebRepository : PgBaseRepository, IBaseFieldWebRepository
    {
        private readonly IConfiguration _configuration;
        public BaseFieldWebRepository(IConfiguration configuration) : base(configuration)
        {

            _configuration = configuration;
        }


        public async Task<ActionFunction> fn_base_field_web_create(FnBaseFieldWebCreateParamsFunction paramsFunction, CancellationToken cancellation = default)
        {
            var sql = "public.fn_base_field_web_create";
            return await ExecuteFunctionAsync<ActionFunction>(sql, paramsFunction, cancellation);
        }

        public async Task<ActionFunction> fn_base_field_web_delete(FnBaseFieldWebDeleteParamsFunction paramsFunction, CancellationToken cancellation = default)
        {
            var sql = "public.fn_base_field_web_delete";
            return await ExecuteFunctionAsync<ActionFunction>(sql, paramsFunction, cancellation);
        }
        public async Task<FnBaseFieldWebGetByIdFunction> fn_base_field_web_get_by_id(FnBaseFieldWebGetByIdParamsFunction paramsFunction, CancellationToken cancellation = default)
        {
            var sql = "public.fn_base_field_web_get_by_id";
            return await ExecuteFunctionAsync<FnBaseFieldWebGetByIdFunction>(sql, paramsFunction, cancellation);
        }





    }
}
