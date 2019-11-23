namespace LightADO.Test
{
    public class Product
    {
        [ColumnName("ProductID")]
        public int ID { get; set; }

        [NotNullOrEmpty(false, "Product Name must not be empty or null or white space")]
        public string ProductName { get; set; }

        [ColumnName("SupplierID")]
        public int Supplier { get; set; }

        public string QuantityPerUnit { get; set; }

        public double UnitPrice { get; set; }

        [Max(50, "Units In Stock must be less than 50")]
        public int UnitsInStock { get; set; }

        [Min(10, "Unit in stock must be bigger than 10")]
        public int UnitsOnOrder { get; set; }

        public int ReorderLevel { get; set; }

        public bool Discontinued { get; set; }

        [CreateOnNotExists("Create")]
        [ColumnName("CategoryID")]
        [ForeignKey]
        public Category Category { get; set; }

        public bool Create(){
            return new NonQuery().Execute("Products_Create", this);
        }
    }
}