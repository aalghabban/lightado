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
    public class Types
    {
        /// <summary>
        /// The Configuration Sections
        /// </summary>
        internal enum ConfigurationSections
        {
            /// <summary>
            /// Connection String Section
            /// </summary>
            ConnectionString,

            /// <summary>
            /// The App Settings section
            /// </summary>
            AppSettings
        }

        /// <summary>
        /// The direction of setting 
        /// up the default value.
        /// </summary>
        public enum Directions
        {
            /// <summary>
            /// Only with Query.
            /// </summary>
            WithQuery,

            /// <summary>
            /// Only with non Query.
            /// </summary>
            WithNonQuery,

            /// <summary>
            /// With both of them.
            /// </summary>
            WithBoth
        }

        /// <summary>
        /// Encrypt Engine Options
        /// </summary>
        internal enum OprationType
        {
            /// <summary>
            /// Encrypt string.
            /// </summary>
            Encrypt,

            /// <summary>
            /// decrypt string.
            /// </summary>
            Descrypt
        }

        /// <summary>
        /// Type of supported Format.
        /// </summary>
        public enum FormatType
        {
            /// <summary>
            /// As Xml
            /// </summary>
            XML,

            /// <summary>
            /// As JSON
            /// </summary>
            Json,
        }
    }
}