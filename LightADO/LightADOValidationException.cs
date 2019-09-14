// Decompiled with JetBrains decompiler
// Type: LightADO.LightADOValidationException
// Assembly: LightADO, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 51CA6897-B553-4842-97D6-3F8C9C17C880
// Assembly location: C:\Users\ALGHABBAN\source\repos\ClassLibrary1\packages\LightAdo.net.4.6.0\lib\LightADO.dll

using System;

namespace LightADO
{
  public sealed class LightADOValidationException : Exception
  {
    public new string Message { get; internal set; }

    public string Code { get; internal set; }

    public new string Source { get; set; }

    internal LightADOValidationException(string message)
    {
      this.Message = message;
    }
  }
}
