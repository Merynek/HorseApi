using System.Collections.Generic;
using HorsiApi.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace HorseApi.Controllers
{
    [Route("api/values")]
    [ApiController]
    public class ValuesController : BaseApiController
    {
        private readonly IConfiguration _configuration;

        public ValuesController(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }


        [HttpGet]
        [Route("admin")]
        [Authorize(Roles = "admin")]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "adminValue", "adminValue2" };
        }

        [HttpGet]
        [Route("noadmin")]
        [Authorize]
        public ActionResult<IEnumerable<string>> GetXX()
        {
            return new string[] { "authValue", "authValue2" };
        }


        [HttpGet]
        [Route("free")]
        public ActionResult<IEnumerable<string>> GetId()
        {
            return new string[] { "freeValue", "freeValue" };
        }
    }
}
