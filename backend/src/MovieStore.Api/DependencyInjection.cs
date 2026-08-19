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
using MovieStore.Api.Actors.Commands.CreateActor;
using MovieStore.Api.Common.Configuration;
using MovieStore.Api.Common.ErrorHandling;
using MovieStore.Api.Common.FileStorage;
using MovieStore.Api.Common.OpenApi.Transformers;
using MovieStore.Api.Common.Pagination;
using MovieStore.Api.Common.Persistence;
using MovieStore.Api.Common.Pipeline;
using MovieStore.Api.Genres.Commands.CreateGenre;
using MovieStore.Api.Genres.Queries.GetGenres;
using MovieStore.Api.Users.Commands.CreatePublisherProfile;
using MovieStore.Api.Users.Commands.LoginUser;
using MovieStore.Api.Users.Commands.RefreshAuthTokens;
using MovieStore.Api.Users.Commands.RegisterUser;
using MovieStore.Api.Users.DTOs;
using MovieStore.Api.Users.Entities.Identity;
using MovieStore.Api.Users.Services;
using MovieStore.Api.Users.Services.Interfaces;

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
        
        public IServiceCollection AddExceptionHandling()
        {
            return services
                .AddExceptionHandler<GlobalExceptionHandler>()
                .AddProblemDetails(); // Required for structured error responses
        }
        
        public IServiceCollection AddOpenApiDocumentation()
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

        public IServiceCollection AddJwtAuthentication(IConfiguration configuration)
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
        
        public IServiceCollection AddCorsPolicy(IConfiguration configuration)
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
        
        public IServiceCollection AddPersistence(IConfiguration configuration)
        {
            var dbSettings = configuration.GetSection(nameof(DbSettings)).Get<DbSettings>()!;
            services.AddDbContext<MovieStoreDbContext>(options => options.UseSqlServer(dbSettings.ConnectionString));
            
            services.AddScoped<IDbInitializer, DbInitializer>();
            
            return services;
        }
        
        public IServiceCollection AddIdentity()
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
        
        public IServiceCollection AddFileStorage(IConfiguration configuration)
        {
            var inspector = new FileFormatInspector(
                [
                    new Png(),
                    new Jpeg(), 
                    new MP4()
                ]
            );
            services.AddSingleton<IFileFormatInspector>(inspector);
            
            var s3Settings = configuration.GetSection(nameof(S3Settings)).Get<S3Settings>()!;
            var s3Config = new AmazonS3Config
            {
                ServiceURL = s3Settings.Endpoint, // e.g., "http://minio:9000"
                ForcePathStyle = true,
                UseHttp = true,
            };
            services.AddSingleton<IAmazonS3>(new AmazonS3Client(s3Settings.AccessKey, s3Settings.SecretKey, s3Config));

            services.AddScoped<IFileService, S3FileService>();
            services.AddScoped<IFileStorageInitializer>();
            
            return services;
        }

        public IServiceCollection AddFluentValidation()
        {
            services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
            return services;
        }
        
        public IServiceCollection AddUserServices()
        {
            return services
                .AddScoped<IIdentityService, IdentityService>()
                .AddScoped<IJwtService, JwtService>()
                .AddScoped<ICurrentUserProvider, CurrentUserProvider>();
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