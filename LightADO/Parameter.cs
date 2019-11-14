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

namespace LightADO {
    using System.Data;

    /// <summary>
    /// Providers a way send parameters to SQL command.
    /// </summary>
    public sealed class Parameter {
        /// <summary>
        /// Initializes a new instance of the <see cref="Parameter"/> class.
        /// </summary>
        /// <param name="name">name of the Parameter</param>
        /// <param name="value">value of the Parameter</param>
        /// <param name="direction">the direction of the Parameter</param>
        public Parameter (string name, object value, ParameterDirection direction = ParameterDirection.Input) {
            this.Name = "@" + name;
            this.Value = value;
            this.Direction = direction;
        }

        /// <summary>
        /// Gets or sets the name of the Parameter.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Parameter value.
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// Gets or sets the Direction of the Parameter.
        /// </summary>
        public ParameterDirection Direction { get; set; }
    }
}