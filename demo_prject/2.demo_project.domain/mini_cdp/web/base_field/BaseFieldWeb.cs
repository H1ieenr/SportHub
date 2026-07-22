using demo_project.domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace demo_project.domain
{
    public class BaseFieldWebItem
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
    public class FnBaseFieldWebCreateParamsFunction
    {
        public long id { get; set; }
        public string code { get; set; }
        public string type_key { get; set; }
        public string data_type_key { get; set; }
        public string format { get; set; }
        [PgJsonb]
        public string? config { get; set; }
        [PgJsonb]
        public string? default_value { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public bool active { get; set; }
        public long user_id { get; set; }

    }
    #endregion

    #region fn_app_tracking_web_delete
    public class FnBaseFieldWebDeleteParamsFunction
    {
        public long id { get; set; }
        public long user_id { get; set; }
    }
    #endregion

    #region fn_base_field_web_get_by_id
    public class FnBaseFieldWebGetByIdParamsFunction
    {
        public long id { get; set; }
    }
    public class FnBaseFieldWebGetByIdFunction : ActionFunction
    {
        public BaseFieldWebItem base_field { get; set; }
    }
    #endregion
}
