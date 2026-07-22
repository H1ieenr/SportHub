using System;

namespace mini_form.shared
{
    public class AppServiceException : NotificationException
    {
        public AppServiceException(string message) 
            : base(message, -100)
        {
        }

        public AppServiceException(string message, Exception inner)
            : base(message, -100, inner)
        {
        }
    }
}
