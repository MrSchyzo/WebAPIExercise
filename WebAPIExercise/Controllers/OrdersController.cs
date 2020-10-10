using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using WebAPIExercise.Output;
using InOrder = WebAPIExercise.Input.Order;

namespace WebAPIExercise.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;

        public OrdersController(ILogger<ProductsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<OrderSummary>> Get(
            [FromQuery(Name = "startPage")] int? startPage,
            [FromQuery(Name = "pageSize")] int? pageSize
        )
        {
            int start = Math.Max(0, startPage ?? 0);
            int size = Math.Clamp(pageSize ?? 100, 1, 100);

            return Ok(Enumerable.Range(1, int.MaxValue).Skip(start * size).Take(size).Select(i => new OrderSummary
            {
                Id = i,
                CompanyCode = $"COMPANY_{i%2}",
                Date = DateTime.Now.AddDays(-i),
                Total = i * 1.2 + 750 * (Math.Sin(i) + 1)
            }));
        }

        [HttpGet]
        [Route("{id:int:min(1)}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Order> Get(int id)
        {
            if (id % 2 != 0) return NotFound();

            return Ok(
                new Order
                {
                    Id = id,
                    CompanyCode = $"COMPANY_{id % 2}",
                    Date = DateTime.Now.AddDays(-id),
                    Total = id * 1.2 + 750 * (Math.Sin(id) + 1),
                    Items = Enumerable.Range(0, id % 10).Select(j => new ProductItem
                    {
                        Id = j,
                        OrderedQuantity = (int)(10 * (Math.Sin(j) + 1)) + 1,
                        UnitPrice = 100 * (Math.Sin(j) + 1)
                    })
                }
            );
        }

        [HttpPost]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<OrderSummary> New(InOrder toInsert)
        {
            return Ok(
                new OrderSummary
                {
                    Id = (int)(DateTime.Now.Ticks % 1_000_000L),
                    CompanyCode = $"COMPANY_1",
                    Date = DateTime.Now,
                    Total = toInsert.Items.Sum(item => item.OrderedQuantity * (100 * (Math.Sin(item.Id) + 1.01)))
                }
            );
        }
    }
}
