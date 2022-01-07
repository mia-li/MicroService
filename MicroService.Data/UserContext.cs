using Microsoft.EntityFrameworkCore;
using MicroService.Model;
using System;
using Microsoft.Extensions.Logging;

namespace MicroService.Data
{
    public class UserContext:DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {
        }

        public DbSet<User> user { get; set; }


        ///// <summary>
        ///// 连接数据库
        ///// </summary>
        ///// <param name="optionsBuilder"></param>
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseLoggerFactory(ConsoleLoggerFactory)
        //        .UseSqlServer(
        //       connectionString: "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MicroService;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;");
        //}

        //用来打印具体操作的sql语句的
        public static readonly ILoggerFactory ConsoleLoggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddFilter((category, level) =>
            category == DbLoggerCategory.Database.Command.Name
            && level == LogLevel.Information)
            .AddConsole();
        });

        //protected override void OnModelCreating(ModelBuilder builder)
        //{
        //    base.OnModelCreating(builder);
        //    builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        //}
    }
}
