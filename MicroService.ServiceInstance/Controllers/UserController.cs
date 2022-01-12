using MicroService.Interface;
using MicroService.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroService.ServiceInstance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService = null;
        private readonly IConfiguration _configuration;
        public UserController(ILogger<UserController> logger, IUserService userService,IConfiguration configuration)
        {
            _logger = logger;
            _userService = userService;
            _configuration = configuration;
        }
        

        [HttpGet]
        [Route("All")]
        public IEnumerable<User> Get()
        {
            Console.WriteLine($"This is UserController {this._configuration["port"]} Invoke");
            return _userService.UserAll();
        }

        [HttpGet]
        [Route("Get")]
        public User Get(int id)
        {
            return _userService.FindUser(id);
        }
    }
}
