using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;

namespace ExpressionExtensionSQL.Dapper {
    public static class DapperExtension {


        private static KeyValuePair<string, DynamicParameters> GetWhere<TReturn>(Expression<Func<TReturn, bool>> expression, string sql) {
            var whereSql = expression.ToSql();
            var parameter = new DynamicParameters();

            if (whereSql != null) {
                foreach (var param in whereSql.Parameters) {
                    parameter.Add(param.Key, param.Value);
                }
                sql = sql.Replace("{where}", " WHERE " + whereSql.Sql);
            }

            return new KeyValuePair<string, DynamicParameters>(sql, parameter);
        }

        /// <summary>
        ///  Return a sequence of dynamic objects with properties matching the columns.
        /// </summary>
        /// <param name="cnn">The connection to query on.</param>
        /// <param name="sql">The SQL to execute for the query.</param>
        /// <param name="expression">The parameters to pass, if any.</param>
        /// <param name="transaction">The transaction to use, if any.</param>
        /// <param name="buffered">Whether to buffer the results in memory.</param>
        /// <param name="commandTimeout">The command timeout (in seconds).</param>
        /// <param name="commandType">The type of command to execute.</param>
        /// <returns></returns>
        public static IEnumerable<dynamic> Query(this IDbConnection cnn,
            string sql,
            Expression<Func<dynamic, bool>> expression,
            IDbTransaction transaction = null,
            bool buffered = true,
            int? commandTimeout = null, CommandType? commandType = null) {

            var whereSql = GetWhere(expression, sql);
            return cnn.Query(whereSql.Key, whereSql.Value, transaction, buffered, commandTimeout, commandType);
        }

        /// <summary>
        /// Perform an object of defined <typeparamref name="TReturn"/> type. 
        /// This returns a single type, combined from the raw types via <paramref name="map"/>.
        /// </summary>
        /// <typeparam name="TReturn">The combined type to return.</typeparam>
        /// <param name="cnn">The connection to query on.</param>
        /// <param name="sql">The SQL to execute for the query.</param>
        /// <param name="expression">The parameters to pass, if any.</param>
        /// <param name="transaction">The transaction to use, if any.</param>
        /// <param name="buffered">Whether to buffer the results in memory.</param>
        /// <param name="commandTimeout">The command timeout (in seconds).</param>
        /// <param name="commandType">The type of command to execute.</param>
        /// <returns></returns>
        public static IEnumerable<TReturn> Query<TReturn>(this IDbConnection cnn,
            string sql,
            Expression<Func<TReturn, bool>> expression,
            IDbTransaction transaction = null,
            bool buffered = true,
            int? commandTimeout = null, CommandType? commandType = null)
        {

            var whereSql = GetWhere(expression, sql);
            return cnn.Query<TReturn>(whereSql.Key, whereSql.Value, transaction, buffered, commandTimeout, commandType);
        }

        /// <summary>
        /// Perform a multi-mapping query with 2 input types. 
        /// This returns a single type, combined from the raw types via <paramref name="map"/>.
        /// </summary>
        /// <typeparam name="TFirst">The first type in the recordset.</typeparam>
        /// <typeparam name="TSecond">The second type in the recordset.</typeparam>
        /// <typeparam name="TReturn">The combined type to return.</typeparam>
        /// <param name="cnn">The connection to query on.</param>
        /// <param name="sql">The SQL to execute for this query.</param>
        /// <param name="map">The function to map row types to the return type.</param>
        /// <param name="expression">The parameters to pass, if any.</param>
        /// <param name="transaction">The transaction to use for this query.</param>
        /// <param name="buffered">Whether to buffer the results in memory.</param>
        /// <param name="splitOn">The field we should split and read the second object from (default: "Id").</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
        /// <param name="commandType">Is it a stored proc or a batch?</param>
        /// <returns>An enumerable of <typeparamref name="TReturn"/>.</returns>
        public static IEnumerable<TReturn> Query<TFirst, TSecond, TReturn>(this IDbConnection cnn,
            string sql,
            Expression<Func<TReturn, bool>> expression,
            Func<TFirst, TSecond, TReturn> map,
            IDbTransaction transaction = null,
            bool buffered = true, string splitOn = "Id",
            int? commandTimeout = null,
            CommandType? commandType = null) {

            var whereSql = GetWhere(expression, sql);

            return cnn.Query<TFirst, TSecond, TReturn>(whereSql.Key, map, whereSql.Value, transaction, buffered, splitOn, commandTimeout, commandType);
        }

        /// <summary>
        /// Perform a multi-mapping query with 3 input types. 
        /// This returns a single type, combined from the raw types via <paramref name="map"/>.
        /// </summary>
        /// <typeparam name="TFirst">The first type in the recordset.</typeparam>
        /// <typeparam name="TSecond">The second type in the recordset.</typeparam>
        /// <typeparam name="TThird">The third type in the recordset.</typeparam>
        /// <typeparam name="TReturn">The combined type to return.</typeparam>
        /// <param name="cnn">The connection to query on.</param>
        /// <param name="sql">The SQL to execute for this query.</param>
        /// <param name="map">The function to map row types to the return type.</param>
        /// <param name="expression">The parameters to pass, if any.</param>
        /// <param name="transaction">The transaction to use for this query.</param>
        /// <param name="buffered">Whether to buffer the results in memory.</param>
        /// <param name="splitOn">The field we should split and read the second object from (default: "Id").</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
        /// <param name="commandType">Is it a stored proc or a batch?</param>
        /// <returns>An enumerable of <typeparamref name="TReturn"/>.</returns>
        public static IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TReturn>(this IDbConnection cnn,
            string sql,
            Expression<Func<TReturn, bool>> expression,
            Func<TFirst, TSecond, TThird, TReturn> map,
            IDbTransaction transaction = null,
            bool buffered = true, string splitOn = "Id",
            int? commandTimeout = null,
            CommandType? commandType = null) {


            var whereSql = GetWhere(expression, sql);

            return cnn.Query<TFirst, TSecond, TThird, TReturn>(whereSql.Key, map, whereSql.Value, transaction, buffered, splitOn, commandTimeout, commandType);
        }


        /// <summary>
        /// Perform a multi-mapping query with 4 input types. 
        /// This returns a single type, combined from the raw types via <paramref name="map"/>.
        /// </summary>
        /// <typeparam name="TFirst">The first type in the recordset.</typeparam>
        /// <typeparam name="TSecond">The second type in the recordset.</typeparam>
        /// <typeparam name="TThird">The third type in the recordset.</typeparam>
        /// <typeparam name="TFourth">The fourth type in the recordset.</typeparam>
        /// <typeparam name="TReturn">The combined type to return.</typeparam>
        /// <param name="cnn">The connection to query on.</param>
        /// <param name="sql">The SQL to execute for this query.</param>
        /// <param name="map">The function to map row types to the return type.</param>
        /// <param name="expression">The parameters to pass, if any.</param>
        /// <param name="transaction">The transaction to use for this query.</param>
        /// <param name="buffered">Whether to buffer the results in memory.</param>
        /// <param name="splitOn">The field we should split and read the second object from (default: "Id").</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
        /// <param name="commandType">Is it a stored proc or a batch?</param>
        /// <returns>An enumerable of <typeparamref name="TReturn"/>.</returns>
        public static IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TReturn>(this IDbConnection cnn,
            string sql,
            Expression<Func<TReturn, bool>> expression,
            Func<TFirst, TSecond, TThird, TFourth, TReturn> map,
            IDbTransaction transaction = null,
            bool buffered = true, string splitOn = "Id",
            int? commandTimeout = null,
            CommandType? commandType = null) {


            var whereSql = GetWhere(expression, sql);

            return cnn.Query<TFirst, TSecond, TThird, TFourth, TReturn>(whereSql.Key, map, whereSql.Value, transaction, buffered, splitOn, commandTimeout, commandType);
        }
        /// <summary>
        /// Perform a multi-mapping query with 5 input types. 
        /// This returns a single type, combined from the raw types via <paramref name="map"/>.
        /// </summary>
        /// <typeparam name="TFirst">The first type in the recordset.</typeparam>
        /// <typeparam name="TSecond">The second type in the recordset.</typeparam>
        /// <typeparam name="TThird">The third type in the recordset.</typeparam>
        /// <typeparam name="TFourth">The fourth type in the recordset.</typeparam>
        /// <typeparam name="TFifth">The fifth type in the recordset.</typeparam>
        /// <typeparam name="TReturn">The combined type to return.</typeparam>
        /// <param name="cnn">The connection to query on.</param>
        /// <param name="sql">The SQL to execute for this query.</param>
        /// <param name="map">The function to map row types to the return type.</param>
        /// <param name="expression">The parameters to pass, if any.</param>
        /// <param name="transaction">The transaction to use for this query.</param>
        /// <param name="buffered">Whether to buffer the results in memory.</param>
        /// <param name="splitOn">The field we should split and read the second object from (default: "Id").</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
        /// <param name="commandType">Is it a stored proc or a batch?</param>
        /// <returns>An enumerable of <typeparamref name="TReturn"/>.</returns>
        public static IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(this IDbConnection cnn,
            string sql,
            Expression<Func<TReturn, bool>> expression,
            Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> map,
            IDbTransaction transaction = null,
            bool buffered = true, string splitOn = "Id",
            int? commandTimeout = null,
            CommandType? commandType = null) {


            var whereSql = GetWhere(expression, sql);

            return cnn.Query<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(whereSql.Key, map, whereSql.Value, transaction, buffered, splitOn, commandTimeout, commandType);
        }
        /// <summary>
        /// Perform a multi-mapping query with 6 input types. 
        /// This returns a single type, combined from the raw types via <paramref name="map"/>.
        /// </summary>
        /// <typeparam name="TFirst">The first type in the recordset.</typeparam>
        /// <typeparam name="TSecond">The second type in the recordset.</typeparam>
        /// <typeparam name="TThird">The third type in the recordset.</typeparam>
        /// <typeparam name="TFourth">The fourth type in the recordset.</typeparam>
        /// <typeparam name="TFifth">The fifth type in the recordset.</typeparam>
        /// <typeparam name="TSixth">The sixth type in the recordset.</typeparam>
        /// <typeparam name="TReturn">The combined type to return.</typeparam>
        /// <param name="cnn">The connection to query on.</param>
        /// <param name="sql">The SQL to execute for this query.</param>
        /// <param name="map">The function to map row types to the return type.</param>
        /// <param name="expression">The parameters to pass, if any.</param>
        /// <param name="transaction">The transaction to use for this query.</param>
        /// <param name="buffered">Whether to buffer the results in memory.</param>
        /// <param name="splitOn">The field we should split and read the second object from (default: "Id").</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
        /// <param name="commandType">Is it a stored proc or a batch?</param>
        /// <returns>An enumerable of <typeparamref name="TReturn"/>.</returns>
        public static IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(this IDbConnection cnn,
            string sql,
            Expression<Func<TReturn, bool>> expression,
            Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn> map,
            IDbTransaction transaction = null,
            bool buffered = true, string splitOn = "Id",
            int? commandTimeout = null,
            CommandType? commandType = null) {


            var whereSql = GetWhere(expression, sql);

            return cnn.Query<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(whereSql.Key, map, whereSql.Value, transaction, buffered, splitOn, commandTimeout, commandType);
        }
        /// <summary>
        /// Perform a multi-mapping query with 7 input types. If you need more types -> use Query with Type[] parameter.
        /// This returns a single type, combined from the raw types via <paramref name="map"/>.
        /// </summary>
        /// <typeparam name="TFirst">The first type in the recordset.</typeparam>
        /// <typeparam name="TSecond">The second type in the recordset.</typeparam>
        /// <typeparam name="TThird">The third type in the recordset.</typeparam>
        /// <typeparam name="TFourth">The fourth type in the recordset.</typeparam>
        /// <typeparam name="TFifth">The fifth type in the recordset.</typeparam>
        /// <typeparam name="TSixth">The sixth type in the recordset.</typeparam>
        /// <typeparam name="TSeventh">The seventh type in the recordset.</typeparam>
        /// <typeparam name="TReturn">The combined type to return.</typeparam>
        /// <param name="cnn">The connection to query on.</param>
        /// <param name="sql">The SQL to execute for this query.</param>
        /// <param name="map">The function to map row types to the return type.</param>
        /// <param name="expression">The parameters to pass, if any.</param>
        /// <param name="transaction">The transaction to use for this query.</param>
        /// <param name="buffered">Whether to buffer the results in memory.</param>
        /// <param name="splitOn">The field we should split and read the second object from (default: "Id").</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
        /// <param name="commandType">Is it a stored proc or a batch?</param>
        /// <returns>An enumerable of <typeparamref name="TReturn"/>.</returns>
        public static IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(this IDbConnection cnn,
            string sql,
            Expression<Func<TReturn, bool>> expression,
            Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh,TReturn> map,
            IDbTransaction transaction = null,
            bool buffered = true, string splitOn = "Id",
            int? commandTimeout = null,
            CommandType? commandType = null) {
            
            var whereSql = GetWhere(expression, sql);

            return cnn.Query<TFirst, TSecond, TThird, TFourth, TFifth, TSixth,TSeventh, TReturn>(whereSql.Key, map, whereSql.Value, transaction, buffered, splitOn, commandTimeout, commandType);
        }

        /// <summary>
        /// Executes a query, returning the data typed as T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cnn"></param>
        /// <param name="sql"></param>
        /// <param name="expression"></param>
        /// <param name="transaction"></param>
        /// <param name="buffered"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static IEnumerable<T> Query<T>(this IDbConnection cnn, string sql, Expression<Func<T, bool>> expression, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null) {
            var whereSql = GetWhere(expression, sql);
            return cnn.Query<T>(whereSql.Key, whereSql.Value, transaction:transaction,buffered:buffered,commandTimeout:commandTimeout,commandType:commandType);
        }
        /// <summary>
        /// Execute Scalar
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="cnn"></param>
        /// <param name="sql"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static T ExecuteScalar<T,TReturn>(this IDbConnection cnn, string sql, Expression<Func<TReturn, bool>> expression) {
            var whereSql = GetWhere(expression, sql);
            return cnn.ExecuteScalar<T>(whereSql.Key, whereSql.Value);
        }
    }
}
