
namespace mini_form.shared
{
    public static class MessageTemplateStatusKeyConst
    {
        public const string PARENT_KEY = "message.template.status";
        public const string DRAFT =  $"{PARENT_KEY}.draft";
        public const string PUBLISH =  $"{PARENT_KEY}.publish";
        public const string RUNNING =  $"{PARENT_KEY}.running";
        public const string PAUSE =  $"{PARENT_KEY}.pause";
        public const string COMPLETED =  $"{PARENT_KEY}.completed";
        public const string APPROVED =  $"{PARENT_KEY}.approved";
    }

    public static class MessageLogStatusKeyConst
    {
        public const string PARENT_KEY = "message.log.status";
        public const string DRAFT =  $"{PARENT_KEY}.draft";
        public const string PUBLISH =  $"{PARENT_KEY}.publish";
        public const string RUNNING =  $"{PARENT_KEY}.running";
        public const string PAUSE =  $"{PARENT_KEY}.pause";
        public const string COMPLETED =  $"{PARENT_KEY}.completed";
        public const string APPROVED =  $"{PARENT_KEY}.approved";
    }
}