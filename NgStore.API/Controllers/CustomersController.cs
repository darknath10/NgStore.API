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
    [Route("api/[controller]"), EnableCors("AllowNgStore.Client"), Authorize]
    public class CustomersController : Controller
    {
        private INgStoreRepository _repo;

        public CustomersController(INgStoreRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public IActionResult GetCustomers()
        {
            try
            {
                var customers = _repo.getCustomers();
                return Ok(Mapper.Map<IEnumerable<CustomerDto>>(customers));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{customerId}/orders")]
        public IActionResult GetCustomerOrders(int customerId)
        {
            try
            {
                var customer = _repo.getCustomer(customerId);
                if (customer == null) return BadRequest("This Customer doesn't exists");

                var orders = _repo.getCustomerOrders(customerId);
                return Ok(Mapper.Map<IEnumerable<OrderDto>>(orders));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
