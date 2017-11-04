using Microsoft.EntityFrameworkCore;

namespace customconnector.Models{
    public class Subscription{
        public int ID { get; set; }

        public string ObjectID { get; set; }

        public string WebHookUrl { get; set; }


    }


    public class MyDbContext:DbContext{
        public MyDbContext(DbContextOptions<MyDbContext> options):base(options){}
        public DbSet<Subscription> Subscriptions { get; set; }
    }
}