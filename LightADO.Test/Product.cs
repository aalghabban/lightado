namespace LightADO.Test
{
    public class Product
    {
        [ColumnName("ProductID")]
        public int ID { get; set; }

        public string ProductName { get; set; }

        [ColumnName("SupplierID")]
        public int Supplier { get; set; }

        public string QuantityPerUnit { get; set; }

        public double UnitPrice { get; set; }

        public int UnitsInStock { get; set; }

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