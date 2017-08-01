using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using NgStore.API.Data;
using NgStore.API.Services;
using AutoMapper;
using NgStore.API.Entities;
using NgStore.API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace NgStore.API
{
    public class Startup
    {
        private IConfigurationRoot _config;

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables();

            _config = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(_config);

            services.AddCors(opt =>
            {
                opt.AddPolicy("AllowNgStore.Client", cfg =>
                {
                    cfg.WithOrigins("http://localhost:4200")
                       .AllowAnyHeader()
                       .AllowAnyMethod();
                });
            });

            services.AddAuthorization(cfg =>
            {
                cfg.AddPolicy("NgStoreWorkers", p => p.RequireClaim("NgStoreWorker", "true"));
            });

            services.AddMvc();

            services.AddDbContext<NgStoreDBContext>(opt => opt.UseSqlServer(_config.GetConnectionString("NgStore")));

            services.AddScoped<INgStoreRepository, NgStoreRepository>();

            services.AddIdentity<User, IdentityRole>()
                    .AddEntityFrameworkStores<NgStoreDBContext>();

            services.AddTransient<UserSeeder>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, UserSeeder seeder)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Customer, CustomerDto>();
                cfg.CreateMap<Order, OrderDto>();
                cfg.CreateMap<OrderItem, OrderItemDto>();
                cfg.CreateMap<Product, ProductDto>();
                cfg.CreateMap<OrderItemPostDto, OrderItem>();
            });

            app.UseIdentity();

            app.UseJwtBearerAuthentication(new JwtBearerOptions()
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidIssuer = _config["Tokens:Issuer"],
                    ValidAudience = _config["Tokens:Audience"],
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"])),
                    ValidateLifetime = true
                }
            });

            app.UseMvc();

            seeder.Seed().Wait();
        }
    }
}
