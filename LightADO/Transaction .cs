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
    using System.Data;

    /// <summary>
    /// Provide a way to handle Transaction.
    /// </summary>
    public class Transaction
    {
        /// <summary>
        /// Gets or sets data of Transaction.
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// Gets or sets the Transaction command.
        /// </summary>
        public string Command { get; set; }

        /// <summary>
        /// Gets or sets the Transaction command type.
        /// </summary>
        public CommandType CommandType { get; set; }

        /// <summary>
        /// Gets or sets the parameters list.
        /// </summary>
        public Parameter[] Parameters { get; set; }
    }
}