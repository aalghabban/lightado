using System.Data;
using System.Collections.Generic;
using Xunit;
using System;

namespace LightADO.Test
{
    public class NonQueryTest
    {
        [Fact]
        public void CreateListOfCategoriesThisShouldUpdateTheCategoryId(){
            List<Category> categories = new List<Category>();
            categories.Add(new Category{
                Name = "Test",
                Description = "this test number 1"
            });

            categories.Add(new Category{
                Name = "Test",
                Description = "this test number 2"
            });

            categories.Add(new Category{
                Name = "Test",
                Description = "this test number 3"
            });

            categories.Add(new Category{
                Name = "Test",
                Description = "this test number 4"
            });

            categories.Add(new Category{
                Name = "Test",
                Description = "this test number 5"
            });

            Assert.True(new NonQuery().Execute("Categories_Create", categories) == true);
        }
    }
}