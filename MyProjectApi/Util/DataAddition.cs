using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyProjectApi.Entities;

namespace MyProjectApi.Util
{
    public static class DataAddition
    {
        public static void AddSomeData(this AppDbContext db)
        {
            Brand brand1 = new Brand() { Name = "Brand 1" };
            Brand brand2 = new Brand() { Name = "Brand 2" };
            Brand brand3 = new Brand() { Name = "Brand 3" };
            Brand brand4 = new Brand() { Name = "Brand 4" };
            Brand brand5 = new Brand() { Name = "Brand 5" };


            db.Brand.AddRange(brand1, brand2, brand3, brand4, brand5);
            db.SaveChanges();

            db.Product.AddRange(
               new Product() { BrandId = brand1.Id, Name = "Product1.1", Category = "home", Description = "desc for product 1.1", Price = 101 },
               new Product() { BrandId = brand1.Id, Brand = brand1, Name = "Product1.2", Category = "home", Description = "desc for product 1.1", Price = 213 },
               new Product() { BrandId = brand1.Id, Brand = brand1, Name = "Product1.3", Category = "electronic", Description = "desc for product 1.3", Price = 456 },
               new Product() { BrandId = brand1.Id, Brand = brand1, Name = "Product1.4", Category = "auto", Description = "desc for product 1.4", Price = 32 },
               new Product() { BrandId = brand1.Id, Brand = brand1, Name = "Product1.5", Category = "medical", Description = "desc for product 1.5", Price = 11 },

               new Product() { BrandId = brand2.Id, Brand = brand2, Name = "Product2.1", Category = "clothes", Description = "desc for product 2.1", Price = 131 },
               new Product() { BrandId = brand2.Id, Brand = brand2, Name = "Product2.2", Category = "clothes", Description = "desc for product 2.1", Price = 2413 },
               new Product() { BrandId = brand2.Id, Brand = brand2, Name = "Product2.3", Category = "clothes", Description = "desc for product 2.3", Price = 4536 },
               new Product() { BrandId = brand2.Id, Brand = brand2, Name = "Product2.4", Category = "electronic", Description = "desc for product 2.4", Price = 112 },
               new Product() { BrandId = brand2.Id, Brand = brand2, Name = "Product2.5", Category = "electronic", Description = "desc for product 2.5", Price = 141 },

               new Product() { BrandId = brand3.Id, Brand = brand3, Name = "Product3.1", Category = "furniture", Description = "desc for product 3.1", Price = 111 },
               new Product() { BrandId = brand3.Id, Brand = brand3, Name = "Product3.2", Category = "furniture", Description = "desc for product 3.1", Price = 24243 },
               new Product() { BrandId = brand3.Id, Brand = brand3, Name = "Product3.3", Category = "furniture", Description = "desc for product 3.3", Price = 456 },
               new Product() { BrandId = brand3.Id, Brand = brand3, Name = "Product3.4", Category = "furniture", Description = "desc for product 3.4", Price = 1432 },
               new Product() { BrandId = brand3.Id, Brand = brand3, Name = "Product3.5", Category = "furniture", Description = "desc for product 3.5", Price = 12 },

               new Product() { BrandId = brand4.Id, Brand = brand4, Name = "Product4.1", Category = "electronic", Description = "desc for product 4.1", Price = 100 },
               new Product() { BrandId = brand4.Id, Brand = brand4, Name = "Product4.2", Category = "electronic", Description = "desc for product 4.1", Price = 24 },
               new Product() { BrandId = brand4.Id, Brand = brand4, Name = "Product4.3", Category = "electronic", Description = "desc for product 4.3", Price = 6 },
               new Product() { BrandId = brand4.Id, Brand = brand4, Name = "Product4.4", Category = "electronic", Description = "desc for product 4.4", Price = 222 },
               new Product() { BrandId = brand4.Id, Brand = brand4, Name = "Product4.5", Category = "electronic", Description = "desc for product 4.5", Price = 15 },

               new Product() { BrandId = brand5.Id, Brand = brand5, Name = "Product5.1", Category = "auto", Description = "desc for product 5.1", Price = 100 },
               new Product() { BrandId = brand5.Id, Brand = brand5, Name = "Product5.2", Category = "auto", Description = "desc for product 5.1", Price = 24 },
               new Product() { BrandId = brand5.Id, Brand = brand5, Name = "Product5.3", Category = "home", Description = "desc for product 5.3", Price = 654 },
               new Product() { BrandId = brand5.Id, Brand = brand5, Name = "Product5.4", Category = "home", Description = "desc for product 5.4", Price = 2000 },
               new Product() { BrandId = brand5.Id, Brand = brand5, Name = "Product5.5", Category = "auto", Description = "desc for product 5.5", Price = 150 }
               );

            db.SaveChanges();

        }

    }
}
