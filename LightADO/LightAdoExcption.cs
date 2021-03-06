﻿/*
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
    using System;
    using System.Reflection;

    /// <summary>
    /// Provider exception for light ADO errors.
    /// </summary>
    public class LightAdoExcption : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LightAdoExcption"/> class.
        /// </summary>
        /// <param name="message">the error details</param>
        public LightAdoExcption(string message)
        {
            this.Message = message;
        }

        public LightAdoExcption(Exception ex, string message = "An error occurred, check the error details for more.")
        {
            this.Message = message;
            this.Details = ex;
        }

        /// <summary>
        /// Gets the error Messages
        /// </summary>
        public new string Message { get; private set; }

        public Exception Details { get; set; }
    }
}