// Decompiled with JetBrains decompiler
// Type: LightADO.Parameter
// Assembly: LightADO, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 51CA6897-B553-4842-97D6-3F8C9C17C880
// Assembly location: C:\Users\ALGHABBAN\source\repos\ClassLibrary1\packages\LightAdo.net.4.6.0\lib\LightADO.dll

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
