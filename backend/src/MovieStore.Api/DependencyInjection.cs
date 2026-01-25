using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MovieStore.Api.Configuration;
using MovieStore.Api.Handlers;
using MovieStore.Api.OpenApi.Transformers;
using MovieStore.Application.Common.Extensions;
using MovieStore.Infrastructure.Common.Configurations;

namespace MovieStore.Api;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddAndValidateConfiguration(IConfiguration configuration)
        {
            // used during startup
            configuration.ValidateRequiredSection<CorsSettings>(nameof(CorsSettings));
            configuration.ValidateRequiredSection<DbSettings>(nameof(DbSettings));
            
            // used during runtime
            var jwtSettingsSection = configuration.GetAndValidateRequiredSection<JwtSettings>(nameof(JwtSettings));
            var fileStorageSettingsSection =
                configuration.GetAndValidateRequiredSection<FileStorageSettings>(nameof(FileStorageSettings));
            var refreshTokenSettingsSection =
                configuration.GetAndValidateRequiredSection<RefreshTokenSettings>(nameof(RefreshTokenSettings));
            var s3Settings = configuration.GetAndValidateRequiredSection<S3Settings>(nameof(S3Settings));

            services.Configure<JwtSettings>(jwtSettingsSection);
            services.Configure<FileStorageSettings>(fileStorageSettingsSection);
            services.Configure<RefreshTokenSettings>(refreshTokenSettingsSection);
            services.Configure<S3Settings>(s3Settings);
            
            return services;
        }

        public IServiceCollection AddApiLayerDependencies(IConfiguration configuration)
        {
            return services
                .AddGlobalExceptionHandler()
                .AddOpenApiWithTransformers()
                .AddJwtAuth(configuration)
                .AddCorsPolicy(configuration)
                .AddHttpContextAccessor();
        }
        
        private IServiceCollection AddCorsPolicy(IConfiguration configuration)
        {
            var corsSettings = configuration.GetSection(nameof(CorsSettings)).Get<CorsSettings>()!;
            
            services.AddCors(opt =>
            {
                opt.AddPolicy(corsSettings.PolicyName, policy =>
                {
                    policy.AllowAnyHeader().AllowAnyMethod().WithOrigins(corsSettings.AllowedOrigins);
                });
            });

            return services;
        }

        private IServiceCollection AddJwtAuth(IConfiguration configuration)
        {
            services.AddAuthorization();
            
            // Prevents "sub" from being renamed to "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            
            var jwtSettings = configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>()!;
            var key = Encoding.ASCII.GetBytes(jwtSettings.Secret);
        
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = true;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidateAudience = true,
                        ValidAudience = jwtSettings.Audience,
                        ValidateLifetime = true
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            Console.WriteLine($"AUTH FAILED: {context.Exception.Message}");
                            return Task.CompletedTask;
                        },
                        OnTokenValidated = _ =>
                        {
                            Console.WriteLine("TOKEN VALIDATED");
                            return Task.CompletedTask;
                        }
                    };
                });
        
            return services;
        }

        private IServiceCollection AddGlobalExceptionHandler()
        {
            return services
                .AddExceptionHandler<GlobalExceptionHandler>()
                .AddProblemDetails(); // Required for structured error responses
        }

        private IServiceCollection AddOpenApiWithTransformers()
        {
            return services.AddOpenApi(options =>
            {
                // Everything below is needed for OpenAPI generated documentation 
        
                // Defines the "Authorize" globally
                options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();

                // Registers required types
                options.AddDocumentTransformer<SchemaRegistrationTransformer>();

                options.AddSchemaTransformer<EnumSchemaTransformer>();
    
                // Applies the padlock icon to specific [Authorize] endpoints and documents 401/403 responses
                options.AddOperationTransformer<SecurityRequirementsTransformer>();

                // Applies pagination header for all endpoints with [ProvidesPaginationHeader] marker attribute
                options.AddOperationTransformer<PaginationHeaderTransformer>();
        
                // Applies 500 Internal Error response for all endpoints
                options.AddOperationTransformer<InternalServerErrorTransformer>();

                // Applies 400 Bad Request response for all endpoint with at least one parameter
                options.AddOperationTransformer<ValidationErrorTransformer>();
            });
        }
    }
}