using sporthub.app.contracts;
using System.ComponentModel; 
using System.Reflection; 

namespace sporthub.app
{
    public static class EnumHelper
    {
        public static List<EnumItemDTO> ToList<TEnum>() where TEnum : struct, Enum
        {
            return Enum.GetValues<TEnum>().Select(e =>
            {
                var member = typeof(TEnum).GetMember(e.ToString())[0];
                var desc = member.GetCustomAttribute<DescriptionAttribute>()?.Description;

                return new EnumItemDTO
                {
                    value = Convert.ToInt32(e),
                    name = e.ToString(),
                    label = desc ?? e.ToString()
                };
            }).ToList();
        }
    }
}