using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace demo_project.repository
{
    public sealed class JsonElementTypeHandler : SqlMapper.TypeHandler<JsonElement>
    {
        public override JsonElement Parse(object value)
        {
            if (value == null || value is DBNull)
                return default;

            // postgres jsonb thường trả string
            if (value is string s)
            {
                using var doc = JsonDocument.Parse(s);
                return doc.RootElement.Clone();
            }

            // fallback
            using var fallback = JsonDocument.Parse(value.ToString()!);
            return fallback.RootElement.Clone();
        }

        public override void SetValue(IDbDataParameter parameter, JsonElement value)
        {
            parameter.Value = value.GetRawText();
            parameter.DbType = DbType.String;
        }
    }
}
