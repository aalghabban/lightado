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
    using System;

    /// <summary>
    /// Use this class to rename the property 
    /// to database name.
    /// </summary>
    public class ColumnName : Attribute
    {
        /// <summary>
        /// Get or set the Column Name in database
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Change the property name to 
        /// the database column name before 
        /// execute to the database.
        /// </summary>
        /// <param name="name">the name of the column</param>
        public ColumnName(string name)
        {
            this.Name = name;
        }
    }
}
