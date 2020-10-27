using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyProjectApi.Util;

namespace MyProjectApi.Controllers
{
    [Route("api")]
    [ApiController]
    public class RootController : ControllerBase
    {
        [HttpGet(Name = "GetRoot")]
        public IActionResult GetRoot()
        {
            var links = new List<LinkDto>();
            links.Add(new LinkDto(Url.Link("GetRoot", null), "self", "GET"));
            links.Add(new LinkDto(Url.Link("GetBrands", null), "brands", "GET"));
            links.Add(new LinkDto(Url.Link("CreateBrand", null), "create_brand", "POST "));

            return Ok(links);
        }
    }
}
