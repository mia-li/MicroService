using System;
using System.Collections.Generic;
using MicroService.Interface;
using MicroService.Model;
using MicroService.Data;
using System.Linq;

namespace MicroService.Service
{
    public class UserService : IUserService
    {
        private readonly UserContext _usercontext;
        public UserService(UserContext userContext)
        {
            this._usercontext = userContext;
        }
        public User FindUser(int id)
        {
            var user = _usercontext.user
                .Where(x => x.Id == id)
                .FirstOrDefault();
            return user;
        }

        public IEnumerable<User> UserAll()
        {
            var user = _usercontext.user
                .OrderBy(x => x.Id)
                .ToList();
            return user;
        }

        
    }
}
