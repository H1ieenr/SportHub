
namespace mini_form.shared
{
    public static class CampaignStatusKeyConst
    {
        public const string PARENT_KEY = "campaign.status";
        public const string DRAFT =  $"{PARENT_KEY}.draft";
        public const string PUBLISH =  $"{PARENT_KEY}.publish";
        public const string RUNNING =  $"{PARENT_KEY}.running";
        public const string PAUSE =  $"{PARENT_KEY}.pause";
        public const string COMPLETED =  $"{PARENT_KEY}.completed";
    }
}