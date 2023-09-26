using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace ITCLib
{
    internal static class DbCommandExtensions
    {
        internal static int AddInputParameter<T>(this IDbCommand cmd,
            string name, T value)
        {
            var p = cmd.CreateParameter();
            p.ParameterName = name;
            p.Value = value;
            return cmd.Parameters.Add(p);
        }

        internal static int AddInputParameter<T>(this IDbCommand cmd,
            string name, Nullable<T> value) where T : struct
        {
            var p = cmd.CreateParameter();
            p.ParameterName = name;
            p.Value = value.HasValue ? (object)value : DBNull.Value;
            return cmd.Parameters.Add(p);
        }

        internal static int AddInputParameter(this IDbCommand cmd,
            string name, string value)
        {
            var p = cmd.CreateParameter();
            p.ParameterName = name;
            p.Value = string.IsNullOrEmpty(value) ? DBNull.Value : (object)value;
            return cmd.Parameters.Add(p);
        }

        internal static IDbDataParameter AddOutputParameter(this IDbCommand cmd,
            string name, DbType dbType)
        {
            var p = cmd.CreateParameter();
            p.ParameterName = name;
            p.DbType = dbType;
            p.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(p);
            return p;
        }
    }
}
