using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BattleShip.Api.Filters;
using BattleShip.Api.Models;
using BattleShip.Api.Services;
using Microsoft.AspNetCore.Authorization;

namespace BattleShip.Api.Controllers
{
    [Route("api/v1/game")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IGameService _gameService;

        public GameController(IGameService gameService)
        {
            _gameService = gameService;
        }


        /// <summary>
        /// This is the endpoint that players will call for hitting a co-ordinate within battle-ship board. On a successful hit, this endpoint returns "Hit!" otherwise "Miss!"
        /// </summary>
        /// <param name="x">X Co-ordinate</param>
        /// <param name="y">Y Co-Ordinate</param>
        /// <returns>String Hit /Miss</returns>
        [Route("hit")]
        [HttpPut]
        [AllowAnonymous]
        [ProducesResponseType(typeof(string), 200)]
        public async Task<IActionResult> HitAsync([FromQuery]int x, [FromQuery]int y)
        {
            var result = await _gameService.HitAsync(x, y);
            return Ok(result);
        }


    }
}
