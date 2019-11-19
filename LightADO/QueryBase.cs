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
    using System;

    /// <summary>
    /// Provides shared functions between Query and Non Query.
    /// </summary>
    public class QueryBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QueryBase"/> class.
        /// </summary>
        public QueryBase()
        {
            this.LightAdoSetting = new LightADOSetting();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryBase"/> class.
        /// </summary>
        /// <param name="connectionString">the database connection string</param>
        public QueryBase(string connectionString)
        {
            this.LightAdoSetting = new LightADOSetting(connectionString);
        }

        /// <summary>
        /// Gets or sets light ADO settings
        /// </summary>
        public LightADOSetting LightAdoSetting { get; set; }

        /// <summary>
        /// Throw exception or event if it's not null.
        /// </summary>
        /// <param name="onError">the error to throw</param>
        /// <param name="exception">the exception to throw</param>
        /// <param name="extraInfo">any more details</param>
        internal static void ThrowExacptionOrEvent(OnError onError, Exception exception, string extraInfo = "")
        {
            if (onError == null)
            {
                throw new LightAdoExcption(exception);
            }

            exception.Source = extraInfo;
            onError(new LightAdoExcption(exception));
        }
    }
}