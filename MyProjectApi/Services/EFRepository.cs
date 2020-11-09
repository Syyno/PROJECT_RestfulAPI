using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using MyProjectApi.Models;

namespace MyProjectApi.Entities
{
    public class EFRepository : IRepository
    {
        private readonly AppDbContext _db;

        public EFRepository(AppDbContext db)
        {
            _db = db;
        }

        #region CRUD for brand entity
        public void AddBrand(Brand brand)
        {
            if (brand != null)
            {
                if (brand.Id == null) //if upserted
                    brand.Id = Guid.NewGuid();
                _db.Brand.Add(brand);
                foreach (Product brandProduct in brand.Products)
                {
                    brandProduct.Id = Guid.NewGuid();
                }
            }
            else
                throw new ArgumentNullException(nameof(brand));
        }

        public IEnumerable<Brand> GetBrands() => _db.Brand.ToList().OrderBy(b => b.Name);

        public Brand GetBrandById(Guid id) => _db.Brand.FirstOrDefault(b => b.Id == id);


        public void UpdateBrand(Brand brand)
        {

        }

        public void DeleteBrand(Brand brand)
        {
            if (brand != null)
                _db.Brand.Remove(brand);
            else
                throw new ArgumentNullException(nameof(brand));
        }
        #endregion

        #region CRUD for product entity
        public void AddProduct(Guid brandId, Product product)
        {
            if (brandId == null)
                throw new ArgumentNullException(nameof(brandId));
            if (product == null)
                throw new ArgumentNullException(nameof(product));
            product.BrandId = brandId;
            _db.Product.Add(product);
        }

        public IEnumerable<Product> GetProductsOfBrand(Guid brandId)
        {
            Brand brand = _db.Brand.FirstOrDefault(b => b.Id == brandId);
            if (brand == null)
                throw new ArgumentNullException(nameof(brand));
            return _db.Product.Where(b => b.BrandId == brandId).OrderBy(p => p.Name).ToList();
        }

        public IEnumerable<Product> GetProductsOfBrandWithParams(Guid brandId, ProductSearchParams productSearchParams)
        {
            Brand brand = _db.Brand.FirstOrDefault(b => b.Id == brandId);
            if (brand == null)
            {
                throw new ArgumentNullException(nameof(brand));
            }

            if (productSearchParams == null)
            {
                throw new ArgumentNullException(nameof(productSearchParams));
            }

            IQueryable<Product> productsToReturn = _db.Product.Where(p => p.BrandId == brandId);

            if (string.IsNullOrWhiteSpace(productSearchParams.Category)
                && string.IsNullOrWhiteSpace(productSearchParams.SearchQuery) && string.IsNullOrWhiteSpace(productSearchParams.OrderBy))
            {
                return productsToReturn.ToList();
            }


            if (!string.IsNullOrWhiteSpace(productSearchParams.Category))
            {
                productsToReturn = productsToReturn.Where(p =>
                    p.Category.ToLower().Trim() == productSearchParams.Category.ToLower().Trim());
            }

            if (!string.IsNullOrWhiteSpace(productSearchParams.SearchQuery))
            {
                string query = productSearchParams.SearchQuery.Trim();
                productsToReturn = productsToReturn.Where(p =>
                    p.Category.Contains(query) || p.Description.Contains(query) || p.Name.Contains(query));
            }

            if (!string.IsNullOrWhiteSpace(productSearchParams.OrderBy))
            {
                //productsToReturn = productsToReturn.Sort(productSearchParams.OrderBy);
                string orderType = " ascending";
                string[] query = productSearchParams.OrderBy.Split(',');
                if (query.Length > 1)
                {
                    switch (query[1].ToLower().Trim())
                    {
                        case "desc":
                            orderType = " descending";
                            break;
                    }
                }

                productsToReturn = productsToReturn.OrderBy(query[0].Trim() + orderType);
            }

            return productsToReturn.ToList();
        }


        public Product GetProductById(Guid id) => _db.Product.FirstOrDefault(p => p.Id == id);


        public void UpdateProduct(Product product)
        {

        }

        public void DeleteProduct(Product product)
        {
            if (product != null)
                _db.Product.Remove(product);
            else
                throw new ArgumentNullException(nameof(product));
        }
        #endregion


        public void SaveChanges()
        {
            _db.SaveChanges();
        }

    }
}
