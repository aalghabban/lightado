using System.Collections.Generic;
using System.Data;

namespace LightADO
{
    public class Transaction
    {
        public object Data { get; set; }

        public string Command { get; set; }

        public CommandType CommandType { get; set; }

        public Parameter[] Parameters { get; set; }
    }
}
