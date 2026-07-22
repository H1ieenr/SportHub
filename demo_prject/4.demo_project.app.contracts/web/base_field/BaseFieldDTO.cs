using azicloud.app.contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace demo_project.app.contracts
{
    public class BaseFieldWebItemDTO
    {
        public long id { get; set; }
        public string code { get; set; }
        public string type_key { get; set; }
        public string data_type_key { get; set; }
        public string format { get; set; }
        public JsonElement? config { get; set; }
        public JsonElement? default_value { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public bool active { get; set; }
    }
    #region fn_base_field_web_create
    public class FnBaseFieldWebCreateRequestDTO : BaseRequestDTO
    {
        public long id { get; set; }
        public string code { get; set; }
        public string type_key { get; set; }
        public string data_type_key { get; set; }
        public string format { get; set; }
        public JsonElement? config { get; set; }
        public JsonElement? default_value { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public bool active { get; set; }

    }
    #endregion

    #region fn_app_tracking_web_delete
    public class FnBaseFieldWebDeleteRequestDTO : BaseRequestDTO
    {
        public long id { get; set; }
    }
    #endregion

    #region fn_base_field_web_get_by_id
    public class FnBaseFieldWebGetByIdRequestDTO : BaseRequestDTO
    {
        public long id { get; set; }
    }
    public class FnBaseFieldWebGetByIdDTO : BaseFieldWebItemDTO
    {
    }
    #endregion
}
