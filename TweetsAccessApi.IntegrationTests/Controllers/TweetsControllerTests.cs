using TweetsAccessApi.IntegrationTests;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using System.Net;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;
using TweetsAccessApi.IntegrationTests.Helpers;
using System.Security.Claims;

namespace TweetsAccessApi.UnitTests.Controllers
{
    public class TweetsControllerTests : ServiceTestBase
    {
        public class TestUtilityController
        {
            private const string TEST_JWT = "eyJhbGciOiJSUzI1NiIsImtpZCI6InN4OEFudTd0MzY0MDZYZGlibHFRSmdEUEVZdyIsIng1dCI6InN4OEFudTd0MzY0MDZYZGlibHFRSmdEUEVZdyIsInR5cCI6IkpXVCJ9.eyJhdWQiOiJkZXYuZm1nbG9iYWwiLCJpc3MiOiJkZXYuY29ycC5mbWdsb2JhbC5jb20iLCJpZHAiOiJkZXYuY29ycC5mbWdsb2JhbC5jb20iLCJpYXQiOjE1OTUwMzY0MjQsIm5iZiI6MTU5NTAzNjQyNCwiZXhwIjoxNTk1MDM2NzI0LCJjdHgiOiJ1c2VyIiwiZW1haWwiOiJwYWRtaW5pLnJhb0BmbWdsb2JhbC5jb20iLCJzdWIiOiJyYW9wIn0.b3UnQ_qOMUQaXTQei73lhQvhqmChaZ4VHzbdomYXjws4w5NsOvkCAu7MMYgTlw1SWX58vlWL1K38GNzVsFaOYBG3imu0TNyQTaRlhe8RfpAPTonMIqdiHg_LLOldGEBSCZqFkySrJt5E1hp_jKp9AzihfokGWVreRQD6kUa-J1Op8LZnyWD94cs0ECkD_KrGOvtvGxFhlx5dQkV4CKPah_xcOAq51JKC6kbK_4tDjF_rr7uxt6oChMuA7gU8B1_q0HtNccN6Xu0-0nswiJleE906kZh0cQXlLtLiBmYyXkcMWQEys0HvcDAMVJq1XOG3J7mkRWNjNPzDS744RC_PRg";

            [Fact]
            public async Task Ping_WebFactory_200()
            {
                var webFactory = GetWebApiFactory();
                var client = webFactory.CreateClient();

                var response = await client.GetAsync("tweets/ping");

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }

            [Fact]
            public async Task Health_WebFactory_200()
            {
                var webFactory = GetWebApiFactory();
                var client = webFactory.CreateClient();

                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {TEST_JWT}");

                var response = await client.GetAsync("tweets/health");

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }

            private static WebApiFactory<Program> GetWebApiFactory()
            {
                Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "UnitTesting");

                return new WebApiFactory<Program>(services =>
                {
                    // services.AddMockedBusinessLogic();

                    var claimsPrincipal = new ClaimsPrincipal(
                        new ClaimsIdentity(
                            new List<Claim>()
                            {
                            new Claim("email", "jasvir.singh@gmail.com")
                            }, "test"));

                    services.AddMockedAuthentication(claimsPrincipal);
                });
            }
        }
    }
}
