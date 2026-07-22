
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace demo_project.domain
{
    //public class ActionFunction
    //{
    //    public int result { get; set; }
    //    public string status { get; set; } = "";
    //    public string message { get; set; } = "";
    //    public string action { get; set; } = "";
    //    public long record_id { get; set; }
    //}
    public class ActionFunction
    {
        public int result { get; set; }
        public string? status { get; set; } 
        public string? message { get; set; } 
        public string? action { get; set; } 
        public long? record_id { get; set; }
        public ResultDataBase? result_data { get; set; }

        // Bắt tất cả field động (support, user, admin, ...)
        [JsonExtensionData]
        public Dictionary<string, JsonElement>? ExtraData { get; set; }
        /// <summary>
        /// Parse ra class tương ứng với key mong muốn (vd: "support")
        /// </summary>
        public T? GetDetail<T>(string key)
        {
            if (ExtraData == null || !ExtraData.TryGetValue(key, out var element))
                return default;

            return JsonSerializer.Deserialize<T>(element.GetRawText());
        }
        // Clone sang subclass để giữ các field chung
        public T CloneTo<T>() where T : ActionFunction, new()
        {
            var clone = new T
            {
                result = this.result,
                status = this.status,
                message = this.message,
                action = this.action,
                record_id = this.record_id,
                ExtraData = this.ExtraData
            };
            return clone;
        }
    }

    public class ResultDataBase
    {
        public JsonElement? para_input { get; set; } // hoặc Dictionary<string, object>
        public string? message_error { get; set; }

        /// <summary>
        /// Parse para_input sang model cụ thể khi cần
        /// </summary>
        public T? GetParaInput<T>()
        {
            if (para_input is null || para_input.Value.ValueKind == JsonValueKind.Undefined)
                return default;

            return JsonSerializer.Deserialize<T>(para_input.Value.GetRawText());
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class PgJsonbAttribute : Attribute { }


}