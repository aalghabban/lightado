namespace LightADO.Test
{
    using System;
    using LightADO;

    public class Category
    {
        [ColumnName("CategoryID")]
        public int ID { get; set; }

        [ColumnName("CategoryName")]
        public string Name { get; set; }

        [DefaultValue("Guid.NewGuid", Types.Directions.WithNonQuery)]
        public string Description { get; set; }

        [DefaultValue("DateTime.Now", Types.Directions.WithBoth)]
        public DateTime CreateDate { get; set; }

        
        public string Guid { get; set; }
    }
}