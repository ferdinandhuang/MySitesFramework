using System;
using System.Collections.Generic;
using Framework.WebApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySites.IRepositories.Interface;
using MySites.IServices;

namespace MySites.Website.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : ApiBasicController
    {
        private IClass1 class1;
        public ValuesController(IClass1 _class)
        {
            class1 = _class;
        }
        // GET api/values
        [AllowAnonymous]
        [HttpGet]
        public IEnumerable<string> Get()
        {
            class1.Test();
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
