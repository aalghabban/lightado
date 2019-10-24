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

namespace LightADO.Tests
{
    using System;

    public class Application
    {
        public enum Platforms
        {
            Java,

            DotNet,

            Javascript
        }

        [PrimaryKey]
        public int ID { get; set; }

        public Application()
        {
        }

        public Application(int id)
        {
            new Query().ExecuteToObject("Applications_GetByID", this, System.Data.CommandType.StoredProcedure, new Parameter("ID", id));
        }

        [Encrypto]
        public string Name { get; set; }

        public string Description { get; set; }

        public string Logo { get; set; }

        [DefaultValue("Now", DefaultValue.ValueTypes.Properties, DefaultValue.Directions.WithNonQuery)]
        public DateTime CreateDate { get; set; }

        [DefaultValue("False", DefaultValue.ValueTypes.Value)]
        public bool IsEnabled { get; set; }

        [DefaultValue("https://www.google.com", DefaultValue.ValueTypes.Value)]
        public string OriginUrl { get; set; }

        [ColumnName("IP")]
        public string IPx { get; set; }

        public Platforms Platform { get; set; }
    }
}
