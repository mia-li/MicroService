using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MicroService.Model;
namespace MicroService.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new UserContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<UserContext>>()))
            {
                // Look for any Users.
                if (context.user.Any())
                {
                    return;   // DB has been seeded
                }
                //注意，新建user不能设置主键ID的值
                context.user.AddRange(
                    new User
                    {
                        Name="MLP",
                        Account="1007473330",
                        Password="1234",
                        Email= "1007473330@qq.com",
                        Role="hunan",
                        LoginTime= DateTime.Parse("1989-2-12")
                    },

                    new User
                    {
                        Name = "lb",
                        Account = "19973224896",
                        Password = "1234",
                        Email = "19973224896@qq.com",
                        Role = "zhangjiajie",
                        LoginTime = DateTime.Now
                    },

                    new User
                    {
                        Name = "gdd",
                        Account = "15973993287",
                        Password = "1234",
                        Email = "15973993287@qq.com",
                        Role = "nanjing",
                        LoginTime = DateTime.Now
                    },

                    new User
                    {
                        Name = "zzz",
                        Account = "7758266",
                        Password = "1234",
                        Email = "7758266@qq.com",
                        Role = "anhui",
                        LoginTime = DateTime.Now
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
