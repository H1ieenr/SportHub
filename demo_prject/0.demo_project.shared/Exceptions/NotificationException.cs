using System;

namespace mini_form.shared
{
    public class NotificationException : Exception
    {
        public NotificationException(string message, int code)
            : base(message)
        {
            this.code = code;
        }

        public NotificationException(string message, int code, Exception inner)
            : base(message, inner)
        {
        }

        public int code { get; set; }
    }
}
