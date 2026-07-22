using System;

namespace mini_form.shared
{
    public class RepositoryException : NotificationException
    {
        public RepositoryException(string message) : base(message, -101)
        {
        }

        public RepositoryException(string message, Exception inner)
            : base(message, -101, inner)
        {
        }
    }
}
