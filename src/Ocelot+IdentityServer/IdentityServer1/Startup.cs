using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityServer
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //使用内存存储的秘钥、客户端和资源配置id4
            services.AddIdentityServer(x => x.IssuerUri = "http://localhost:8500")
           .AddDeveloperSigningCredential()
            .AddInMemoryApiResources(Config.GetApiResources())
            .AddInMemoryClients(Config.GetClients());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime applifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            using (var client = new ConsulClient((ConsulClientConfiguration obj) =>
            {
                obj.Address = new Uri("http://localhost:8500");
            }))
            {
                var registration = new AgentServiceRegistration()
                {
                    ID = "Identity2",
                    Name = "Identity",
                    Address = "localhost",
                    Port = 5001
                };
                client.Agent.ServiceRegister(registration).Wait();//服务启动时注册，内部实现其实就是使用 Consul API 进行注册（HttpClient发起）              
            }
            app.UseIdentityServer();

            applifetime.ApplicationStopping.Register(() => {
                using (var client = new ConsulClient((ConsulClientConfiguration obj) =>
                {
                    obj.Address = new Uri("http://localhost:8500");
                }))
                {
                    client.Agent.ServiceDeregister("Identity2").Wait();//服务启动时注册，内部实现其实就是使用 Consul API 进行注册（HttpClient发起）
                }
            });
        }
    }
}
