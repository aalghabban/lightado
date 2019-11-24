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
    [System.AttributeUsage(System.AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class NotNullOrEmpty : AutoValidation
    {
        public NotNullOrEmpty(bool allowWhiteSpace = false, string onNotValidMessage = "value must not be null or empty")
        {
            this.onNotValidMessage = onNotValidMessage;
            this.allowWhiteSpace = allowWhiteSpace;
        }

        private bool allowWhiteSpace;

        private string onNotValidMessage;

        public void Validate(string value)
        {
            if (string.IsNullOrEmpty(value) == true)
            {
                throw new ValidationException(this.onNotValidMessage);
            }

            if (this.allowWhiteSpace == false)
            {
                if (string.IsNullOrWhiteSpace(value) == true)
                {
                    throw new ValidationException(this.onNotValidMessage);
                }
            }
        }
    }
}