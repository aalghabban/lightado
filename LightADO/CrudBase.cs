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
    using System.Data;
    using System.Reflection;

    public class CrudBase<T>
    {
        private Table table = null;

        public CrudBase()
        {
            table = this.GetType().GetCustomAttribute<Table>(true);
        }

        public CrudBase(int id)
        {
            table = this.GetType().GetCustomAttribute<Table>(true);

            new Query().ExecuteToObject<T>(
                table.Name + "_GetById",
                this,
                CommandType.StoredProcedure,
                new Parameter("ID", id));
        }

        public void Create()
        {
            this.DoNonQuery("Create");
        }

        public void Update()
        {
            this.DoNonQuery("Update");
        }

        public void Delete()
        {
            this.DoNonQuery("Delete");
        }

        public T Get(int id)
        {
            return new Query().ExecuteToObject<T>(
                table.Name + "_GetById",
                CommandType.StoredProcedure,
                new Parameter("ID", id));
        }

        private void DoNonQuery(string actionName)
        {
            if (table == null)
            {
                throw new LightAdoExcption(
                    "In order to use Base Crud you will need to add a table name attribute to the class, lighado will call SP like following: tablename_getById, tablename_create, tablename_update, tablename_delete");
            }

            new NonQuery().Execute(table.Name + "_" + actionName, this);
        }
    }
}