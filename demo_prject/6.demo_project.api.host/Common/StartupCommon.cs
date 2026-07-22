//using azicloud.library;
//using pos.app;
//using demo_project.repository;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Options;
//using Microsoft.IdentityModel.Tokens;
//using Microsoft.OpenApi.Models;
//using Swashbuckle.AspNetCore.SwaggerGen;
//using System;
//using System.Text;
//using azicloud.grpc.client;

//namespace demo_project.api.host.Common
//{
//    public static class StartupCommon
//    {
//        public static void ConfigureServicesCommon(IServiceCollection services, IConfiguration configuration)
//        {
//            services.AddApiVersioning(
//             options =>
//             {
//                    // reporting api versions will return the headers "api-supported-versions" and "api-deprecated-versions"
//                    options.DefaultApiVersion = new ApiVersion(2, 0);
//                 options.AssumeDefaultVersionWhenUnspecified = true;
//                 options.ReportApiVersions = true;
//             });
//            services.AddVersionedApiExplorer(
//                options =>
//                {
//                    // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
//                    // note: the specified format code will format the version as "'v'major[.minor][-status]"
//                    options.GroupNameFormat = "'v'VVV";

//                    // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
//                    // can also be used to control the format of the API version in route templates
//                    options.SubstituteApiVersionInUrl = true;
//                });
//            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

//            services.AddAuthentication(x =>
//            {
//                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
//                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

//            })
//                .AddJwtBearer(options =>
//                {
//                    options.RequireHttpsMetadata = false;
//                    options.SaveToken = true;
//                    options.TokenValidationParameters = new TokenValidationParameters
//                    {
//                        ValidateIssuerSigningKey = true,
//                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(new TokenService().securityKey)),
//                        ValidateIssuer = false,
//                        ValidateAudience = false,
//                        // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
//                        ClockSkew = TimeSpan.Zero,
//                        RequireExpirationTime = false,
//                        ValidateLifetime = true

//                    };
//                });
//            services.AddSwaggerGen(c =>
//            {
//                //c.SwaggerDoc("v1", new OpenApiInfo { Title = "Res WebGateway", Version = "v1" });
//                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
//                {
//                    Name = "Authorization",
//                    In = ParameterLocation.Header,
//                    Type = SecuritySchemeType.ApiKey,
//                    Scheme = JwtBearerDefaults.AuthenticationScheme
//                });
//                c.AddSecurityRequirement(new OpenApiSecurityRequirement
//                {
//                    {
//                        new OpenApiSecurityScheme
//                    {
//                        Reference = new OpenApiReference
//                        {
//                            Type = ReferenceType.SecurityScheme,
//                            Id = JwtBearerDefaults.AuthenticationScheme
//                        }
//                    },
//                    new string[] {}
//                    }

//                });
//                c.OperationFilter<SwaggerDefaultValues>();
//            });

//            _ = services.AddScoped<IDapperService>(sp => new DapperService(configuration.GetConnectionString("appsConStr")));
//            //_ = services.AddScoped<IDapperService, IDapperService>();

//            services.AddHttpContextAccessor();
//            services.AddPosRepository(configuration);
//            services.AddPosAppService(configuration);
//            services.AddGrpcClientFromConfig(configuration);
//        }
//    }
//}
