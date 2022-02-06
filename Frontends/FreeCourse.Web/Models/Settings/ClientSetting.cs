namespace FreeCourse.Web.Models.Settings
{
    public class ClientSetting
    {
        public Client WebClient { get; set; }
        public Client WebClientForUser { get; set; }
    }
    public class Client
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
}


