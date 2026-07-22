using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mini_form.shared
{
    public class OpenApiException : Exception
    {
        public string ErrorCode { get; }
        public int StatusCode { get; }

        public OpenApiException(string code, string message, int statusCode)
            : base(message)
        {
            ErrorCode = code;
            StatusCode = statusCode;
        }
    }

}
