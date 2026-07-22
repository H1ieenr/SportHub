using System;

namespace lpage.app.common
{
    public class TenantException : Exception
    {
        public int error_code { get; set; }

        public TenantException(int error_code, string message)
            : base(message)
        {
            this.error_code = error_code;
        }

        public TenantException(string message)
            : base(message)
        {
            this.error_code = 0;
        }

        public TenantException(int error_code, string message, Exception innerException)
            : base(message, innerException)
        {
            this.error_code = error_code;
        }
    }
}
