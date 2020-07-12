using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace travel_service.Controllers
{   [Route("api/testapi1")]
    [Controller]
    public class ApiController:Controller
    {
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
    }
}
