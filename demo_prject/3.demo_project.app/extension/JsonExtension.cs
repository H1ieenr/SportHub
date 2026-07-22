using System.Text.Json;

namespace demo_project.app
{
    public static class SystemTextJsonExtension
    {
        public static bool TryGetBool(this JsonElement element, out bool value)
        {
            value = default;

            switch (element.ValueKind)
            {
                case JsonValueKind.True:
                    value = true;
                    return true;

                case JsonValueKind.False:
                    value = false;
                    return true;

                case JsonValueKind.String:
                    return bool.TryParse(element.GetString(), out value);

                case JsonValueKind.Number:
                    if (element.TryGetInt32(out var i))
                    {
                        value = i != 0;
                        return true;
                    }
                    return false;

                default:
                    return false;
            }
        }
    }
}