using System;
using System.Collections.Generic;
using System.Text;

namespace mini_form.shared
{
    //public class ResponseDTO
    //{
    //    public ResponseDTO()
    //    {
    //    }

    //    public ResponseDTO(object result)
    //    {
    //        this.result = result;
    //        success = true;
    //    }

    //    public ResponseDTO(ErrorDTO error)
    //    {
    //        success = false;
    //        this.result = null;
    //        this.error = error;
    //    }

    //    public object result { get; set; } = null;
    //    public bool success { get; set; } = false;
    //    public ErrorDTO error { get; set; } = null;

    //    public static ResponseDTO Success(object result)
    //    {
    //        return new ResponseDTO
    //        {
    //            success = true,
    //            result = result,
    //            error = null,
    //        };
    //    }

    //    public static ResponseDTO Error(object result, int code, string message, string description)
    //    {
    //        return new ResponseDTO
    //        {
    //            success = false,
    //            result = result,
    //            error = new ErrorDTO(code, message, description),
    //        };
    //    }
    //}

    //public class ResponseDTO<T> : ResponseDTO
    //{
    //    /// <summary>
    //    /// Kết quả trả về nếu success = true
    //    /// </summary>
    //    public new T? result { get; set; } = default;
    //}
    public class PaginationDTO<T>
    {
        public List<T> items { get; set; }
        public long total_count { get; set; }
    }
    public class PaginationDTO
    {

        public int page { get; set; } = 0;
        public int page_size { get; set; } = 0;
        public string search_text { get; set; } = "";
        public string sort_option { get; set; } = "";
        public string sort_type { get; set; } = "";
        public long lang_id { get; set; } = 0;
    }
    public class ErrorDTO
    {
        public ErrorDTO(int code, string message, string description)
        {
            this.code = code;
            this.message = message ?? "";
            this.description = description ?? "";
        }

        public ErrorDTO(int code, string message)
            : this(code, message, message)
        {
        }

        public int code { get; set; } = -1;
        public string message { get; set; } = "";
        public object description { get; set; } = "";
    }
}
