using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace ITCLib
{
    public static class DapperExtensions
    {
        public static void AddNullableParameter(this DynamicParameters parameters, string parameterName, int nullableValue)
        {
            if (nullableValue == 0)
            {
                parameters.Add(parameterName, null);
            }
            else
            {
                parameters.Add(parameterName, nullableValue);
            }
        }
    }
}
