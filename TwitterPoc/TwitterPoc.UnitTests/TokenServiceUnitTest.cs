using Microsoft.VisualStudio.TestTools.UnitTesting;
using TwitterPoc.Authorization;
using TwitterPoc.Logic;
using TwitterPoc.UnitTests.Mocks;

namespace TwitterPoc.UnitTests
{
    [TestClass]
    public class TokenServiceUnitTest
    {
        [TestMethod]
        public void BuildTokenAndTokenValidationTest()
        {
            const string key = "adi123";
            const string issuer = "issuer";

            ITokenService service = new TokenService(new LoggerMock());
            var token = service.BuildToken(key, issuer, new Logic.Entities.User() { Username = "user123" });
            Assert.IsTrue(service.IsTokenValid(key, issuer, token));
        }

    }
}
