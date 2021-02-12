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

    [Route("api/v1")]
    [ApiController]
    public class BoardController : ControllerBase
    {
        private readonly IBoardService _boardService;

        public BoardController(IBoardService boardService)
        {
            _boardService = boardService;
        }



        /// <summary>
        /// Creates and initialise battleship 10x10 game board, Each tile on the board has a name and by default it is set to "-" meaning unoccupied
        /// Once a ship is placed on a tile, tile name will become ship name, once tile takes a hit tile name changes to x
        /// </summary>
        [HttpPost]
        [Route("board")]
        [ValidateModelFilter]
        [AllowAnonymous]
        [ProducesResponseType(typeof(string), 200)]
        public async Task<IActionResult> CreateBoardAsync()
        {
            var response = await _boardService.CreateBoardAsync();
            var responseMessage = response ? "Board created!" : "Board creation failed!";
            return Ok(responseMessage);
        }


        /// <summary>
        /// Place a new ship on the board, you need to pass x/y coordinates and the orientation in which ship will be placed vertical/horizontal.
        /// You need to also pass in the size of the ship which would determine the number of tiles the ship will occupy on the board.
        /// </summary>
        /// <param name="request">CreateShipRequest</param>
        /// <returns>true/false</returns>
        [Route("board/ships")]
        [ValidateModelFilter]
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> AddShipsAsync([FromBody]CreateShipRequest request)
        {
            var response = await _boardService.AddShipAsync(request);
            return Ok(response);
        }


        /// <summary>
        /// Clears existing board from cache.
        /// </summary>
        /// <returns></returns>
        [Route("board")]
        [HttpDelete]
        [AllowAnonymous]
        [ProducesResponseType(typeof(bool), 200)]
        public IActionResult Delete()
        {
            _boardService.ClearBoardFromCache();
            return Ok(true);
        }
    }
}
