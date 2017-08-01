using AutoMapper;
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
    [Route("api/[controller]"), EnableCors("AllowNgStore.Client")]
    public class ProductsController : Controller
    {
        private INgStoreRepository _repo;

        public ProductsController(INgStoreRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public IActionResult GetProducts()
        {
            try
            {
                var products = _repo.getProducts();
                return Ok(Mapper.Map<IEnumerable<ProductDto>>(products));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
