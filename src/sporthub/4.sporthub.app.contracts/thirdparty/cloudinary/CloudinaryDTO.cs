namespace sporthub.app.contracts
{
    public class CloudinaryResponse
    {
        public bool IsSuccess { get; set; }
        public string? PublicId { get; set; }
        public string? Url { get; set; }
        public string? ErrorMessage { get; set; }
    }
}