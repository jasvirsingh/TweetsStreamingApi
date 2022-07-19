using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TweetsSteamingApi.Security;

namespace TweetsAccessApi.IntegrationTests.Helpers
{
    /// <summary>
    ///     Mock Service Extensions
    /// </summary>
    public static class MockServiceExensions
    {
        /// <summary>
        ///     Adds Mock Authrntication to the Services collection
        /// </summary>
        /// <param name="services">
        ///     <see cref="IServiceCollection"/>
        /// </param>
        /// <param name="claimsPrincipal">
        ///     <see cref="ClaimsPrincipal"/>
        /// </param>
        public static void AddMockedAuthentication(
            this IServiceCollection services, ClaimsPrincipal claimsPrincipal)
        {
            //Make sure TokenValidation passes
            var tokenValidatorMock = new Mock<ITokenValidator>();

            tokenValidatorMock.Setup(method =>
                method.IsValidToken(It.IsAny<string>()))
                    .ReturnsAsync(true);

            tokenValidatorMock.Setup(method =>
                method.RetrieveClaimsPrincipalFromToken(It.IsAny<string>()))
                    .ReturnsAsync(claimsPrincipal);

            ////Make sure TokenRules pass
            //var identityRulesValidatorMock = new Mock<ITokenRule>();

            //identityRulesValidatorMock.Setup(method =>
            //    method.ExecuteRules(It.IsAny<ClaimsPrincipal>()))
            //        .Returns(Task.CompletedTask);

            ////Make sure RealmLookup returns a valid value
            //var oidcRealmRepository = new Mock<IOidcRealmRepository>();

            //oidcRealmRepository.Setup(method =>
            //    method.RetrieveWhiteListedRealms())
            //        .ReturnsAsync(new Dictionary<string, OidcRealmModel>());

            //oidcRealmRepository.Setup(method =>
            //    method.RequestRefresh())
            //        .Returns(Task.CompletedTask);

            //oidcRealmRepository.Setup(method =>
            //    method.RetrieveOidcRealm(It.IsAny<string>()))
            //        .ReturnsAsync(new OidcRealmModel());

            //oidcRealmRepository.Setup(method =>
            //    method.RetrieveOidcConfiguration(It.IsAny<OidcRealmModel>()))
            //        .ReturnsAsync(new OpenIdConnectConfiguration());

            ////Pass in mocked services
            //services.AddSingleton(oidcRealmRepository.Object);
            services.AddSingleton(tokenValidatorMock.Object);
           // services.AddSingleton(identityRulesValidatorMock.Object);
        }

        /// <summary>
        ///     Adds mock business logic calls
        /// </summary>
        /// <param name="services">
        ///     <see cref="IServiceCollection"/>
        /// </param>
        public static void AddMockedBusinessLogic(this IServiceCollection services)
        {
        }
    }
}
