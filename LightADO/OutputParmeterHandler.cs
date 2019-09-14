// Decompiled with JetBrains decompiler
// Type: LightADO.OutputParmeterHandler
// Assembly: LightADO, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 51CA6897-B553-4842-97D6-3F8C9C17C880
// Assembly location: C:\Users\ALGHABBAN\source\repos\ClassLibrary1\packages\LightAdo.net.4.6.0\lib\LightADO.dll

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Reflection;

namespace LightADO
{
  internal class OutputParmeterHandler
  {
    internal static List<Parameter> GetOutputParamters(SqlCommand sqlCommand)
    {
      List<Parameter> parameterList = new List<Parameter>();
      foreach (SqlParameter parameter in (DbParameterCollection) sqlCommand.Parameters)
      {
        if (parameter.Direction == ParameterDirection.Output)
          parameterList.Add(new Parameter(parameter.ParameterName, parameter.Value, ParameterDirection.Input));
      }
      return parameterList;
    }

    internal static void SetOutputParameter<T>(SqlCommand sqlCommand, T objectToMap)
    {
      List<Parameter> outputParamters = OutputParmeterHandler.GetOutputParamters(sqlCommand);
      if (outputParamters == null || outputParamters.Count <= 0)
        return;
      foreach (Parameter parameter in outputParamters)
        objectToMap.GetType().GetProperty(parameter.Name.Remove(0, 2)).SetValue((object) objectToMap, parameter.Value);
    }

    internal static void SetOutputParameter<T>(
      SqlCommand sqlCommand,
      T objectToMap,
      params Parameter[] parameters)
    {
      List<Parameter> outputParamters = OutputParmeterHandler.GetOutputParamters(sqlCommand);
      if (outputParamters == null || outputParamters.Count <= 0)
        return;
      foreach (Parameter parameter1 in outputParamters)
      {
        Parameter parameter = parameter1;
        if (objectToMap.GetType().GetProperty(parameter.Name.Remove(0, 2)) != (PropertyInfo) null)
          objectToMap.GetType().GetProperty(parameter.Name.Remove(0, 2)).SetValue((object) objectToMap, parameter.Value);
        else if (Array.Find<Parameter>(parameters, (Predicate<Parameter>) (x => parameter.Name.Remove(0, 1) == x.Name)) != null)
          Array.Find<Parameter>(parameters, (Predicate<Parameter>) (x => parameter.Name.Remove(0, 1) == x.Name)).Value = parameter.Value;
      }
    }
  }
}
