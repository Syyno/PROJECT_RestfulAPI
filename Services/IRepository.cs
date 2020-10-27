using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyProjectApi.Models;

namespace MyProjectApi.Entities
{
    public interface IRepository
    {
        //CRUD for brand entity
        void AddBrand(Brand brand);
        IEnumerable<Brand> GetBrands();
        Brand GetBrandById(Guid brandId);
        void UpdateBrand(Brand brand);
        void DeleteBrand(Brand brand);

        //CRUD for product entity
        void AddProduct(Guid brandId, Product product);
        IEnumerable<Product> GetProductsOfBrandWithParams(Guid brandId, ProductSearchParams productSearchParams);
        IEnumerable<Product> GetProductsOfBrand(Guid brandId);
        Product GetProductById(Guid productId);
        void UpdateProduct(Product product);
        void DeleteProduct(Product product);

        void SaveChanges();
    }
}
