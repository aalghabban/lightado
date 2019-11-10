namespace LightADO.Tests
{
 
    public class ApplicationC : Application
    {
        [ColumnName("Name")]
        public string FirstName { get; set; }

        [DefaultValue("This a test for the default values", Types.ValueTypes.Value, Types.Directions.WithBoth)]
        public string Address { get; set; }

        [DefaultValue("This a test for the default values", Types.ValueTypes.Value, Types.Directions.WithNonQuery)]
        public string Address2 { get; set; }

        [DefaultValue("This a test for the default values", Types.ValueTypes.Value, Types.Directions.WithQuery)]
        public string Address3 { get; set; }
    }
}
