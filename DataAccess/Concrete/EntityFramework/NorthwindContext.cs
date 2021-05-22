using Core.Entities.Concrete;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Concrete.EntityFramework
{
    // bu class DB tabloları ile proje classlarını bağlayacak
    public class NorthwindContext : DbContext
    {
        //override OnConf.. yazıp enterladık bu geldi. veri tabanının tam olarak nerede olduğunu gösteriyor burası.
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //base.OnConfiguring(optionsBuilder);bunu sildik.

            //optionsBuilder.UseSqlServer(@"Server=175.45.2.12"); normal bi projede bu şekilde yapılır. sql serverin nerede olduğu anlatılır.

            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Northwind;Trusted_Connection=true");
            //trusted kısmı kullanıcı adı şifre gerektirmeden girmeye yarıyor. geri kalan tamamen db ' nin nerede olduğuna dair.
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        //engin demiroğ githubtan çekildi.
        public DbSet<OperationClaim> OperationClaims { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserOperationClaim> UserOperationClaims { get; set; }
    }
    
}
