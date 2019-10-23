namespace LightADO.Tests
{
    using System;
    using Xunit;

    public class QueryTest
    {
        [Fact]
        public void ExecuteQueryAsTextAndGetObject()
        {
            Assert.True(new Query().ExecuteToObject<Application>(
                "select * from Applications where ID = 1",
                System.Data.CommandType.Text) != null);
        }

        [Fact]
        public void ExecuteQueryAsTextAndMapToObject()
        {
            Application application = new Application();
            new Query().ExecuteToObject<Application>("select * from Applications where ID = 1", application, System.Data.CommandType.Text);
            Assert.True(application.ID != 0);
        }

        [Fact]
        public void ExecuteQueryAsStoredProcedureAndGetDataTabel()
        {
            Assert.True(new Query().ExecuteToDataTable(
                 "Get_Applications_ByID",
                 System.Data.CommandType.StoredProcedure,
                 new Parameter("ID", 1)) != null);
        }

        [Fact]
        public void ExecuteQueryAsTextAndGetDataTable()
        {
            Assert.True(new Query().ExecuteToDataTable("select * from Applications where ID = 1", System.Data.CommandType.Text) != null);
        }

        [Fact]
        public void ExecuteQueryAsStoredProcedureAndGetDataSet()
        {
            Assert.True(new Query().ExecuteToDataSet(
                 "Get_Applications_ByID",
                 System.Data.CommandType.StoredProcedure,
                 new Parameter("ID", 1)) != null);
        }

        [Fact]
        public void ExecuteQueryAsTextAndGetDataSet()
        {
            Assert.True(new Query().ExecuteToDataSet("select * from Applications where ID = 1", System.Data.CommandType.Text) != null);
        }

        [Fact]
        public void ExecuteQueryAsTextAndGetListOfObject()
        {
            Assert.True(new Query().ExecuteToListOfObject<Application>(
                "select * from Applications",
                System.Data.CommandType.Text) != null);
        }

        [Fact]
        public void ExecuteQueryAsStoredProcedureAndGetObject()
        {
            Assert.True(new Query().ExecuteToObject<Application>(
                "Get_Applications_ByID",
                System.Data.CommandType.StoredProcedure,
                new Parameter("ID", 1)) != null);
        }

        [Fact]
        public void TestLightAdoColumnName()
        {
            ApplicationC application = new Query().ExecuteToObject<ApplicationC>(
                "Get_Applications_ByID",
                System.Data.CommandType.StoredProcedure,
                new Parameter("ID", 1));

            Assert.True(string.IsNullOrEmpty(application.FirstName) == false);
        }

        [Fact]
        public void TestLightAdoDefaultValue()
        {
            ApplicationC application = new Query().ExecuteToObject<ApplicationC>(
                "Get_Applications_ByID",
                System.Data.CommandType.StoredProcedure,
                new Parameter("ID", 1));

            Assert.True(application.Address == "This a test for the default values");
        }

        [Fact]
        public void TestLightAdoDefaultValueWithOnlyNonQuery()
        {
            ApplicationC application = new Query().ExecuteToObject<ApplicationC>(
                "Get_Applications_ByID",
                System.Data.CommandType.StoredProcedure,
                new Parameter("ID", 1));

            Assert.True(string.IsNullOrEmpty(application.Address2) == true);
        }

        [Fact]
        public void TestLightAdoDefaultValueWithOnlyQuery()
        {
            ApplicationC application = new Query().ExecuteToObject<ApplicationC>(
                "Get_Applications_ByID",
                System.Data.CommandType.StoredProcedure,
                new Parameter("ID", 1));

            Assert.True(string.IsNullOrEmpty(application.Address3) == false);
        }

        [Fact]
        public void ExecuteQueryAsStoredProcedureAndGetListOfObject()
        {
            Assert.True(new Query().ExecuteToListOfObject<Application>(
                "Get_Applications",
                System.Data.CommandType.StoredProcedure) != null);
        }

        [Fact]
        public void ExecuteNotfoundStoredProcedure()
        {
            try
            {
                new Query().ExecuteToListOfObject<Application>(
                "Get_ApplicationsX",
                System.Data.CommandType.StoredProcedure);
            }
            catch (Exception)
            {
                Assert.True(true);
            }
        }

        [Fact]
        public void ExecuteQueryAsStoredProcedureAndGetJson()
        {
            Assert.True(new Query().ExecuteToObject<Application>(
                "Get_Applications_ByID",
                System.Data.CommandType.StoredProcedure,
                FormatType.Json,
                new Parameter("ID", 1)) != null);
        }

        [Fact]
        public void ExecuteQueryAsStoredProcedureAndGetListOfJson()
        {
            Assert.True(new Query().ExecuteToListOfObject<Application>(
                "Get_Applications",
                System.Data.CommandType.StoredProcedure,
                FormatType.Json) != null);
        }

        [Fact]
        public void ExecuteQueryAsStoredProcedureAndGetXml()
        {
            Assert.True(new Query().ExecuteToObject<Application>(
                "Get_Applications_ByID",
                System.Data.CommandType.StoredProcedure,
                FormatType.XML,
                new Parameter("ID", 1)) != null);
        }

        [Fact]
        public void ExecuteQueryAsStoredProcedureAndGetListOfXml()
        {
            Assert.True(new Query().ExecuteToListOfObject<Application>(
                "Get_Applications",
                System.Data.CommandType.StoredProcedure,
                FormatType.XML) != null);
        }
    }
}