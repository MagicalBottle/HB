using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HB.Admin.Services;
using HB.Data;
using HB.Models;
using Microsoft.AspNetCore.Mvc;

namespace HB.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ISysAdminService _sysAdminService;
        public ValuesController(ISysAdminService sysAdminService)
        {
            _sysAdminService = sysAdminService;
        }
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            _sysAdminService.UpdateAdmin(new SysAdmin(), new List<int>());
            var model = _sysAdminService.Get(1);
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
