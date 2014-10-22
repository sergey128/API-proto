using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

using System.Threading.Tasks;
using System.Data.Objects.DataClasses;
using System.Data.Objects;
using System.Data.EntityClient;
using System.Data.SqlClient;

namespace AIG.Science.Backend
{
    internal static class Ext
    {
        /// <summary>
        /// http://bhrnjica.net/2011/10/13/asynchronous-call-of-entity-framework-in-c-5-0/
        /// </summary>
        public static async Task<List<T>> ExecuteAsync<T>(this IQueryable source)// where T : EntityObject
        {
            var query = (ObjectQuery)source;

            //Find conncetion from query context
            var conn = ((EntityConnection)query.Context.Connection).StoreConnection as SqlConnection;
            if (conn == null)
                return null;

            //parse for sql code from the query
            var cmdText = query.ToTraceString();
            if (string.IsNullOrEmpty(cmdText))
                return null;

            //Create SQL Command object
            var cmd = new SqlCommand(cmdText);

            //if query contains parametres append them
            cmd.Parameters.AddRange(query.Parameters.Select(x => new SqlParameter(x.Name, x.Value ?? DBNull.Value)).ToArray());

            //Configure connection string
            cmd.Connection = conn;
            cmd.Connection.ConnectionString = new SqlConnectionStringBuilder(conn.ConnectionString) { AsynchronousProcessing = true }.ToString();

            //Now open the connection
            if (cmd.Connection.State != System.Data.ConnectionState.Open)
                cmd.Connection.Open();

            //New method in C# 5.0 Execute reader async
            var reader = await cmd.ExecuteReaderAsync();
            var data = await Task.Run(() => { return query.Context.Translate<T>(reader).ToList(); });

            return data;

        }
    }
}