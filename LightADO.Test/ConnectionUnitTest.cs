namespace LightADO.Test
{
    using System;
    using Xunit;
    using LightADO;

    public class ConnectionUnitTest
    {
        [Fact]
        public void ShouldThrowExceptionWhenConnectionStringNotFoundInAppSettings()
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
        public void ShouldThrowExceptionWhenConnectionStringIsEmpty()
        {
            try
            {
                new Query("EmptyConnection");
                Assert.True(false);
            }
            catch (LightAdoExcption ex)
            {
                Console.WriteLine(ex);
                Assert.True(true);
            }
        }

        [Fact]
        public void ShouldLoadConnectionFromSystemEnv()
        {
            try
            {
                System.Environment.SetEnvironmentVariable("DefaultConnection", "{ConnectionString}");
                new Query();
                Assert.True(true);
            }
            catch (LightAdoExcption ex)
            {
                Console.WriteLine(ex);
                Assert.True(false);
            }
        }

        [Fact]
        public void ShouldLoadConnectionStringFromAppSettings()
        {
            try
            {
                new Query();
                Assert.True(true);
            }
            catch (LightAdoExcption ex)
            {
                Console.WriteLine(ex);
                Assert.True(false);
            }
        }

        [Fact]
        public void ShouldLoadDirectConnectionStringDirect()
        {
            try
            {
                new Query("Server=.;Database=northwind;User Id=sa;Password=1986Gabban2017*m");
                Assert.True(true);
            }
            catch (LightAdoExcption ex)
            {
                Console.WriteLine(ex);
                Assert.True(false);
            }
        }

        [Fact]
        public void ShouldThrowExceptionWhenConnectionStringNotValid()
        {
            try
            {
                new Query("9687469087436456894");
                Assert.True(false);
            }
            catch (LightAdoExcption ex)
            {
                Console.WriteLine(ex);
                Assert.True(true);
            }
        }
    }
}