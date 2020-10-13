using System;
using System.Collections.Generic;
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
    /// <summary>
    /// Orders API controller
    /// </summary>
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

        /// <summary>
        /// GET /orders?pageStart=x&pageSize=y
        /// </summary>
        /// <param name="pageStart">0-based index of the page, defaults to 0</param>
        /// <param name="pageSize">Page size, defaults to 100, between 1 and 100</param>
        /// <returns>An ActionResult containing the Orders</returns>
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Order>>> Get(
            [FromQuery(Name = "pageStart")] int? pageStart,
            [FromQuery(Name = "pageSize")] int? pageSize
        )
        {
            int start = Math.Max(0, pageStart ?? 0);
            int size = Math.Clamp(pageSize ?? 100, 1, 100);

            return Ok(await service.GetAllPagedAsync(start, size));
        }

        /// <summary>
        /// GET /orders/ID
        /// </summary>
        /// <param name="id">Order ID</param>
        /// <returns>An ActionResult containing the Order</returns>
        [HttpGet]
        [Route("{id:int:min(1)}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Order>> Get(int id)
        {
            return Ok(await service.GetByIdAsync(id));
        }

        /// <summary>
        /// POST /orders
        /// </summary>
        /// <param name="toInsert">Order to create</param>
        /// <returns>An ActionResult containing the newly created Order</returns>
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
