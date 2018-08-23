using IdentityModel.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;

namespace Client
{
    class Program
    {
        static  void Main(string[] args)
        {
            // 从元数据中发现端口
            var disco =  DiscoveryClient.GetAsync("http://localhost:5000").Result;
            // 请求令牌
            var tokenClient = new TokenClient(disco.TokenEndpoint, "client", "secret");
            var tokenResponse =  tokenClient.RequestClientCredentialsAsync("api1").Result;

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }
            Console.WriteLine(tokenResponse.Json);

            // 调用api
            var client = new HttpClient();
            //设置Token
            client.SetBearerToken(tokenResponse.AccessToken);

            var response = client.GetAsync("http://localhost:5001/identity").Result;
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                var content =  response.Content.ReadAsStringAsync().Result;
                Console.WriteLine(JArray.Parse(content));
            }
        }
    }
}
