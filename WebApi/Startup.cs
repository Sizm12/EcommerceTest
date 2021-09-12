using BusinessLogic.Data;
using BusinessLogic.Logic;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.DTO;
using WebApi.Middleware;

namespace WebApi
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
            services.AddScoped<IUnitofWork, UnitofWork>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IOrdenCompraServices, OrdenCompraService>();
            var builder = services.AddIdentityCore<Usuario>();
            builder = new IdentityBuilder(builder.UserType, builder.Services);
            builder.AddRoles<IdentityRole>();
            builder.AddEntityFrameworkStores<SecurityDbContext>();
            builder.AddSignInManager<SignInManager<Usuario>>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Token:Key"])),
                    ValidIssuer = Configuration["Token:Issuer"],
                    ValidateIssuer = true,
                    ValidateAudience = false
                };
            });

            services.AddAutoMapper(typeof(MappingProfiles));
            services.AddScoped(typeof(IGenericRepository<>), (typeof(GenericRepository<>)));
            services.AddScoped(typeof(IGenericSecurityRepository<>), (typeof(GenericSecurityRepository<>)));
            services.AddDbContext<EcommerceDbContext>(opt =>
            {
                opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddDbContext<SecurityDbContext>(x =>
            {
                x.UseSqlServer(Configuration.GetConnectionString("IdentitySecurity"));
            });

            services.AddSingleton<IConnectionMultiplexer>(c =>
            {
                var configuration = ConfigurationOptions.Parse(Configuration.GetConnectionString("Redis"), true);
                return ConnectionMultiplexer.Connect(configuration);

            });

            services.TryAddSingleton<ISystemClock, SystemClock>();
            services.AddTransient<IProductoRepository, ProductoRepository>();
            services.AddControllers();

            services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();
            services.AddCors(
                opt =>
                {
                    opt.AddPolicy("CorsRule", rule =>
                    {
                        rule.AllowAnyHeader().AllowAnyMethod().WithOrigins("*");
                    });
                });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseStatusCodePagesWithReExecute("/errors", "?code={0}");

            app.UseRouting();
            app.UseCors("CorsRule");

            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
