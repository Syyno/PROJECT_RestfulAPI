using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using MyProjectApi.DTO_s.Brand_DTO_s;
using MyProjectApi.Entities;
using MyProjectApi.Entities.Dtos;
using MyProjectApi.Models;
using MyProjectApi.Util;

namespace MyProjectApi.Controllers
{
    [ApiController]
    [Route("api/brands")]
    public class BrandsController : ControllerBase
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public BrandsController(IRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        #region CRUD 

        // URI = api/brands
        [HttpPost(Name = "CreateBrand")]
        public ActionResult<BrandReadDto> CreateBrand(BrandCreateDto brandCreateDto,
            [FromHeader(Name = "Accept")] string mediaType)
        {
            if (!MediaTypeHeaderValue.TryParse(mediaType, out MediaTypeHeaderValue parsedMediaType))
            {
                return BadRequest();
            }
            Brand createdBrand = _mapper.Map<Brand>(brandCreateDto);
            _repository.AddBrand(createdBrand);
            _repository.SaveChanges();
            BrandReadDto brandReadDto = _mapper.Map<BrandReadDto>(createdBrand);

            if (parsedMediaType.MediaType == "application/vnd.my.hateoas+json")
            {
                var links = CreateLinksForBrand(brandReadDto.Id);
                var smth = brandReadDto.ShapeData(null) as System.Collections.Generic.IDictionary<string, object>;
                smth.Add("links", links);
                return CreatedAtRoute("GetBrandById", new { brandId = smth["Id"] }, smth);
            }
            return Ok(brandReadDto);
        }

        // URI = api/brands
        [HttpGet(Name = "GetBrands")]
        [HttpHead]
        public IActionResult GetBrands([FromHeader(Name = "Accept")] string mediaType)
        {
            if (!MediaTypeHeaderValue.TryParse(mediaType, out MediaTypeHeaderValue parsedMediaType))
            {
                return BadRequest();
            }

            IEnumerable<Brand> brandsFromDb = _repository.GetBrands();
            if (brandsFromDb == null)
                return NotFound();

            if (parsedMediaType.MediaType == "application/vnd.my.hateoas+json")
            {
                var links = CreateLinksForBrands();

                List<ExpandoObject> list = new List<ExpandoObject>();
                foreach (Brand brand in brandsFromDb)
                {
                    var brandAndHisLinks = _mapper.Map<BrandReadDto>(brand).ShapeData(null) as IDictionary<string, object>;
                    var linksForOneBrand = CreateLinksForBrand(brand.Id);
                    brandAndHisLinks.Add("links", linksForOneBrand);
                    list.Add(brandAndHisLinks as ExpandoObject);
                }

                var brandsWithLinksResult = new
                {
                    list,
                    links
                };

                return Ok(brandsWithLinksResult);
            }

            return Ok(_mapper.Map<IEnumerable<BrandReadDto>>(brandsFromDb));
        }

        // URI = api/brands/{brandId}
        [HttpGet("{brandId}", Name = "GetBrandById")]
        public ActionResult<BrandReadDto> GetBrandById(Guid brandId,
            [FromHeader(Name = "Accept")] string mediaType)
        {
            if (!MediaTypeHeaderValue.TryParse(mediaType, out MediaTypeHeaderValue parsedMediaType))
            {
                return BadRequest();
            }

            Brand brandFromDb = _repository.GetBrandById(brandId);
            if (brandFromDb != null)
            {
                if (parsedMediaType.MediaType == "application/vnd.my.hateoas+json")
                {
                    var smth =
                    _mapper.Map<BrandReadDto>(brandFromDb).ShapeData(string.Empty)
                        as IDictionary<string, object>;
                    var link = CreateLinksForBrand(brandId);
                    smth.Add("links", link);
                    return Ok(smth);
                }
                return Ok(_mapper.Map<BrandReadDto>(brandFromDb));
            }
            return NotFound();
        }

        // URI = api/brands/{brandId}
        [HttpPut("{brandId}")]
        public ActionResult UpdateBrand(Guid brandId, BrandUpdateDto brandUpdateDto)
        {
            Brand brandFromRep = _repository.GetBrandById(brandId);
            //put upserting
            if (brandFromRep == null)
            {
                Brand newBrand = _mapper.Map<Brand>(brandUpdateDto);
                newBrand.Id = brandId;
                _repository.AddBrand(newBrand);
                _repository.SaveChanges();

                BrandReadDto newBrandRead = _mapper.Map<BrandReadDto>(newBrand);
                return CreatedAtRoute("GetBrandById", new { brandId }, newBrandRead);
            }
            _mapper.Map(brandUpdateDto, brandFromRep);

            _repository.UpdateBrand(brandFromRep);

            _repository.SaveChanges();

            return NoContent();
        }

        // URI = api/brands/{brandId}
        [HttpPatch("{brandId}")]
        public ActionResult PartialBrandUpdate(Guid brandId, JsonPatchDocument<BrandUpdateDto> patcher)
        {
            Brand brandFromRep = _repository.GetBrandById(brandId);
            //patch upserting
            if (brandFromRep == null)
            {
                var newBrand = new BrandUpdateDto();
                patcher.ApplyTo(newBrand, ModelState);
                var brand = _mapper.Map<Brand>(newBrand);
                brand.Id = brandId;

                _repository.AddBrand(brand);
                _repository.SaveChanges();

                var brandReadDto = _mapper.Map<BrandReadDto>(brand);
                return CreatedAtRoute("GetBrandById", new { brandId }, brandReadDto);
            }

            var brandToPatch = _mapper.Map<BrandUpdateDto>(brandFromRep);
            patcher.ApplyTo(brandToPatch, ModelState);

            _mapper.Map(brandToPatch, brandFromRep);

            _repository.UpdateBrand(brandFromRep);

            _repository.SaveChanges();

            return NoContent();
        }

        // URI = api/brands/{brandId}
        [HttpDelete("{brandId}")]
        public ActionResult DeleteBrand(Guid brandId)
        {
            Brand brandFromRep = _repository.GetBrandById(brandId);
            if (brandFromRep == null)
            {
                return NotFound();
            }
            _repository.DeleteBrand(brandFromRep);
            _repository.SaveChanges();

            return NoContent();
        }

        #endregion

        #region Options and util

        // URI = api/brands
        [HttpOptions]
        public IActionResult BrandsOptions()
        {
            Response.Headers.Add("Allow", "GET,POST,PATCH(Upsert),PUT(Upsert),OPTIONS");
            return Ok();
        }

        // URI = api/brands/{brandId}
        [HttpOptions("{brandId}", Name = "DeleteBrandById")]
        public IActionResult BrandOptions(Guid brandId)
        {
            Response.Headers.Add("Allow", "DELETE,PUT,PATCH,OPTIONS");
            return Ok();
        }

        private IEnumerable<LinkDto> CreateLinksForBrand(Guid brandId)
        {
            var links = new List<LinkDto>();

            links.Add(
                new LinkDto(Url.Link("GetBrandById", new { brandId }),
                    "self",
                    "GET"));

            links.Add(
                new LinkDto(Url.Link("DeleteBrandById", new { brandId }),
                    "delete_brand",
                    "DELETE"));

            links.Add(
                new LinkDto(Url.Link("CreateProductOfBrand", new { brandId }),
                    "create_product_of_brand",
                    "POST"));

            links.Add(
                new LinkDto(Url.Link("GetProductsOfBrand", new { brandId }),
                    "products_of_brand",
                    "GET"));

            return links;
        }

        private IEnumerable<LinkDto> CreateLinksForBrands()
        {
            var links = new List<LinkDto>();
            links.Add(
                new LinkDto(Url.Link("GetBrands", null), "self", "GET"));

            return links;
        }

        #endregion
    }
}
