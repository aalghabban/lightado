/*
 * Copyright (C) 2019 ALGHABBAn
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Affero General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Affero General Public License for more details.
 *
 * You should have received a copy of the GNU Affero General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

namespace LightADO
{
    using System.Collections.Generic;
    using System.Data;

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
