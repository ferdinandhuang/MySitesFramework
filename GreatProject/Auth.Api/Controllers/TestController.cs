using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace Auth.Api.Controllers
{
    public class TestController : Controller
    {
        private readonly IDistributedCache _cache;
        public TestController(IDistributedCache cache)
        {
            _cache = cache;
        }

        [HttpGet]
        public async Task<string> test()
        {
            var token = "";



            return "";
        }
    }
}