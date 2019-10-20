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
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlClient;
    using System.Reflection;

    internal class OutputParmeterHandler
    {
        internal static List<Parameter> GetOutputParamters(SqlCommand sqlCommand)
        {
            List<Parameter> parameterList = new List<Parameter>();
            foreach (SqlParameter parameter in (DbParameterCollection)sqlCommand.Parameters)
            {
                if (parameter.Direction == ParameterDirection.Output)
                    parameterList.Add(new Parameter(parameter.ParameterName, parameter.Value, ParameterDirection.Input));
            }
            return parameterList;
        }

        internal static void SetOutputParameter<T>(SqlCommand sqlCommand, T objectToMap)
        {
            List<Parameter> outputParamters = OutputParmeterHandler.GetOutputParamters(sqlCommand);
            if (outputParamters == null || outputParamters.Count <= 0)
                return;
            foreach (Parameter parameter in outputParamters)
                objectToMap.GetType().GetProperty(parameter.Name.Remove(0, 2)).SetValue((object)objectToMap, parameter.Value);
        }

        internal static void SetOutputParameter<T>(SqlCommand sqlCommand, T objectToMap, params Parameter[] parameters)
        {
            List<Parameter> outputParamters = OutputParmeterHandler.GetOutputParamters(sqlCommand);
            if (outputParamters == null || outputParamters.Count <= 0)
                return;
            foreach (Parameter parameter1 in outputParamters)
            {
                Parameter parameter = parameter1;
                if (objectToMap.GetType().GetProperty(parameter.Name.Remove(0, 2)) != (PropertyInfo)null)
                    objectToMap.GetType().GetProperty(parameter.Name.Remove(0, 2)).SetValue((object)objectToMap, parameter.Value);
                else if (Array.Find<Parameter>(parameters, (Predicate<Parameter>)(x => parameter.Name.Remove(0, 1) == x.Name)) != null)
                    Array.Find<Parameter>(parameters, (Predicate<Parameter>)(x => parameter.Name.Remove(0, 1) == x.Name)).Value = parameter.Value;
            }
        }
    }
}
