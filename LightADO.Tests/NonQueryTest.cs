namespace LightADO.Tests
{
    using Xunit;

    public class NonQueryTest
    {
        [Fact]
        public void ExecuteNonQueryToInsertWithStoredProcedure()
        {
            Application application = new Application()
            {
                Description = "This a test",
                IPx = "192.168.1.1",
                Name = "An application to test non query method.",
                Platform = Application.Platforms.DotNet,
                OriginUrl = "https://www.lightado.net",
                Logo = "DefaultApplicationLogo.png"
            };

            Assert.True(new NonQuery().Execute(
                "Applications_Create",
                application) == true);
        }

        [Fact]
        public void ExecuteNonQueryToInsertWithStoredProcedureAndGetOutputId()
        {
            Application application = new Application()
            {
                Description = "This a test",
                IPx = "192.168.1.1",
                Name = "An application to test non query method.",
                Platform = Application.Platforms.DotNet,
                OriginUrl = "https://www.lightado.net",
                Logo = "DefaultApplicationLogo.png"
            };

            new NonQuery().Execute("Applications_Create", application);
            Assert.True(application.ID != 0);
        }

        [Fact]
        public void ExecuteNonQueryToInsertWithStoredProcedureAndForeignKey()
        {
            Token token = new Token();
            token.Key = System.Guid.NewGuid().ToString();
            token.Application = new Application()
            {
                ID = 2
            };

            new NonQuery().Execute("Tokens_Create", token);
            Assert.True(token.ID != 0);
        }

        [Fact]
        public void ExecuteNonQueryToInsertWithStoredProcedureAndGetOutputIdInhertied()
        {
            ApplicationC application = new ApplicationC()
            {
                Description = "This a test",
                IPx = "192.168.1.1",
                Name = "An application to test non query method.",
                Platform = Application.Platforms.DotNet,
                OriginUrl = "https://www.lightado.net",
                Logo = "DefaultApplicationLogo.png"
            };

            new NonQuery().Execute("Applications_Create", application);
            Assert.True(application.ID != 0);
        }
    }
}
