﻿/*
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

    public class QueryBase
    {
        public LightADOSetting LightAdoSetting { get; set; }

        public QueryBase()
        {
            this.LightAdoSetting = new LightADOSetting();
        }

        public QueryBase(string connectionString, bool loadFromConfigrationFile)
        {
            this.LightAdoSetting = new LightADOSetting(connectionString, loadFromConfigrationFile);
        }

        internal static void ThrowExacptionOrEvent(
          OnError onError,
          Exception exception,
          string extraInfo = "")
        {
            if (onError == null)
                throw exception;
            exception.Source = extraInfo;
            onError(exception);
        }
    }
}
