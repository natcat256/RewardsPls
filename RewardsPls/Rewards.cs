using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using Newtonsoft.Json.Linq;

namespace RewardsPls
{
    internal static class Rewards
    {
        public static class UserAgent
        {
            public const string Firefox = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:109.0) Gecko/20100101 Firefox/110.0";
            public const string Edge = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/52.0.2743.116 Safari/537.36 Edge/15.15063";
            public const string Android = "Mozilla/5.0 (Linux; Android 12) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/103.0.5060.71 Mobile Safari/537.36";
        }

        public static JObject ReportActivity(WebClient webClient, string epuid, string searchQuery, string userAgent = UserAgent.Firefox)
        {
            string searchUrl = "https://www.bing.com/search?q=" + searchQuery + "&V=web";

            webClient.Headers["Cookie"] = "_RwBf=e=" + epuid;
            webClient.Headers["User-Agent"] = userAgent;
            webClient.Headers["Content-Type"] = "application/x-www-form-urlencoded";

            string response = webClient.UploadString("https://www.bing.com/rewardsapp/reportActivity", 
                "url=" + WebUtility.UrlEncode(searchUrl));

            const int prologueLength = 151;
            const int epilogueLength = 61;

            if (response.Length <= (prologueLength + epilogueLength))
                throw new Exception("Invalid response");

            string json = response.Remove(response.Length - epilogueLength)
                .Remove(0, prologueLength);

            return JObject.Parse(json);
        }
    }
}
