// Decompiled with JetBrains decompiler
// Type: LightADO.StoredProcedureParameter
// Assembly: LightADO, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 51CA6897-B553-4842-97D6-3F8C9C17C880
// Assembly location: C:\Users\ALGHABBAN\source\repos\ClassLibrary1\packages\LightAdo.net.4.6.0\lib\LightADO.dll

using System.Collections.Generic;
using System.Data;

namespace LightADO
{
  internal class StoredProcedureParameter
  {
    private string storedProcedureName;

    public string Name { get; set; }

    public string Mode { get; set; }

    public string TypeName { get; set; }

    internal ParameterDirection GetParameterDirection
    {
      get
      {
        return !(this.Mode == "INOUT") ? ParameterDirection.Input : ParameterDirection.Output;
      }
    }

    internal LightADOSetting LightAdoSetting { get; set; }

    internal List<StoredProcedureParameter> Parameters
    {
      get
      {
        return new Query(this.LightAdoSetting.ConnectionString, false).ExecuteToListOfObject<StoredProcedureParameter>("select PARAMETER_NAME as Name, PARAMETER_MODE as Mode, Data_Type as TypeName from information_schema.parameters where specific_name= @StoredProcedureName", CommandType.Text, new Parameter("StoredProcedureName", (object) this.storedProcedureName, ParameterDirection.Input));
      }
    }

    public StoredProcedureParameter()
    {
    }

    internal StoredProcedureParameter(string storedProcedureName, LightADOSetting setting)
    {
      this.storedProcedureName = storedProcedureName;
      this.LightAdoSetting = setting;
    }
  }
}
