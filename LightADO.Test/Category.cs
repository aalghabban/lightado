namespace LightADO.Test
{
    using LightADO;
    
    public class Category
    {
        [ColumnName("CategoryID")]
        public int ID { get; set; }

        [ColumnName("CategoryName")]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}