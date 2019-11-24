using System.Data;
using System.Collections.Generic;
using Xunit;
using System;

namespace LightADO.Test
{
    public class NonQueryTest
    {
        [Fact]
        public void ShoulThrowMaxValidationException()
        {
            try
            {
                new NonQuery().Execute<Product>("ProductCreate", new Product()
                {
                    UnitsInStock = 49, 
                    ProductName = "test"
                });
            }
            catch (ValidationException)
            {
                Assert.True(true);
            }
            catch (LightAdoExcption ex)
            {
                Console.WriteLine(ex.Details.Message);
                Assert.True(false);
            }
            catch (Exception ex)
            {
                Assert.True(false);
                Console.WriteLine(ex.Message);
            }
        }

        [Fact]
        public void SubObjectIdShouldBeBiggerThanZero()
        {
            try{
                Product product = new Product()
            {
                ProductName = "test",
                Supplier = 1,
                Category = new Category()
                {
                    Name = "this a test for workflow"
                },
                QuantityPerUnit = "36 boxes",
                UnitPrice = 98.0922,
                UnitsInStock = 49,
                UnitsOnOrder = 11,
                ReorderLevel = 10,
                Discontinued = false
            };

            product.Create();
            Assert.True(product.ID > 0 && product.Category.ID > 0);
            }
            catch(LightAdoExcption ex){
                
            }
            catch(Exception ex){
                Console.WriteLine(ex.Message);
            }
        }

        [Fact]
        public void BulkExecuteTest()
        {
            List<Category> categories = new List<Category>();
            categories.Add(new Category
            {
                Name = "Test",
                Description = "this test number 1"
            });

            categories.Add(new Category
            {
                Name = "Test",
                Description = "this test number 2"
            });

            categories.Add(new Category
            {
                Name = "Test",
                Description = "this test number 3"
            });

            categories.Add(new Category
            {
                Name = "Test",
                Description = "this test number 4"
            });

            categories.Add(new Category
            {
                Name = "Test",
                Description = "this test number 5"
            });

            Assert.True(new NonQuery().Execute("Categories_Create", categories) == true);
        }
    }
}