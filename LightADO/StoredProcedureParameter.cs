/*
 * a.alghabban@icloud.com
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

    /// <summary>
    /// Provide a way to handle the stored procedure parameters.
    /// </summary>
    internal class StoredProcedureParameter
    {
        /// <summary>
        /// stored Procedure Name
        /// </summary>
        private string storedProcedureName;

        /// <summary>
        /// Initializes a new instance of the <see cref="StoredProcedureParameter"/> class.
        /// </summary>
        public StoredProcedureParameter() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="StoredProcedureParameter"/> class.
        /// </summary>
        /// <param name="storedProcedureName">the stored procedure name.</param>
        /// <param name="setting">light ADO settings</param>
        internal StoredProcedureParameter(string storedProcedureName, LightADOSetting setting)
        {
            this.storedProcedureName = storedProcedureName;
            this.LightAdoSetting = setting;
        }

        /// <summary>
        /// Gets or sets the name of parameter.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the parameter mode.
        /// </summary>
        public string Mode { get; set; }

        /// <summary>
        /// Gets or sets the parameter type name.
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// Gets or sets light ADO settings.
        /// </summary>
        internal LightADOSetting LightAdoSetting { get; set; }

        /// <summary>
        /// Gets Parameter Direction
        /// </summary>
        internal ParameterDirection GetParameterDirection
        {
            get
            {
                return !(this.Mode == "INOUT") ? ParameterDirection.Input : ParameterDirection.Output;
            }
        }

        /// <summary>
        /// Gets the parameters of stored procedure.
        /// </summary>
        internal List<StoredProcedureParameter> Parameters
        {
            get
            {
                return new Query(this.LightAdoSetting.ConnectionString).ExecuteToListOfObject<StoredProcedureParameter>("select PARAMETER_NAME as Name, PARAMETER_MODE as Mode, Data_Type as TypeName from information_schema.parameters where specific_name= @StoredProcedureName", CommandType.Text, new Parameter("StoredProcedureName", this.storedProcedureName, ParameterDirection.Input));
            }
        }
    }
}