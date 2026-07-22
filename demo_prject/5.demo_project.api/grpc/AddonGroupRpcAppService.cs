//using Microsoft.Extensions.Logging;
//using System.Threading.Tasks;
//using azicloud.grpc.client;
//using azicloud.grpc;
//using ProtoBuf.Grpc;
//using demo_project.app.contracts;

//namespace demo_project.api.grpc
//{
//    public class AddonGroupRpcAppService : IChannelAddonGroupRpcService
//    {
//        private readonly IAddonGroupSyncAppsAppService _addonGroupSyncAppsAppService;
//        private readonly ILogger<AddonGroupRpcAppService> _logger;

//        public AddonGroupRpcAppService(
//            ILogger<AddonGroupRpcAppService> logger, IAddonGroupSyncAppsAppService addonGroupSyncAppsAppService)
//        {
//            _logger = logger;
//            _addonGroupSyncAppsAppService= addonGroupSyncAppsAppService;
//        }

//        public async Task<ObjectFromMasterCreateResult<AddonGroupMasterCreateRpc>> CreateFromMaster(ObjectFromMasterCreate<AddonGroupMasterCreateRpc> request, CallContext context = default)
//        {
//            var syncResult = await _addonGroupSyncAppsAppService.SyncFromMaster(new SyncMasterRequest
//            {
//                master_id = request.master_id,
//                zaloapp_id = request.link_channel_id,
//                tenant_id = request.tenant_id,
//                is_replace = request.is_replace,
//                user_id = request.user_id,
//                lang_id = request.lang_id,
//            });
//            return new ObjectFromMasterCreateResult<AddonGroupMasterCreateRpc>
//            {
//                result = syncResult.result,
//                master_id = syncResult.master_id,
//                message = syncResult.message,
//            };
//        }
//    }
//}
