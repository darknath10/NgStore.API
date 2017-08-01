using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using NgStore.API.Entities;
using NgStore.API.Models;
using NgStore.API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NgStore.API.Controllers
{
    [Route("api/[controller]"), EnableCors("AllowNgStore.Client")]
    public class OrdersController : Controller
    {
        private INgStoreRepository _repo;

        public OrdersController(INgStoreRepository repo)
        {
            _repo = repo;
        }

        [HttpPost("new")]
        public IActionResult AddNewOrder([FromBody] OrderPostDto orderDto)
        {
            try
            {
                if (orderDto == null) return BadRequest("A new order cannot be null.");
                if (!ModelState.IsValid) return BadRequest(ModelState);

                orderDto.OrderItems = SetUnitPrices(orderDto.OrderItems);
                var newOrderNumber = Guid.NewGuid().ToString();

                var order = new Order()
                {
                    CustomerId = orderDto.CustomerId,
                    OrderDate = DateTime.Now,
                    OrderNumber = newOrderNumber.Substring(newOrderNumber.Length-7, 7),
                    OrderItems = Mapper.Map<ICollection<OrderItem>>(orderDto.OrderItems),
                    TotalAmount = CalculatePrice(orderDto.OrderItems, orderDto.Discount)
                };

                _repo.addNewOrder(order);

                if (!_repo.Save()) return StatusCode(500, "An error occured while creating the order.");

                return Ok(Mapper.Map<OrderDto>(order));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        private ICollection<OrderItemPostDto> SetUnitPrices(ICollection<OrderItemPostDto> orderItemsDto)
        {
            foreach (var item in orderItemsDto)
            {
                item.UnitPrice = _repo.getProducts().Where(p => p.Id == item.ProductId).FirstOrDefault().UnitPrice;
            }

            return orderItemsDto;
        }

        private decimal? CalculatePrice(ICollection<OrderItemPostDto> orderItems, bool makeDiscount)
        {
            decimal? totalAmount = 0;

            foreach (var item in orderItems)
            {
                totalAmount += (item.UnitPrice * item.Quantity);
            }
            if (makeDiscount)
            {
                totalAmount = totalAmount * 9 / 10;
            }
            return totalAmount;
        }
    }    
}
