using MicroService.ClientDemo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using MicroService.Interface;
using MicroService.Service;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using MicroService.Model;
using Consul;

namespace MicroService.ClientDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserService _userService = null;

        public HomeController(ILogger<HomeController> logger,IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        public IActionResult Index()
        {
            //base.ViewBag.Users=this._userService.UserAll(); 
            // 单体应用直接调用service，而分布式是跨进程，让该进程去调用service；必须跑两个控制台才能运行起来
            //基于httpclient 实现跨进程服务调用--分布式
            //string url = "https://localhost:44311/api/user/all";
            //string url = "https://localhost:44312/api/user/all";
            //string url = "https://localhost:44313/api/user/all";
            //现在有三个服务实例的ip地址，这些地址都由我来管理吗？应该调用哪个？
      
            ConsulClient client = new ConsulClient(c =>
            {
                c.Address = new Uri("http://localhost:8500/");
                c.Datacenter = "dcl";
            });
            var response = client.Agent.Services().Result.Response;//能够得到consul里注册的所有服务实例

            string url = "https://firstService:44311/api/user/all"; //现在我想要调用firstService这个组里的服务实例

            Uri uri = new Uri(url);
            string groupName = uri.Host;

            //在所有服务实例中，找出我们想要的特定某个分组的全部服务实例
            var serviceList = response.Where(k => k.Value.Service.Equals(groupName, StringComparison.OrdinalIgnoreCase)).ToArray();

            //符合条件的全部服务实例找到后，我们到底调用哪个？ --负载均衡 策略
            AgentService agentService = null;

            //平均策略--平均分配--随机就是平均
            agentService = serviceList[new Random().Next(0, serviceList.Length)].Value;
            //权重 and so on...

            url = $"{uri.Scheme}://{agentService.Address}:{agentService.Port}{uri.PathAndQuery}";

            string content = InvokeApi(url);
            base.ViewBag.Users = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<User>>(content);

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        ///
        /// 调用web api
        ///
        public static string InvokeApi(string url)
        {
            using (HttpClient httpClient=new HttpClient())
            {
                HttpRequestMessage message = new HttpRequestMessage();
                message.Method = HttpMethod.Get;
                message.RequestUri = new Uri(url);
                var result = httpClient.SendAsync(message).Result;
                string content = result.Content.ReadAsStringAsync().Result;
                return content;
            }
        }
    }
}
