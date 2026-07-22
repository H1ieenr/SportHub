using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace demo_project.app
{
    public static class JsonMapperHelper
    {
        public static JsonElement Parse(string? json)
        {
            if (string.IsNullOrWhiteSpace(json))
                json = "{}";

            using var doc = JsonDocument.Parse(json);
            return doc.RootElement.Clone(); // 🔥 bắt buộc
        }
    }

}
