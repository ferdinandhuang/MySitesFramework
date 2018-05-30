using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Framework.Core.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Auth.Api.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly IConfiguration configuration;
        public ValuesController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
    }
}
