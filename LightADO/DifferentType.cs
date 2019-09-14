namespace LightADO
{
    using System;

    public class DifferentType : Attribute
    {
        public Type TypeOf { get; set; }

        public DifferentType()
        {
        }

        public DifferentType(Type typeOf)
        {
            this.TypeOf = typeOf;
        }
    }
}
