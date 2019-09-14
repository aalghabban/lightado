using System.Data;

namespace LightADO
{
  public sealed class Parameter
  {
    public string Name { get; set; }

    public object Value { get; set; }

    public ParameterDirection Direction { get; set; }

    public Parameter(string name, object value, ParameterDirection direction = ParameterDirection.Input)
    {
      this.Name = "@" + name;
      this.Value = value;
      this.Direction = direction;
    }
  }
}
