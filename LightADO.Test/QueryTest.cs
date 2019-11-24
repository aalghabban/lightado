using System.Data;
namespace LightADO.Test
{
    using System;
    using Xunit;
    using LightADO;
    using System.Collections.Generic;

    public class QueryTest
    {
        [Fact]
        public void ExecuteTextQueryAndShouldReturnListOfCategories()
        {
            try
            {
                List<Category> categories = new Query().ExecuteToListOfObject<Category>("select * from categories", CommandType.Text);
                Assert.True(categories.Count > 0);
            }
            catch (LightAdoExcption ex)
            {
                Console.WriteLine(ex.Message);
                Assert.True(false);
            }
        }

        [Fact]
        public void ExecuteTextQueryAndShouldReturnSingleCateogry()
        {
            try
            {
                Category category = new Query().ExecuteToObject<Category>("select * from categories where CategoryID = 1", CommandType.Text);
                Assert.True(category.ID != 0);
            }
            catch (LightAdoExcption ex)
            {
                Console.WriteLine(ex.Message);
                Assert.True(false);
            }
        }

        [Fact]
        public void ExecuteTextQueryWithParametersShouldReturnSingleCategory()
        {
            try
            {
                Category category = new Query().ExecuteToObject<Category>("select * from categories where CategoryID = @ID", CommandType.Text, new Parameter("ID", 1));
                Assert.True(category.ID != 0);
            }
            catch (LightAdoExcption ex)
            {
                Console.WriteLine(ex.Message);
                Assert.True(false);
            }
        }

        [Fact]
        public void ExecuteStoredProcedureAndShouldReturnListOfCategorie()
        {
            try
            {
                List<Category> categories = new Query().ExecuteToListOfObject<Category>("GetCategories", CommandType.StoredProcedure);
                Assert.True(categories.Count > 0);
            }
            catch (LightAdoExcption ex)
            {
                Console.WriteLine(ex.Message);
                Assert.True(false);
            }
        }

        [Fact]
        public void ExecuteStoredProcedureWithParametersShouldReturnSingleCategory()
        {
            try
            {
                Category category = new Query().ExecuteToObject<Category>("GetCategoriesByID", CommandType.StoredProcedure, new Parameter("CategoryID", 1));
                Assert.True(category.ID != 0);
            }
            catch (LightAdoExcption ex)
            {
                Console.WriteLine(ex.Message);
                Assert.True(false);
            }
        }

        [Fact]
        public void CategoryDateTimeShouldBeValid()
        {
            try
            {
                Category category = new Query().ExecuteToObject<Category>("GetCategoriesByID", CommandType.StoredProcedure, new Parameter("CategoryID", 1));
                Assert.True(category.CreateDate.Year == DateTime.Now.Year);
            }
            catch (LightAdoExcption ex)
            {
                Console.WriteLine(ex.Message);
                Assert.True(false);
            }
        }

        [Fact]
        public void CategoryDescriptionShouldBeIgnoredDuringMapping()
        {
            try
            {
                Category category = new Query().ExecuteToObject<Category>("GetCategoriesByID", CommandType.StoredProcedure, new Parameter("CategoryID", 1));
                Assert.True(category.Description == null );
            }
            catch (LightAdoExcption ex)
            {
                Console.WriteLine(ex.Message);
                Assert.True(false);
            }
        }
    }
}