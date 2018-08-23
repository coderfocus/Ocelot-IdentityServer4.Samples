using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer
{
    public class Config
    {
        /// <summary>
        /// 定义资源
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("api1", "My API")
            };
        }

        /// <summary>
        /// 定义客户端
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                 new Client
                 {
                       ClientId = "client",
                       // 没有交互性用户，使用 客户端模式 实现认证。
                       AllowedGrantTypes = GrantTypes.ClientCredentials,
                       // 用于认证的密码
                       ClientSecrets =
                       {
                       new Secret("secret".Sha256())
                       },
                       // 客户端有权访问的范围（Scopes）
                       AllowedScopes = { "api1" }
                }
            };
        }
    }
}
