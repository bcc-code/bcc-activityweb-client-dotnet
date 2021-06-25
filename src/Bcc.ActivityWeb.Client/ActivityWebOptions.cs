namespace Bcc.ActivityWeb.Client
{
    public class ActivityWebOptions
    {
        public string BasePath { get; set; }

        public string SystemUserKey { get; set; }

        public string UserEncryptionIV { get; set; }

        public string UserEncryptionKey { get; set; }

        public int DefaultTeamId { get; set; }

    }
}