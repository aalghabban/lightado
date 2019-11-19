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
    /// <summary>
    /// Will be fired after the connection closed.
    /// </summary>
    public delegate void AfterCloseConnection();

    /// <summary>
    /// Will be fired After query get Executed.
    /// </summary>
    public delegate void AfterExecute();

    /// <summary>
    /// Will be fired After Open Connection.
    /// </summary>
    public delegate void AfterOpenConnection();

    /// <summary>
    /// Will be fired Before Close Connection.
    /// </summary>
    public delegate void BeforeCloseConnection();

    /// <summary>
    /// Will be fired Before Execute.
    /// </summary>
    public delegate void BeforeExecute();

    /// <summary>
    /// Will be fired Before Open Connection.
    /// </summary>
    public delegate void BeforeOpenConnection();

    /// <summary>
    /// Will be fired On Error.
    /// </summary>
    /// <param name="ex">the error to throw</param>
    public delegate void OnError(LightAdoExcption ex);
}