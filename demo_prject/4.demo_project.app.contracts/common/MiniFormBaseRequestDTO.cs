using azicloud.app.contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace demo_project.app.contracts
{
    public class MiniformBaseRequestDTO : TenantConsumerBaseRequestDTO
    {
        public string miniapp_id { get; set; } = "";
        public Guid tenant_public_id { get; set; } 
        // 🔥 Dùng `new` để đè thuộc tính `lang_id` và ánh xạ JSON
        [JsonPropertyName("lang_id")]
        public new long lang_id
        {
            get => base.lang_id;
            set => base.lang_id = value;
        }

    }

}
