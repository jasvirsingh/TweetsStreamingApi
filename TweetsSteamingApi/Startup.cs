using TweetsAccessAPI.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using TweetsAccessApi.Model;
using TweetsAccessApi.Model.Services.Interfaces;
using TweetsAccessApi.Model.Services;
using System.IO;
using Microsoft.OpenApi.Models;
using TweetsSteamingApi.Security;
using TweetsAccessApi.Common;
using TweetsSteamingApi.Extensions;
using Serilog;
using TweetsSteamingApi.Middleware;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Reflection;
using TwitterAccessApi.Integration.Interfaces;
using TwitterAccessApi.Integration;

namespace TweetsStreamingAPI
{
    public class Startup
    {
        /// <summary>Initializes a new instance of the <see cref="Startup" /> class.</summary>
        /// <param name="env">The env.</param>
        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                    .SetBasePath(env.ContentRootPath)
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables();

            this.Configuration = builder.Build();
        }

        /// <summary>Gets the service provider.</summary>
        /// <value>The service provider.</value>
        public IServiceProvider ServiceProvider { get; set; }

        /// <summary>Gets or sets the service preprocessor.</summary>
        /// <value>The service preprocessor.</value>
        public Action<IServiceCollection> ServiceCollection { get; set; }

        /// <summary>Gets the configuration.</summary>
        /// <value>The configuration.</value>
        public IConfiguration Configuration { get; }

        /// <summary>Configures the services.</summary>
        /// <param name="services">The services.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            // ----------------------
            // Setting up MVC Options
            // ----------------------
            services
                 .AddControllers()
                 .AddJsonOptions((o) =>
                 {
                     o.JsonSerializerOptions.AllowTrailingCommas = true;
                     o.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.Never;
                     o.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                     o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(namingPolicy: JsonNamingPolicy.CamelCase, allowIntegerValues: true));
                     o.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                 });

            services
                .AddLogging()

                // Disbaling modlestate error filter
                // to get full control of error handling
                .Configure<ApiBehaviorOptions>(options =>
                {
                    options.SuppressModelStateInvalidFilter = true;
                })

                // Used so we don't have to access the HttpContext directly
                // * Helps with unit tests and mocking
                .AddHttpContextAccessor()

            // Register the Swagger generator, defining 1 or more Swagger documents
            .AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Tweets streaming API",
                    Description = "",
                    Contact = new OpenApiContact
                    {
                        Name = "Jasvir Singh",
                        Email = "jasvir.nar@gmail.com"
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under OpenApiLicense",
                        Url = new Uri("https://example.com/license"),
                    }
                });
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
                c.OperationFilter<AuthorizationHeaderOperationFilter>();
            });

            // setup.EnableAnnotations();

            //setup.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
            //    setup.OperationFilter<AuthorizationHeaderOperationFilter>();
            //});

            services.AddHttpClient();
            services.AddPollyConfiguration();
            services.AddMemoryCache();
            //services.AddHttpClient<UnreliableEndpointCallerService>()
            // .AddTransientHttpErrorPolicy(p =>
            //   p.WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(600)));


            services.Configure<ApiConfigModel>(Configuration.GetSection("ApiConfig"));

            services.AddTransient<TweetsController>();

            // Services
            services.AddTransient<ITweetsStreamingService, TweetsStreamingService>();
            services.AddTransient<ITwitterIntegration, TwitterIntegration>();
            services.AddTransient<IRestApiClient, RestApiClient>();


            // A pre-processor hook is provided to allow for unit tests to 
            // pre configure/replace services prior to the service provider being built
            if (ServiceCollection != null)
            {
                ServiceCollection(services);
            }
        }

        /// <summary>Configures the specified application.</summary>
        /// <param name="app">The application.</param>
        /// <param name="env">The env.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsEnvironment("Local"))
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSerilogRequestLogging();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSwagger();
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseMiddleware<RequestResponseLoggingMiddleware>();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My Test1 Api v1");
            });

            if (!env.IsDevelopment())
            {
                app.UseHttpsRedirection();
            }
            else if (env.IsDevelopment())
            {
                app.UseCors(options => options
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            }

            app.UseEndpoints(c =>
            {
                c.MapControllers();

                c.MapGet(string.Empty, context =>
                {
                    context.Response.Redirect($"swagger/index.html");
                    return Task.FromResult(0);
                });
                c.MapGet("{operation:regex(^index.html?$)}", context =>
                {
                    context.Response.Redirect($"swagger/index.html");
                    return Task.FromResult(0);
                });

                // Registering a local copy of the IServiceProvider here so it can be 
                // used in the post configure methods of the various configuration sections
                this.ServiceProvider = app.ApplicationServices;
            });
        }
    }
}