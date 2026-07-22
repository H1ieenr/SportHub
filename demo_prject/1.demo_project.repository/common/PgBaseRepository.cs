using Dapper;
using Microsoft.Extensions.Configuration;
using demo_project.domain;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace demo_project.repository
{
    public abstract class PgBaseRepository
    {
        private readonly string _connString;

        protected PgBaseRepository(IConfiguration configuration)
        {
            _connString = configuration.GetConnectionString("postgresql")
                ?? throw new InvalidOperationException("Missing PostgreSQL connection string");
        }

        /// <summary>
        /// Thực thi 1 function SQL trả về JSON, parse tự động sang class con kế thừa ActionFunction
        /// </summary>
        protected async Task<T> ExecuteFunctionAsync<T>(
     string functionName,
     object? parameters = null,
     CancellationToken cancellation = default)
     where T : ActionFunction, new()
        {
            try
            {
                await using var conn = new NpgsqlConnection(_connString);
                await conn.OpenAsync(cancellation);

                var sql = BuildFunctionSql(functionName, parameters);
                var json = await conn.ExecuteScalarAsync<string>(sql);
                await conn.CloseAsync();

                if (string.IsNullOrWhiteSpace(json))
                    return new T
                    {
                        result = -99,
                        status = "error",
                        message = "Function returned empty",
                        record_id = -1
                    };

                var baseResult = JsonSerializer.Deserialize<ActionFunction>(json);
                if (baseResult == null)
                    return new T
                    {
                        result = -99,
                        status = "error",
                        message = "Deserialize failed",
                        record_id = -1
                    };

                var result = baseResult.CloneTo<T>();

                using (var doc = JsonDocument.Parse(json))
                {
                    if (doc.RootElement.TryGetProperty("result_data", out var resultDataEl))
                    {
                        try
                        {
                            var parsed = JsonSerializer.Deserialize<ResultDataBase>(resultDataEl.GetRawText());
                            var prop = typeof(T).GetProperty("result_data");
                            if (prop != null && prop.CanWrite)
                                prop.SetValue(result, parsed);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"[ResultData Parse Error] {ex.Message}");
                        }
                    }
                }

                if (baseResult.ExtraData != null)
                {
                    var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                        .Where(p => p.CanWrite && p.PropertyType.IsClass && p.PropertyType != typeof(string));

                    foreach (var prop in props)
                    {
                        var key = prop.Name.ToLower();
                        var match = baseResult.ExtraData
                            .FirstOrDefault(x => string.Equals(x.Key, key, StringComparison.OrdinalIgnoreCase));

                        if (match.Value.ValueKind != JsonValueKind.Undefined)
                        {
                            try
                            {
                                var deserialized = JsonSerializer.Deserialize(match.Value.GetRawText(), prop.PropertyType);
                                prop.SetValue(result, deserialized);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"[ExtraData Parse Error] Property: {prop.Name}, Type: {prop.PropertyType.Name}, Error: {ex.Message}");
                                continue;
                            }
                        }
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ ExecuteFunctionAsync Error: {ex.Message}");
                return new T
                {
                    result = -99,
                    status = "error",
                    message = $"Exception: {ex.Message}",
                    record_id = -1
                };
            }
        }
        /// <summary>
        /// Thực thi SQL function trả về TABLE / SETOF và map sang List<T>
        /// </summary>
        protected async Task<List<T>> ExecuteFunctionListAsync<T>(
            string functionName,
            object? parameters = null,
            CancellationToken cancellation = default)
        {
            try
            {
                await using var conn = new NpgsqlConnection(_connString);
                await conn.OpenAsync(cancellation);
                var sql = BuildFunctionSql(functionName, parameters);
                var result = await conn.QueryAsync<T>(
                    new CommandDefinition(
                        sql,
                        cancellationToken: cancellation
                    )
                );

                return result.AsList();
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ ExecuteFunctionTableAsync Error");
                //Console.WriteLine($"SQL: {sql}");
                Console.WriteLine($"Error: {ex.Message}");
                return new List<T>();
            }
        }
        /// <summary>
        /// Thực thi SQL function trả về 1 record (TABLE/SETOF nhưng lấy 1 dòng)
        /// </summary>
        protected async Task<T?> ExecuteFunctionSingleAsync<T>(
            string functionName,
            object? parameters = null,
            CancellationToken cancellation = default)
        {
            try
            {
                await using var conn = new NpgsqlConnection(_connString);
                await conn.OpenAsync(cancellation);

                var sql = BuildFunctionSql(functionName, parameters);

                var result = await conn.QueryFirstOrDefaultAsync<T>(
                    new CommandDefinition(
                        sql,
                        cancellationToken: cancellation
                    )
                );

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ ExecuteFunctionSingleAsync Error");
                Console.WriteLine($"Error: {ex.Message}");
                return default;
            }
        }

        /// <summary>
        /// Tự động build câu SQL gọi function PostgreSQL với ép kiểu từ object parameters
        /// </summary>
        //protected string BuildFunctionSql<T>(string functionName, T parameters)
        //{
        //    var props = parameters.GetType().GetProperties();
        //    var args = string.Join(",\n", props.Select(p => $"@{p.Name}{MapPgType(p)}"));
        //    return $"SELECT * FROM {functionName}(\n{args}\n)";
        //}
        //protected string BuildFunctionSql<T>(string functionName, T parameters)
        //{
        //    var props = parameters.GetType().GetProperties();

        //    var args = string.Join(",\n",
        //        props.Select(p =>
        //            $"p_{p.Name} := @{p.Name}{MapPgType(p)}"
        //        )
        //    );

        //    return $"SELECT * FROM {functionName}(\n{args}\n)";
        //}
        protected string BuildFunctionSql<T>(string functionName, T parameters)
        {
            if (parameters == null)
                return $"SELECT * FROM {functionName}()";

            var props = parameters.GetType().GetProperties();

            var args = string.Join(",\n", props.Select(p =>
            {
                var value = p.GetValue(parameters);
                var formattedValue = FormatPgValue(value, p.PropertyType);
                var pgType = MapPgType(p);

                return $"p_{p.Name} := {formattedValue}{pgType}";
            }));

            return $"SELECT * FROM {functionName}(\n{args}\n)";
        }



        private string MapPgType(PropertyInfo prop)
        {
            var type = prop.PropertyType;
            var isJsonb = prop.GetCustomAttribute<PgJsonbAttribute>() != null;

            if (isJsonb) return "::jsonb";

            // Arrays
            if (type == typeof(string[]))
                return "::varchar[]";
            if (type == typeof(int[]))
                return "::integer[]";
            if (type == typeof(long[]))
                return "::bigint[]";
            if (type == typeof(bool[]))
                return "::boolean[]";
            if (type == typeof(DateTime[]))
                return "::timestamp[]";

            // Scalars
            if (type == typeof(long) || type == typeof(long?)) return "::bigint";
            if (type == typeof(int) || type == typeof(int?)) return "::integer";
            if (type == typeof(string)) return "::varchar";
            if (type == typeof(bool) || type == typeof(bool?)) return "::boolean";
            if (type == typeof(decimal) || type == typeof(decimal?)) return "::numeric";
            if (type == typeof(DateTime) || type == typeof(DateTime?)) return "::timestamp";
            if (type == typeof(TimeSpan) || type == typeof(TimeSpan?)) return "::time";
            if (type == typeof(Guid)) return "::uuid";
            return "::text";
        }
        //private string BuildDebugSql(string sql, object? parameters)
        //{
        //    if (parameters == null) return sql;

        //    var props = parameters.GetType().GetProperties();
        //    var sb = new StringBuilder(sql);

        //    foreach (var prop in props)
        //    {
        //        var name = prop.Name;
        //        var value = prop.GetValue(parameters);
        //        var pgType = MapPgType(prop);

        //        string formatted = FormatPgValue(value, prop.PropertyType);

        //        sb = new StringBuilder(
        //            Regex.Replace(
        //                sb.ToString(),
        //                $@"@{name}\b",
        //                $"{formatted}{pgType}"
        //            )
        //        );
        //    }

        //    return sb.ToString();
        //}
        private string FormatPgValue(object? value, Type type)
        {
            if (value == null)
                return "NULL";

            if (type == typeof(string))
                return $"'{value.ToString()!.Replace("'", "''")}'";

            if (type == typeof(DateTime) || type == typeof(DateTime?))
                return $"'{((DateTime)value):yyyy-MM-dd HH:mm:ss}'";

            if (type == typeof(bool) || type == typeof(bool?))
                return (bool)value ? "TRUE" : "FALSE";

            if (type == typeof(Guid))
                return $"'{value}'";

            // ===== ARRAY =====
            if (type == typeof(string[]))
                return $"ARRAY[{string.Join(",", ((string[])value).Select(v => $"'{v.Replace("'", "''")}'"))}]";

            if (type == typeof(int[]))
                return $"ARRAY[{string.Join(",", (int[])value)}]";

            if (type == typeof(long[]))
                return $"ARRAY[{string.Join(",", (long[])value)}]";

            if (type == typeof(bool[]))
                return $"ARRAY[{string.Join(",", ((bool[])value).Select(v => v ? "TRUE" : "FALSE"))}]";

            if (type == typeof(DateTime[]))
                return $"ARRAY[{string.Join(",", ((DateTime[])value).Select(v => $"'{v:yyyy-MM-dd HH:mm:ss}'"))}]";

            // ===== JSONB =====
            if (value is string json)
                return $"'{json.Replace("'", "''")}'";

            // ===== DEFAULT =====
            return value.ToString() ?? "NULL";
        }



        //private string BuildDebugSql(string sql, object? parameters)
        //{
        //    if (parameters == null) return sql;

        //    var props = parameters.GetType().GetProperties();
        //    var sb = new StringBuilder(sql);

        //    foreach (var prop in props)
        //    {
        //        var name = prop.Name;
        //        var value = prop.GetValue(parameters);

        //        string formatted = value switch
        //        {
        //            null => "NULL",
        //            string s => $"'{s.Replace("'", "''")}'",
        //            DateTime dt => $"'{dt:yyyy-MM-dd HH:mm:ss}'",
        //            bool b => b ? "TRUE" : "FALSE",
        //            _ => value.ToString() ?? "NULL"
        //        };

        //        // thay thế @param bằng giá trị thực (cẩn thận nếu param trùng tên nhau)
        //        sb.Replace("@" + name, formatted);
        //    }

        //    return sb.ToString();
        //}


    }
}
