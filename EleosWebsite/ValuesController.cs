using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EleosWebsite
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpPost("Recordings/handleCompletedRecordings")]
        public void GetAll()
        {
            //return new[]
            //{
            //new Person { Name = "Ana" },
            //new Person { Name = "Felipe" },
            //new Person { Name = "Emillia" }
            //};
        }


    }
    public class Person
    {
        public string Name { get; set; }
    }
}
