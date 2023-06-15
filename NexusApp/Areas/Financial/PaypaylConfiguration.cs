using PayPal.Api;

namespace NexusApp.Areas.Financial
{
    public static class PaypaylConfiguration
    {
        static PaypaylConfiguration()
        { }
        public static Dictionary<string, string> GetConfig(string mode)
        {
            return new Dictionary<string, string>()
            {
                {"mode",mode }
            };
        }
        private static string GetAccessToken(string ClientID, string ClientSecret, string mode)
        {
            string accessToken = new OAuthTokenCredential(ClientID, ClientSecret, new Dictionary<string, string>()
            {
                {"mode",mode }
            }).GetAccessToken();
            return accessToken;
        }
        public static APIContext GetAPIContext(string ClientID, string ClientSecret, string mode)
        {
            APIContext apiContext = new APIContext(GetAccessToken(ClientID, ClientSecret, mode));
            apiContext.Config = GetConfig(mode);
            return apiContext;
        }
    }
}
