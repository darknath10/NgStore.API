using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using NgStore.API.Models;
using NgStore.API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NgStore.API.Controllers
{
    [Route("api/orders/{orderId}/items"), EnableCors("AllowNgStore.Client"), Authorize]
    public class OrderItemsController : Controller
    {
        private INgStoreRepository _repo;

        public OrderItemsController(INgStoreRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public IActionResult GetOrderItems(int orderId)
        {
            try
            {
                var order = _repo.getOrder(orderId);
                if (order == null) return BadRequest("Invalid Order");

                var orderItems = _repo.getOrderItems(orderId);
                return Ok(Mapper.Map<IEnumerable<OrderItemDto>>(orderItems));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
