//using azicloud.library;
//using System;
//using System.Threading.Tasks;
//using Microsoft.Extensions.Logging;
//using azicloud.app.contracts;
//using Microsoft.Data.SqlClient;
//using azicloud.common;

//namespace demo_project.api
//{
//    public static class GrpcLoggerExtension
//    {
//        public static async Task<T> GrpcResponseOrDefault<T>(Func<Task<T>> func,ILogger _logger)
//        {
//            try
//            {
//                return await func();
//            }
//            catch (RepositoryException ex)
//            {
//                var logMessage = new LogMessage
//                {
//                    service = LogService.Notification,
//                    value = ex.Message,

//                    error_code = ex.code,
//                    time = DateTime.UtcNow,
//                    type = LogType.Repository,
//                };
//                _logger.LogError(ex, logMessage);
//                return default;
//            }
//            catch (SqlException ex)
//            {
//                var logMessage = new LogMessage
//                {
//                    service = LogService.Notification,
//                    value = ex.Message,
//                    error_code = -101,
//                    time = DateTime.UtcNow,
//                    type = LogType.Repository,
//                };
//                _logger.LogError(ex, logMessage);
//                return default;
//            }
//            catch (Exception ex)
//            {
//                var logMessage = new LogMessage
//                {
//                    service = LogService.Notification,
//                    value = ex.Message,
//                    error_code = -100,
//                    time = DateTime.UtcNow,
//                    type = LogType.Repository,
//                };
//                _logger.LogError(ex, logMessage);
//                return default;
//            }
//        }
//        public static void LogError(this ILogger logger, Exception ex, LogMessage message)
//        {
//            logger.Log(Microsoft.Extensions.Logging.LogLevel.Error, 0, message, ex, (l, e) => "");
//        }
//    }
//}
