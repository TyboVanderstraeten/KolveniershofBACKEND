using System;
using System.Text;
using KolveniershofBACKEND.Data;
using KolveniershofBACKEND.Data.Repositories.Concrete;
using KolveniershofBACKEND.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using NSwag;
using NSwag.SwaggerGeneration.Processors.Security;

namespace KolveniershofBACKEND
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            #region MVC
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            #endregion

            #region DBContext
            services.AddDbContext<DBContext>(options =>
            {
                // Change 'TyboConnection' to 'DefaultConnection', my SQL-server instance name is different than default
                options.UseSqlServer(Configuration["ConnectionStrings:TyboConnection"]);
            });
            #endregion

            #region Dependency Injections
            services
                .AddScoped<DBInitializer>()
                .AddScoped<IActivityRepository, ActivityRepository>()
                .AddScoped<ICustomDayRepository, CustomDayRepository>()
                .AddScoped<IDayRepository, DayRepository>()
                .AddScoped<IUserRepository, UserRepository>()
                .AddScoped<INoteRepository, NoteRepository>()
                .AddScoped<IDayActivityRepository, DayActivityRepository>()
                .AddScoped<IHelperRepository, HelperRepository>()
                .AddScoped<IWeekendDayRepository, WeekendDayRepository>()
                .AddScoped<IAttendanceRepository, AttendanceRepository>()
                .AddScoped<IBusDriverRepository, BusDriverRepository>();
            #endregion

            #region NSwag
            services.AddOpenApiDocument(d =>
            {
                d.Description = "Back-end API written for Kolveniershof application";
                d.DocumentName = "KolveniershofAPI";
                d.Version = "Development";
                d.Title = "KolveniershofAPI";
                d.DocumentProcessors.Add(new SecurityDefinitionAppender("JWT Token", new SwaggerSecurityScheme
                {
                    Type = SwaggerSecuritySchemeType.ApiKey,
                    Name = "Authorization",
                    In = SwaggerSecurityApiKeyLocation.Header,
                    Description = "Copy 'Bearer' + valid JWT token into field"
                }));
                d.OperationProcessors.Add(new OperationSecurityScopeProcessor("JWT Token"));
            });
            #endregion

            #region Default identity
            services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<DBContext>();
            #endregion

            #region Authentication
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    // Make sure to add a (long-enough) token to user-secrets!
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Tokens:Key"])),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
            #endregion

            #region Identity Configuration
            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 5;
                options.Password.RequiredUniqueChars = 4;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;
            });
            #endregion

            #region CORS
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins", builder => builder.AllowAnyOrigin());
            });
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, DBInitializer dbInitializer)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }


            app.UseHttpsRedirection();
            app.UseAuthentication();

            app.UseMvc();

            app.UseSwaggerUi3();
            app.UseSwagger();

            app.UseCors("AllowAllOrigins");

            dbInitializer.seedDatabase().Wait();
        }
    }
}
