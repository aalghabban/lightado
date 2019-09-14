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
