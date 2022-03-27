using cachePractice.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cachePractice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        const string cacheKey = "productKey";
        private readonly IMemoryCache _memoryCache;
        public ProductController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        [HttpGet]
        public ActionResult<IEnumerable<Product>> Get()
        {
            List<Product> products = new List<Product>
            {
                new Product{Name="IPhone",Stock=100,Price=14000},
                new Product{Name="Led TV",Stock=743,Price=25000},
                new Product{Name="Washing Machine",Stock=655,Price=5000},
                new Product{Name="Bed",Stock=450,Price=4000}
            };
            if (!_memoryCache.TryGetValue(cacheKey, out products))
            {
                var cacheExpireOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddMinutes(10),
                    Priority = CacheItemPriority.Normal
                };
                _memoryCache.Set(cacheKey, products, cacheExpireOptions);
            }
            return products;
        }
        [HttpGet]
        public ActionResult DeleteCache()
        {
            _memoryCache.Remove(cacheKey);
            return View();
        }
    }
}
