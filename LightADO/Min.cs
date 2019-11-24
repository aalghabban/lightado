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
    public class Min : AutoValidation
    {
        public Min(double minimum, string onNotValidMessage = "value not valid")
        {
            this.minimum = minimum;
            this.onNotValidMessage = onNotValidMessage;
        }

        public string onNotValidMessage;

        public double minimum;

        public void Validate(double value)
        {
            if (value < this.minimum)
            {
                throw new ValidationException(this.onNotValidMessage);
            }
        }
    }
}