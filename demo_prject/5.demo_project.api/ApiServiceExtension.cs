using azicloud.api;
using azicloud.app.contracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using demo_project.app.contracts;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using ProtoBuf.Grpc.Server;
using System.Text;

namespace demo_project.api
{
    public static class ApiServiceExtension
    {
        public static IServiceCollection AddMiniCdpApi(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpContextAccessor();
            // services.AddControllers();
            services.AddAzicloudApi();

            services.AddAzicloudSwagger();
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("1qazZAQ!-1qazZAQ!-1qazZAQ!-1qazZAQ!")),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                        ClockSkew = TimeSpan.Zero,
                        RequireExpirationTime = false,
                        ValidateLifetime = true

                    };
                });
            services.AddGrpc();
            services.AddCodeFirstGrpc();

            // services.AddSingleton<ResourceDetector>();
            services.AddControllers();
            //.AddNewtonsoftJson();
            services.AddMvcCore().ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = (errorContext) =>
                {
                    var ModelState = errorContext.ModelState;
                    var lang_id = 1;
                    var errorMessage = "";
                    var attribute_value = ModelState.Values.Where(e => e.ValidationState == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Invalid).FirstOrDefault()?.Errors.FirstOrDefault()?.ErrorMessage.Split(","); //range,0
                    var key = attribute_value[0];
                    attribute_value[0] = errorContext.ModelState.Keys.FirstOrDefault()?.ToString();
                   
                    return new OkObjectResult(ResponseDTO.Error(null, 400, errorMessage, errorMessage));
                };
            });
            // services.AddGrpc();
            services.AddGrpcHealthChecks()
                .AddCheck("", () => HealthCheckResult.Healthy());
            // services.AddCodeFirstGrpc();

            //services.AddQueueMassTransit(configuration);

            // services.AddSingleton<ResourceDetector>();

            services.AddOpenTelemetry()
                // .ConfigureResource(builder => builder
                //    .AddDetector(sp => sp.GetRequiredService<ResourceDetector>()))
                .WithTracing(builder => builder
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddSqlClientInstrumentation(options =>
                    {
                        //options.SetDbStatementForStoredProcedure = false;
                        options.SetDbStatementForText = true;
                        // options.EnableConnectionLevelAttributes = true;
                        options.RecordException = true;
                    })
                    //.AddConsoleExporter()
                    .AddOtlpExporter(options =>
                    {
                        // options.Endpoint = new Uri("http://simplest-collector.azicloud-system-dev.svc.cluster.local:4317");
                    }))
                .WithMetrics(builder => builder
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()

                    // .AddConsoleExporter()
                    .AddOtlpExporter(options =>
                    {
                        // options.Endpoint = new Uri("http://simplest-collector.azicloud-system-dev.svc.cluster.local:4317");
                    }));
            return services;
        }

        public static IApplicationBuilder AddZaloAppApplicationBuilder(this IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseAzicloudSwagger();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                //endpoints.MapGrpcService<ZaloAppBookingMenuRpcAppService>();
                endpoints.MapGrpcHealthChecksService();
            });
            return app;
        }

        private static IServiceCollection AddQueueMassTransit(this IServiceCollection services, IConfiguration configuration)
        {
            var clusterConfig = configuration.GetSection("ClusterConfig").Get<ClusterConfiguration>();
            var rabbitMqConfig = clusterConfig.RabbitMQ;

            return services;
        }
    }
}
