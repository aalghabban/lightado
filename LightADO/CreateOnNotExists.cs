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
    public class CreateOnNotExists : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateOnNotExists"/> class , 
        /// assuming IsExists method name will be called IsExists.
        /// </summary>
        /// <param name="useThisMethod">the method name to call in same object to check wither is exists or not</param>
        public CreateOnNotExists(string useThisMethod = "IsExists")
        {
            this.UseThisMethod = useThisMethod;
        }

        /// <summary>
        /// Gets or gets the method name to call to
        /// check if row is exists or not.
        /// </summary>
        /// <value>the method name to call</value>
        public string UseThisMethod { get; set; }
    }
}