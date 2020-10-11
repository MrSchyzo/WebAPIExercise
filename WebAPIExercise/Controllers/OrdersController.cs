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
using InOrder = WebAPIExercise.Input.Order;

namespace WebAPIExercise.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IOrderService service;

        public OrdersController(ILogger<ProductsController> logger, IOrderService service)
        {
            _logger = logger;
            this.service = service;
        }

        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Order>>> Get(
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
        public async Task<ActionResult<Order>> Get(int id)
        {
            return Ok(await service.GetByIdAsync(id));
        }

        [HttpPost]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Order>> New(InOrder toInsert)
        {
            return Ok(await service.NewAsync(toInsert));
        }
    }
}
