using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using WebAPIExercise.Output;
using WebAPIExercise.Services;
using InProduct = WebAPIExercise.Input.Product;

namespace WebAPIExercise.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IProductService service;

        public ProductsController(ILogger<ProductsController> logger, IProductService service)
        {
            _logger = logger;
            this.service = service;
        }

        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Product>>> Get(
            [FromQuery(Name = "pageStart")] int? startPage,
            [FromQuery(Name = "pageSize")] int? pageSize
        )
        {
            int start = Math.Max(0, startPage ?? 0);
            int size = Math.Clamp(pageSize ?? 100, 1, 100);

            return Ok(await service.GetAllPagedAsync(start, size));
        }

        [HttpGet]
        [Route("{id:int:min(1)}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Product>> Get(int id)
        {
            return Ok(await service.GetByIdAsync(id));
        }

        [HttpPost]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<Product> New(InProduct toInsert)
        {
            return await service.NewAsync(toInsert);
        }
    }
}
