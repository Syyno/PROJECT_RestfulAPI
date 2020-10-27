using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyProjectApi.Entities;
using MyProjectApi.Entities.Dtos;
using MyProjectApi.Models;
using MyProjectApi.Util;

namespace MyProjectApi.Controllers
{
    [ApiController]
    [Route("api/brands/{brandId}/products")]
    public class ProductsController : ControllerBase
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public ProductsController(IRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        #region CRUD

        // URI = api/brands/{brandId}/products/
        [HttpPost(Name = "CreateProductOfBrand")]
        public ActionResult<ProductReadDto> CreateProduct(Guid brandId, ProductCreateDto productCreateDto)
        {
            Brand brand = _repository.GetBrandById(brandId);
            if (brand == null)
            {
                return NotFound();
            }

            Product createdProduct = _mapper.Map<Product>(productCreateDto);
            _repository.AddProduct(brandId, createdProduct);
            _repository.SaveChanges();
            ProductReadDto productReadDto = _mapper.Map<ProductReadDto>(createdProduct);

            return CreatedAtRoute("GetProduct", new { brandId, productId = productReadDto.Id }, productReadDto);
        }

        // URI = api/brands/{brandId}/products
        [HttpGet(Name = "GetProductsOfBrand")]
        public IActionResult GetProductsByBrandId(Guid brandId, [FromQuery] ProductSearchParams productSearchParams)
        {
            Brand brand = _repository.GetBrandById(brandId);
            if (brand == null)
            {
                return NotFound();
            }

            if (productSearchParams == null)
                return Ok(_mapper.Map<IEnumerable<ProductReadDto>>(_repository.GetProductsOfBrand(brandId)));

            var productsOfBrandQuery = _repository.GetProductsOfBrandWithParams(brandId, productSearchParams);
            if (productSearchParams.Fields == null)
                return Ok(_mapper.Map<IEnumerable<ProductReadDto>>(productsOfBrandQuery));

            List<ExpandoObject> list = new List<ExpandoObject>();
            foreach (Product product in productsOfBrandQuery)
            {
                ProductReadDto pr = _mapper.Map<ProductReadDto>(product);
                list.Add(pr.ShapeData(productSearchParams.Fields));
            }

            return Ok(list);
        }

        // URI = api/brands/{brandId}/products/{productId}
        [HttpHead]
        [HttpGet("{productId}", Name = "GetProduct")]
        public IActionResult GetProduct(Guid brandId, Guid productId, [FromQuery] string shape)
        {
            Brand brand = _repository.GetBrandById(brandId);
            if (brand == null)
                return NotFound();

            Product productOfBrand = _repository.GetProductById(productId);
            if (productOfBrand == null)
                return NotFound();
            if (shape != null)
            {
                return Ok(productOfBrand.ShapeData(shape));
            }

            return Ok(_mapper.Map<ProductReadDto>(productOfBrand));
        }

       

        //PUT api/commands/{id}
        [HttpPut("{productId}", Name = "UpdateProduct")]
        public ActionResult UpdateProduct(Guid brandId, Guid productId, ProductUpdateDto productUpdateDto)
        {
            Brand brand = _repository.GetBrandById(brandId);
            if (brand == null)
            {
                return NotFound();
            }

            Product productOfBrand = _repository.GetProductById(productId);
            //put upserting
            if (productOfBrand == null)
            {
                Product product = _mapper.Map<Product>(productUpdateDto);
                product.Id = productId;
                _repository.AddProduct(brandId, product);
                _repository.SaveChanges();

                ProductReadDto productRead = _mapper.Map<ProductReadDto>(product);

                return CreatedAtRoute("GetProduct", new { brandId, productId = productRead.Id }, productRead);
            }

            _mapper.Map(productUpdateDto, productOfBrand);
            _repository.UpdateProduct(productOfBrand);
            _repository.SaveChanges();
            return NoContent();
        }

        // URI = api/brands/{brandId}/products/{productId}
        [HttpPatch("{productId}")]
        public ActionResult PartialProductUpdate(Guid brandId, Guid productId, JsonPatchDocument<ProductUpdateDto> patcher)
        {
            Brand brand = _repository.GetBrandById(brandId);
            if (brand == null)
            {
                return NotFound();
            }

            Product productOfBrand = _repository.GetProductById(productId);
            //patch upserting
            if (productOfBrand == null)
            {
                var newProduct = new ProductUpdateDto();
                patcher.ApplyTo(newProduct, ModelState);
                var product = _mapper.Map<Product>(newProduct);
                product.Id = productId;

                _repository.AddProduct(brandId, product);
                _repository.SaveChanges();

                var productReadDto = _mapper.Map<ProductReadDto>(product);
                return CreatedAtRoute("GetProduct", new { brandId, productId = productReadDto.Id }, productReadDto);
            }

            var productToPatch = _mapper.Map<ProductUpdateDto>(productOfBrand);
            patcher.ApplyTo(productToPatch, ModelState);
            _mapper.Map(productToPatch, productOfBrand);
            _repository.UpdateProduct(productOfBrand);
            _repository.SaveChanges();

            return NoContent();
        }

        // URI = api/brands/{brandId}/products/{productId}
        [HttpDelete("{productId}", Name = "DeleteProduct")]
        public ActionResult DeleteProduct(Guid brandId, Guid productId)
        {
            Brand brand = _repository.GetBrandById(brandId);
            if (brand == null)
            {
                return NotFound();
            }
            Product productOfBrand = _repository.GetProductById(productId);
            if (productOfBrand == null)
            {
                return NotFound();
            }

            _repository.DeleteProduct(productOfBrand);
            _repository.SaveChanges();
            return NoContent();
        }

        #endregion

        #region Options

        // URI = api/brands/{brandId}/products
        [HttpOptions]
        public IActionResult ProductsOptions()
        {
            Response.Headers.Add("Allow", "GET,POST,PATCH(Upsert),PUT(Upsert),OPTIONS");
            return Ok();
        }

        // URI = api/brands/{brandId}/products/{productId}
        [HttpOptions("{productId}")]
        public IActionResult ProductOptions(Guid productId)
        {
            Response.Headers.Add("Allow", "DELETE,PUT,PATCH,OPTIONS");
            return Ok();
        }

        #endregion

    }
}
