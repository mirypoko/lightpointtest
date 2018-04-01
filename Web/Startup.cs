using System;
using System.IO;
using System.Threading.Tasks;
using AspNetIdentityAuthenticationServices;
using Database.EntityFrameworkCore;
using Domain.DataBaseModels.Identity;
using Domain.Identity;
using Domain.Services.Interfaces;
using EntityFrameworkLoggingService;
using MailKitEmailService;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using ProductsServices;
using Servises.Interfaces;
using Servises.Interfaces.AuthenticationServices;
using Servises.Interfaces.Products;
using Swashbuckle.AspNetCore.Swagger;
using Utils.NonStatic;
using Web.Initializers;
using Web.Logging.DataBaseLogger;
using Web.Logging.FileLogger;
using Web.Middlewares;

namespace Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new Info
                    {
                        Title = "lightpointtest",
                        Description = "Тестовое задание для Lightpoint",
                        Contact = new Contact { Name = "Dmitry Protko", Email = "mirypoko@gmail.com", Url = "https://github.com/mirypoko/lightpointtest" },
                        Version = "v1"
                    });

                var basePath = AppContext.BaseDirectory;

                var assemblyName = System.Reflection.Assembly.GetEntryAssembly().GetName().Name;
                var fileName = Path.GetFileName(assemblyName + ".xml");
                c.IncludeXmlComments(Path.Combine(basePath, fileName));
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, UserRole>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequiredLength = 8;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.User.RequireUniqueEmail = true;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();


            services.Configure<SecurityStampValidatorOptions>(options
                => options.ValidationInterval = TimeSpan.FromSeconds(600));



            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

                })
                //.AddCookie(
                //    options =>
                //    {
                //        options.SlidingExpiration = true;

                //        options.Cookie.Expiration = TimeSpan.FromMinutes(10);

                //        options.Events.OnRedirectToLogin = (context) =>
                //        {
                //            context.Response.StatusCode = 401;
                //            return Task.CompletedTask;
                //        };

                //    })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;

                    options.SaveToken = true;

                    options.TokenValidationParameters = new TokenValidationParameters
                    {

                        ValidateIssuer = true,
                        ValidIssuer = AuthJwtOptions.Issuer,

                        ValidateAudience = true,
                        ValidAudience = AuthJwtOptions.Audience,

                        ValidateLifetime = true,

                        IssuerSigningKey = AuthJwtOptions.SymmetricSecurityKey,
                        ValidateIssuerSigningKey = true,

                        ClockSkew = TimeSpan.Zero
                    };

                });

            //services.ConfigureApplicationCookie(options =>
            //{
            //    options.Events.OnRedirectToLogin = context =>
            //    {
            //        context.Response.StatusCode = 401;
            //        return Task.CompletedTask;
            //    };
            //});

            //services.AddMvc(config =>
            //{
            //    var policy = new AuthorizationPolicyBuilder()
            //        .RequireAuthenticatedUser()
            //        .Build();
            //    config.Filters.Add(new AuthorizeFilter(policy));
            //});

            services.AddMvc();

            // In production, the Angular files will be served from this directory
            //services.AddSpaStaticFiles(configuration =>
            //{
            //    configuration.RootPath = "ClientApp/dist";
            //});

            services.AddTransient<IGenericUnitOfWork, GenericUnitOfWork>();

            services.AddTransient<IProductsService, ProductsService>();
            services.AddTransient<IShopModesService, ShopModesService>();
            services.AddTransient<IShopsService, ShopsService>();

            services.AddTransient<IHttpUtilsService, HttpUtilsService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ISignInService, SignInService>();
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<IJwtTokensService, JwtTokensService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<ILoggingService, LoggingService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseMiddleware<LogExceptionHandlerMiddleware>();

            Enum.TryParse(Configuration["LogLevel"], true, out LogLevel logLevel);
            loggerFactory.AddConsole(logLevel);
            loggerFactory.AddDebug(logLevel);
            loggerFactory.AddContext(logLevel, Configuration.GetConnectionString("DefaultConnection"));

            if (env.IsDevelopment())
            {
                loggerFactory.AddFile(Path.Combine(Directory.GetCurrentDirectory(), "logger.txt"), logLevel);
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseAuthentication();

            app.UseStaticFiles();

            app.UseDefaultFiles();

            //app.UseSpaStaticFiles();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Web V1");
            });

            app.UseMvcWithDefaultRoute();

            app.Run(async (context) =>
            {
                context.Response.ContentType = "text/html";
                await context.Response.SendFileAsync(Path.Combine(env.WebRootPath, "index.html"));
            });

            //app.UseSpa(spa =>
            //{
            //    spa.Options.SourcePath = "ClientApp";

            //    if (env.IsDevelopment())
            //    {
            //        spa.UseAngularCliServer(npmScript: "start");
            //    }
            //});
        }
    }
}
