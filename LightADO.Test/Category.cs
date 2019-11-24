namespace LightADO.Test
{
    using System.Data;
    using System;
    using LightADO;

    public class Category
    {

        public Category()
        {
        }

        public Category(int id)
        {
            new Query().ExecuteToObject("CategoriesGetByID", this);
        }

        public Category Create(Category category)
        {
            if (this.IsExist(category.ID) == false)
            {
                new NonQuery().Execute("Categories_Create", category);
            }

            return category;
        }

        public bool IsExist(int id)
        {
            if (this.GetByID(id) == null)
            {
                return false;
            }

            return true;
        }

        public Category GetByID(int id)
        {
            return new Query().ExecuteToObject<Category>("CategoriesGetByID", CommandType.StoredProcedure, new Parameter("CategoryID", id));
        }

        [PrimaryKey]
        [ColumnName("CategoryID")]
        public int ID { get; set; }

        [ColumnName("CategoryName")]
        [DefaultValue("Guid.NewGuid", Types.Directions.WithNonQuery)]
        public string Name { get; set; }

        [DefaultValue("Guid.NewGuid", Types.Directions.WithNonQuery)]
        public string Description { get; set; }

        [DefaultValue("DateTime.Now", Types.Directions.WithBoth)]
        public DateTime CreateDate { get; set; }
    }
}