using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace QosApi.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private static int _count = 0;
        [HttpGet]
        public string Get()
        {
            _count++;
            if (_count <= 3)
            {
                System.Threading.Thread.Sleep(5000);
            }
            return "ok!";
        }
    }
}
