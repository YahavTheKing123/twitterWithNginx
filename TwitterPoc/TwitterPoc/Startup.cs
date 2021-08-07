using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TwitterPoc.Authorization;
using TwitterPoc.Data.Interfaces;
using TwitterPoc.Data.Repositories;
using TwitterPoc.Data.Settings;
using TwitterPoc.Initialization;
using TwitterPoc.Logic;
using TwitterPoc.Logic.Interfaces;
using TwitterPoc.Logic.Services;
using TwitterPoc.Middlewares;

namespace TwitterPoc
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

            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TwitterPoc", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}
                    }
                });
            });


            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                };
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var headers = context.Request.Headers;
                        if (!headers.ContainsKey(Constants.AuthorizationHeaderKey) && context.Request.Cookies.ContainsKey(Constants.CookieAccessTokenKey))
                        {
                            context.Request.Headers.Add(Constants.AuthorizationHeaderKey, "Bearer " + context.Request.Cookies[Constants.CookieAccessTokenKey]);
                        }
                        return Task.CompletedTask;
                    }
                };

            });
            

            services.AddCors(options =>
              {
                  options.AddDefaultPolicy(
                  builder =>
                  {
                      builder.WithOrigins("http://localhost:4200", "https://localhost:4200", "https://localhost:8080", "http://localhost:8080", "https://localhost", "http://localhost")
                      .AllowAnyHeader()
                      .AllowAnyMethod()
                      .AllowCredentials();
                  });
              });

            services.Configure<TwitterPocDatabaseSettings>(
                Configuration.GetSection(nameof(TwitterPocDatabaseSettings)));

            services.AddSingleton<ITwitterPocDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<TwitterPocDatabaseSettings>>().Value);

            services.AddScoped<ITokenService, TokenService>()
                    .AddScoped<IUsersRepository, UsersRepository>()
                    .AddScoped<IFollowersRepository, FollowersRepository>()
                    .AddScoped<IMessagesRepository, MessagesRepository>()
                    .AddScoped<IMessagesRepository, MessagesRepository>()
                    .AddScoped<IUsersService, UsersService>()
                    .AddScoped<IFeedsService, FeedsService>()
                    .AddTransient<IDataInitializationService,DataInitializationService>()

                    ;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger, IDataInitializationService dataInitializationService)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TwitterPoc v1"));

            app.UseRouting();

            app.UseAuthorization();

            app.ConfigureExceptionHandler(logger);

            app.UseCors();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            dataInitializationService.AddSampleDataIfEmptyProject();
        }
    }
}
