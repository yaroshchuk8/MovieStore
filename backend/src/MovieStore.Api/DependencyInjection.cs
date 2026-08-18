using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Amazon.S3;
using ErrorOr;
using FileSignatures;
using FileSignatures.Formats;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MovieStore.Api.Actors.Commands;
using MovieStore.Api.Common.Extensions;
using MovieStore.Api.Configuration;
using MovieStore.Api.Genres.Commands.CreateGenre;
using MovieStore.Api.Genres.Queries.GetGenres;
using MovieStore.Api.Handlers;
using MovieStore.Api.OpenApi.Transformers;
using MovieStore.Application.Common.Extensions;
using MovieStore.Application.Common.Interfaces;
using MovieStore.Application.Users.Commands;
using MovieStore.Application.Users.Commands.LoginUser;
using MovieStore.Application.Users.Commands.RefreshAuthTokens;
using MovieStore.Application.Users.Commands.RegisterUser;
using MovieStore.Application.Users.DTOs;
using MovieStore.Application.Users.Interfaces;
using MovieStore.Domain.Common;
using MovieStore.Infrastructure.Common.Configurations;
using MovieStore.Infrastructure.Common.Persistence;
using MovieStore.Infrastructure.Common.Services;
using MovieStore.Infrastructure.Users.Persistence.Identity.Entities;
using MovieStore.Infrastructure.Users.Services;

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
        
        public IServiceCollection AddApplicationLayerDependencies()
        {
            return services
                .AddFluentValidation()
                .AddFileSecurity();
        }

        private IServiceCollection AddFluentValidation()
        {
            services.AddValidatorsFromAssembly(typeof(Api.DependencyInjection).Assembly);
            // services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            return services;
        }
        
        private IServiceCollection AddFileSecurity()
        {
            var inspector = new FileFormatInspector(
                [
                    new Png(),
                    new Jpeg(), 
                    new MP4()
                ]
            );

            services.AddSingleton<IFileFormatInspector>(inspector);
            return services;
        }
        
        public IServiceCollection AddInfrastructureLayerDependencies(IConfiguration configuration)
        {
            return services
                .AddPersistence(configuration)
                .AddIdentity()
                .AddServices()
                .AddS3Client(configuration);
        }
        
        private IServiceCollection AddPersistence(IConfiguration configuration)
        {
            var dbSettings = configuration.GetSection(nameof(DbSettings)).Get<DbSettings>()!;
            services.AddDbContext<MovieStoreDbContext>(options => options.UseSqlServer(dbSettings.ConnectionString));
            
            return services;
        }

        private IServiceCollection AddIdentity()
        {
            services
                .AddIdentityCore<IdentityUserEntity>(options => 
                {
                    options.User.RequireUniqueEmail = true;
                })
                .AddRoles<IdentityRoleEntity>()
                .AddEntityFrameworkStores<MovieStoreDbContext>();

            return services;
        }

        private IServiceCollection AddServices()
        {
            return services
                // .AddScoped<IFileService, FileService>()
                .AddScoped<IFileService, S3FileService>()
                .AddScoped<IIdentityService, IdentityService>()
                .AddScoped<IJwtService, JwtService>()
                .AddScoped<IDbInitializer, DbInitializer>()
                .AddScoped<ICurrentUserProvider, CurrentUserProvider>()
                .AddScoped<IFileStorageInitializer,  S3Initializer>();
        }

        private IServiceCollection AddS3Client(IConfiguration configuration)
        {
            var s3Settings = configuration.GetSection(nameof(S3Settings)).Get<S3Settings>()!;
            var s3Config = new AmazonS3Config
            {
                ServiceURL = s3Settings.Endpoint, // e.g., "http://minio:9000"
                ForcePathStyle = true,
                UseHttp = true,
            };

            services.AddSingleton<IAmazonS3>(new AmazonS3Client(s3Settings.AccessKey, s3Settings.SecretKey, s3Config));

            services.AddScoped<IFileService, S3FileService>();
    
            return services;
        }

        public IServiceCollection AddRequestHandlers()
        {
            services.AddDecoratedRequestHandler<CreateActorCommand, Success, CreateActorCommandHandler>();
            services.AddDecoratedRequestHandler<CreateGenreCommand, Success, CreateGenreCommandHandler>();
            services.AddDecoratedRequestHandler<GetGenresQuery, PagedList<GetGenresQueryDto>, GetGenresQueryHandler>();
            services.AddDecoratedRequestHandler<CreatePublisherProfileCommand, Success, CreatePublisherProfileCommandHandler>();
            services.AddDecoratedRequestHandler<LoginUserCommand, AuthTokens, LoginUserCommandHandler>();
            services.AddDecoratedRequestHandler<RefreshAuthTokensCommand, AuthTokens, RefreshAuthTokensCommandHandler>();
            services.AddDecoratedRequestHandler<RegisterUserCommand, AuthTokens, RegisterUserCommandHandler>();
            
            return services;
        }
    }
}