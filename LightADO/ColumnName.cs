namespace LightADO
{
    using System;

    public class ColumnName : Attribute
    {
        public string Name { get; set; }

        public ColumnName()
        {
        }

        public ColumnName(string name)
        {
            this.Name = name;
        }
    }
}
