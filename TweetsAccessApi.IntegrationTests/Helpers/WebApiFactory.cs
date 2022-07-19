using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics.CodeAnalysis;

namespace TweetsAccessApi.IntegrationTests.Helpers
{
    /// <summary>
    ///     WEB API Factory used to host the API for unit testing
    /// </summary>
    /// <typeparam name="TProgram">
    ///     The Startup class of the API
    /// </typeparam>
    [ExcludeFromCodeCoverage]
    public class WebApiFactory<TProgram> :  WebApplicationFactory<TProgram> where TProgram : class
    {
        private readonly Action<IServiceCollection> _testServicecollection;

        /// <summary>
        ///     Constructor for the <see cref="WebApiFactory{TStartup}"/>.
        /// </summary>
        /// <param name="testServicecollection">
        ///     <see cref="IServiceCollection"/>.
        /// </param>
        public WebApiFactory(Action<IServiceCollection> testServicecollection)
        {
            _testServicecollection = testServicecollection;
        }

        /// <summary>
        ///     Overrides the <see cref="IWebHostBuilder"/>, use our own testing configurations
        /// </summary>
        /// <returns>
        ///     <see cref="IWebHostBuilder"/>
        /// </returns>
        protected override IWebHostBuilder CreateWebHostBuilder()
        {
            return WebHost.CreateDefaultBuilder().UseStartup<TProgram>();
        }

        /// <summary>
        ///     Configures the web host
        /// </summary>
        /// <param name="builder">
        ///     <see cref="IWebHostBuilder"/>
        /// </param>
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            //The builder.UseContentRoot(".") also tells the factory to
            //use the current projects build/runing path as the root.
            //This is used to read our custom appsettings.json file.
            builder.UseContentRoot(".");

            base.ConfigureWebHost(builder);

            //Configure the services as well as the additional services potentially passed in via the constructor.
            builder.ConfigureTestServices(collection => { _testServicecollection?.Invoke(collection); });
        }
    }
}
