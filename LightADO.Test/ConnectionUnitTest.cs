namespace LightADO.Test
{
    using System;
    using Xunit;
    using LightADO;
    using System.Collections.Generic;

    public class ConnectionUnitTest
    {
        [Fact]
        public void ConnectionNotFoundInAppSettingsOrSystemEnv()
        {
            try
            {
                new Query("NoConnection");
                Assert.True(false);
            }
            catch (LightAdoExcption ex)
            {
                Console.WriteLine(ex);
                Assert.True(true);
            }
        }

        [Fact]
        public void LoadConnectionFromSystemEnv()
        {
            try
            {
                System.Environment.SetEnvironmentVariable("DefaultConnection", "{ConnectionString}");
                List<Category> categories =  new Query().ExecuteToListOfObject<Category>("Select * from Categories", System.Data.CommandType.Text);
                Assert.True(categories.Count > 0);
            }
            catch (LightAdoExcption ex)
            {
                Console.WriteLine(ex);
                Assert.True(false);
            }
        }
    }
}