using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;

namespace mini_form.shared.utils
{
    public static class TemplateUtils
    {
        // ============================================================
        // 1) Convert object + dictionary => flat dictionary<string, string>
        // ============================================================
        public static Dictionary<string, string> BuildFlatDict(object root)
        {
            var dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            if (root == null) return dict;

            FlattenValue(root, dict);
            return dict;
        }

        private static void FlattenValue(object value, Dictionary<string, string> dict)
        {
            if (value == null)
                return;

            // ============================
            // CASE 1: primitive
            // ============================
            if (IsPrimitive(value))
                return; // primitives không flatten ở đây

            // ============================
            // CASE 2: Dictionary<string, object>
            // ============================
            if (value is IDictionary<string, object> map)
            {
                foreach (var kv in map)
                {
                    if (kv.Value == null)
                        continue;

                    if (IsPrimitive(kv.Value))
                        dict[kv.Key] = kv.Value.ToString();
                    else
                        FlattenValue(kv.Value, dict);
                }
                return;
            }

            // ============================
            // CASE 3: Skip List / IEnumerable (không flatten)
            // ============================
            if (value is IEnumerable && value is not string)
            {
                // KHÔNG làm gì → KHÔNG lỗi
                return;
            }

            // ============================
            // CASE 4: Object
            // ============================
            var props = value.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var p in props)
            {
                var name = p.Name;
                var v = p.GetValue(value);

                if (v == null)
                    continue;

                if (IsPrimitive(v))
                {
                    if (v is DateTime dt)
                        dict[name] = dt.ToString("HH:mm dd/MM/yyyy");
                    else
                        dict[name] = v.ToString();
                }

                else
                {
                    FlattenValue(v, dict);
                }
            }
        }

        private static bool IsPrimitive(object obj)
        {
            var t = obj.GetType();
            return t.IsPrimitive ||
                   t == typeof(string) ||
                   t == typeof(decimal) ||
                   t == typeof(DateTime) ||
                   t == typeof(Guid);
        }

        // ============================================================
        // 2) Replace placeholder trong JsonElement bằng dict
        // ============================================================
        public static JsonElement ReplacePlaceholders(JsonElement element, Dictionary<string, string> dict)
        {
            object Recursive(JsonElement el)
            {
                switch (el.ValueKind)
                {
                    case JsonValueKind.Object:
                        var obj = new Dictionary<string, object>();
                        foreach (var prop in el.EnumerateObject())
                            obj[prop.Name] = Recursive(prop.Value);
                        return obj;

                    case JsonValueKind.Array:
                        var arr = new List<object>();
                        foreach (var item in el.EnumerateArray())
                            arr.Add(Recursive(item));
                        return arr;

                    case JsonValueKind.String:
                        var str = el.GetString() ?? "";
                        foreach (var kv in dict)
                            str = str.Replace($"{{{{{kv.Key}}}}}", kv.Value);
                        return str;

                    case JsonValueKind.Number:
                        return el.GetDouble(); // hoặc GetInt64 nếu chắc chắn là số nguyên

                    case JsonValueKind.True:
                    case JsonValueKind.False:
                        return el.GetBoolean();

                    case JsonValueKind.Null:
                        return null;

                    default:
                        return el.GetRawText(); // fallback
                }
            }

            var replaced = Recursive(element);
            string json = JsonSerializer.Serialize(replaced);
            return JsonDocument.Parse(json).RootElement;
        }
    }
}
