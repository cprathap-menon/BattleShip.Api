using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;

namespace BattleShip.Api.Controllers
{
    [Route("api")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        /// <summary>
        /// Used for Health check
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("healthcheck")]
        [SwaggerOperation(Tags = new string[] { "Infrastructure" })]
        [ProducesResponseType(200)]
        public IActionResult HealthCheck()
        {
            return Ok();
        }
    }
}
