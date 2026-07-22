using System.Net;

namespace Shared.Exceptions
{
    public abstract class BaseException : Exception
    {
        public string Code { get; }
        public HttpStatusCode StatusCode { get; }
        public List<string>? Errors { get; }

        protected BaseException(string message, string code, HttpStatusCode statusCode, List<string>? errors = null)
            : base(message)
        {
            Code = code;
            StatusCode = statusCode;
            Errors = errors;
        }
    }
    public class NotFoundException : BaseException
    {
        public NotFoundException(string message = "Không có dữ liệu! ", string code = "NOT_FOUND")
            : base(message, code, HttpStatusCode.NotFound) { }
    }

    public class ValidationException : BaseException
    {
        public ValidationException(string message, List<string> errors, string code = "VALIDATION_ERROR")
            : base(message, code, HttpStatusCode.BadRequest, errors) { }
    }

    public class BadRequestException : BaseException
    {
        public BadRequestException(string message, string code = "BAD_REQUEST")
            : base(message, code, HttpStatusCode.BadRequest) { }
    }

    public class ConflictException : BaseException
    {
        public ConflictException(string message, string code = "CONFLICT")
            : base(message, code, HttpStatusCode.Conflict) { }
    }

    public class ForbiddenException : BaseException
    {
        public ForbiddenException(string message, string code = "FORBIDDEN")
            : base(message, code, HttpStatusCode.Forbidden) { }
    }
}