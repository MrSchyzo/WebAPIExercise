using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using WebAPIExercise.Output;
using InProduct = WebAPIExercise.Input.Product;

namespace WebAPIExercise.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(ILogger<ProductsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<Product>> Get(
            [FromQuery(Name = "startPage")] int? startPage,
            [FromQuery(Name = "pageSize")] int? pageSize
        )
        {
            int start = Math.Max(0, startPage ?? 0);
            int size = Math.Clamp(pageSize ?? 100, 1, 100);

            return Ok(Enumerable.Range(1, int.MaxValue).Skip(start * size).Take(size).Select(i => new Product
            {
                Id = i,
                Name = $"Product {i}",
                Description = $"This is the product number {i}",
                StockQuantity = (int)((Math.Sin(i) + 1) * 10000),
                UnitPrice = (Math.Cos(i * Math.Sin(i * 300)) + 1) * 100
            }));
        }

        [HttpGet]
        [Route("{id:int:min(1)}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Product> Get(int id)
        {
            if (id % 2 != 0) return NotFound();

            return Ok(
                new Product
                {
                    Id = id,
                    Name = $"Product {id}",
                    Description = $"This is the product number {id}",
                    StockQuantity = (int)((Math.Sin(id) + 1) * 10000),
                    UnitPrice = (Math.Cos(id * Math.Sin(id * 300)) + 1) * 100
                }
            );
        }

        [HttpPost]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Product> New(InProduct toInsert)
        {
            if (toInsert.Name == "E") return BadRequest("Name and Description already exist");

            return Ok(
                new Product
                {
                    Id = (int) (DateTime.Now.Ticks % 1_000_000L),
                    Name = toInsert.Name,
                    Description = toInsert.Description,
                    StockQuantity = toInsert.StockQuantity,
                    UnitPrice = toInsert.StockQuantity
                }
            );
        }
    }
}
