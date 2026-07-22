// using MediatR;
// using Microsoft.EntityFrameworkCore;

// namespace Shared.Common
// {
//     public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
//             where TRequest : IRequest<TResponse>
//     {
//         private readonly DbContext _dbContext;
//         public TransactionBehavior(
//             DbContext dbContext)
//         {
//             _dbContext = dbContext;
//         }
//         public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
//         {
//             if (IsQueryRequest()) return await next();

//             var strategy = _dbContext.Database.CreateExecutionStrategy();
//             return await strategy.ExecuteAsync(async () =>
//             {
//                 using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
//                 try
//                 {
//                     var response = await next();
//                     await _dbContext.SaveChangesAsync(cancellationToken);
//                     await transaction.CommitAsync(cancellationToken);
//                     return response;
//                 }
//                 catch (Exception ex)
//                 {
//                     await transaction.RollbackAsync(cancellationToken);
//                     throw; 
//                 }
//             });
//         }
//         private bool IsQueryRequest()
//         {
//             var requestName = typeof(TRequest).Name;
//             return requestName.EndsWith("Query", StringComparison.OrdinalIgnoreCase) ||
//                    requestName.StartsWith("Get", StringComparison.OrdinalIgnoreCase);
//         }

//     }
// }