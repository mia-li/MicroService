using Consul;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroService.ServiceInstance.Utility
{
    public static class ConsulHelper 
    {
       public static void ConsulRegist(this IConfiguration configuration)
        {
            ConsulClient client = new ConsulClient(c =>
              {
                  c.Address = new Uri("http://localhost:8500/");
                  c.Datacenter = "dcl";
              });
            string ip = configuration["ip"];
            int port = int.Parse(configuration["port"]);//命令行参数必须传入
            int weight = string.IsNullOrWhiteSpace(configuration["weight"]) ? 1 :
                int.Parse(configuration["weight"]); //做负载均衡用的权重
            client.Agent.ServiceRegister(new AgentServiceRegistration()
            {
                ID = "service" + Guid.NewGuid(), //唯一的
                Name = "firstService", //组名称-group
                Address = ip, //要注册的ip地址
                Port = port,
                Tags = new string[] { weight.ToString() },
                Check = new AgentServiceCheck() //健康检查
                {
                    Interval = TimeSpan.FromSeconds(12), //多久检查一次
                    HTTP = $"http://{ip}:{port}/Api/Health/Index", //每次检查都是给这个地址发一个请求，看有无响应
                    Timeout = TimeSpan.FromSeconds(5),//多久内没有响应 就要将这个服务实例下线
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(60)//60秒后将此服务用例下线
                }
            });
        }
    }
}
