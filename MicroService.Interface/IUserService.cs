﻿using System;
using System.Collections.Generic;
using MicroService.Model;
namespace MicroService.Interface
{
    public interface IUserService
    {
        User FindUser(int id);

        IEnumerable<User> UserAll();
    }
}
