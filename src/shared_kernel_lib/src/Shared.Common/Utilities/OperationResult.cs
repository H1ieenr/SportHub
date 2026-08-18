namespace Shared.Common
{
    public class OperationResult<T>
    {
        public bool is_success { get; set; }
        public string message { get; set; } = string.Empty;
        public string? code { get; set; }
        public T? data { get; set; }
        public OperationType operation_type { get; set; }
        public List<string> errors { get; set; } = new();
        public DateTime timestamp { get; set; } = DateTime.Now;

        public static OperationResult<T> Success(T data, string message = "Thành công") 
            => new() { is_success = true, data = data, message = message, operation_type = OperationType.Read };

        public static OperationResult<T> Created(T data, string message = "Thêm thành công") 
            => new() { is_success = true, data = data, message = message, operation_type = OperationType.Create };

        public static OperationResult<T> Updated(T data, string message = "Cập nhật thành công") 
            => new() { is_success = true, data = data, message = message, operation_type = OperationType.Update };

        public static OperationResult<T> Deleted(string message = "Xóa thành công") 
            => new() { is_success = true, message = message, operation_type = OperationType.Delete };
        public static OperationResult<T> Nodata(T data, string message = "Không có dữ liệu! ") 
            => new() { is_success = false, data = data, message = message, code = "NOT_FOUND", operation_type = OperationType.InternalError };
    
        public static OperationResult<T> Failure(string message = "Có lỗi xảy ra! ", string? code = null, List<string>? errors = null) 
            => new() { is_success = false, message = message, code = code, errors = errors ?? new(), operation_type = OperationType.InternalError };
    
    }
}