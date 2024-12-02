
using API127.Data;
using API127.Logging;
using API127.Repository;
using API127.Repository.IRepository;
using BasicAuth.API;
using ERP.Infrastructure.Persistence.Interceptors;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using System.Text;

namespace API127
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers(options =>
                {
                    //    options.ReturnHttpNotAcceptable = true;
                }
            ).AddNewtonsoftJson()
            .AddXmlDataContractSerializerFormatters();

            Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .WriteTo.File("log/villagLog.txt", rollingInterval: RollingInterval.Day).CreateLogger();

            builder.Host.UseSerilog();

            builder.Services.AddDbContext<ApplicationDbContext>(option =>
            {
                option.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnectionStr"));
            });
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddSingleton<ILogging, Logg>();
            builder.Services.AddAutoMapper(typeof(MappingConfig));
            builder.Services.AddScoped<AuditableSaveChangesInterceptor>();
            builder.Services.AddScoped<IVillaRepository, VillaRepository>();
            builder.Services.AddScoped<IVillaRepositoryV2, VillaRepositoryV2>();
            builder.Services.AddScoped<IVillaNumberRepository, VillaNumberRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));
            builder.Services.AddHttpContextAccessor();
            builder.WebHost.ConfigureKestrel(serverOptions =>
            {
                serverOptions.Limits.MaxRequestBodySize = long.MaxValue;
            });
            builder.Services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = 5L * 1024 * 1024 * 1024;
            });

            // Đăng nhập trong swagger
            builder.Services.AddSwaggerGen(options =>
            {
                #region Bearer Authen 
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description =
                        "JWT Authorization header using the Bearer scheme. \r\n\r\n " +
                        "Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\n" +
                        "Example: \"12345abcdef\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Scheme = "Bearer"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });
                #endregion

                #region Basic Authen user/pass : test/test
                //options.AddSecurityDefinition("basics", new OpenApiSecurityScheme
                //{
                //    Name = "Authorization",
                //    Type = SecuritySchemeType.Http,
                //    Scheme = "basic",
                //    In = ParameterLocation.Header,
                //    Description = "Basic Authorization header using the Bearer scheme. Bao/1234"
                //});
                //options.AddSecurityRequirement(new OpenApiSecurityRequirement
                //{
                //    {
                //          new OpenApiSecurityScheme
                //            {
                //                Reference = new OpenApiReference
                //                {
                //                    Type = ReferenceType.SecurityScheme,
                //                    Id = "basics"
                //                }
                //            },
                //            new string[] {}
                //    }
                //});
                #endregion

            });

            #region Basic Authen user/pass : test/test
            //builder.Services.AddAuthentication("BasicAuthentication_hihi")
            //    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication_hihi", null);
            #endregion

            builder.WebHost.ConfigureKestrel(serverOptions =>
            {
                serverOptions.Limits.MaxRequestBodySize = long.MaxValue;
            });



            #region Bearer Authen 

            var key = builder.Configuration.GetValue<string>("ApiSettings:Secret");

            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.SaveToken = true;
                x.RequireHttpsMetadata = false;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            #endregion

            #region Remove header Server
            //builder.WebHost.UseKestrel(option => option.AddServerHeader = false);
            #endregion
            var app = builder.Build();


            app.Use(async (context, next) =>
            {
                context.Response.OnStarting(state =>
                {
                    var httpcontext = (HttpContext)state;
                    // Remove 2 cái này kết quả ra ko theo format
                    //httpcontext.Response.Headers.Remove("Content-length");
                    httpcontext.Response.Headers.Add("Age","3");
                    return Task.CompletedTask;
                }, context);
                await next.Invoke(context);
            });
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            
            
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            //AutoInitMigration.InitMigration(app).Wait();
            app.Run();
        }
        public class AutoInitMigration
        {
            public static async Task InitMigration(IApplicationBuilder applicationBuilder)
            {
                using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
                {
                    var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    await context.Database.MigrateAsync();
                }
            }
        }
    }
}