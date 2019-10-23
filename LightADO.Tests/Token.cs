namespace LightADO.Tests
{
    [Encrypto]
    public class Token
    {
        public int ID { get; set; }

        public string Key { get; set; }

        [ForeignKey]
        public Application Application { get; set; }
    }
}
