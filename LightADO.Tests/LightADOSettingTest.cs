namespace LightADO.Tests
{
    using System;
    using Xunit;

    public class LightADOSettingTest
    {
        [Fact]
        public void LoadConnectionfromAppSettingsFile()
        {
            Assert.True(new LightADOSetting() != null);
        }

        [Fact]
        public void LoadCustomConnectionfromAppSettingsFileWhereKeyNotListed()
        {
            try
            {
                new LightADOSetting("TryThisConnection");
            }
            catch (Exception)
            {
                Assert.True(true);
            }
        }

        [Fact]
        public void LoadCustomConnectionfromAppSettingsFileWhereKeyIsListed()
        {
            Assert.True(new LightADOSetting("SubConnection") != null);
        }
    }
}
