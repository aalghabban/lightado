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

    [AttributeUsage(AttributeTargets.Property)]
    /// <summary>
    /// Use this class to rename the property 
    /// to database name.
    /// </summary>
    public class ColumnName : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnName"/> class.
        /// </summary>
        /// <param name="name">the name of the column</param>
        public ColumnName(string name)
        {
            if (string.IsNullOrEmpty(name) == true)
            {
                throw new LightAdoExcption("Column name can't be null");
            }

            this.Name = name;
        }

        /// <summary>
        /// Gets or sets the Column Name in database
        /// </summary>
        public string Name { get; set; }
    }
}