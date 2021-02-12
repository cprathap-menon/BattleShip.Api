using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BattleShip.Api.Models;

namespace BattleShip.Api.Services
{
    public interface IGameService
    {

        Task<string> HitAsync(int x, int y);
    }
    public class GameService : IGameService
    {
        private readonly IBoardService _boardService;
        public GameService(
            IBoardService boardService
            )
        {
            _boardService = boardService;
        }


        public async Task<string> HitAsync(int x, int y)
        {
            var board = await _boardService.GetBoardAsync();

            try
            {
                if (x > board.GetLength(0) && y > board.GetLength(1))
                    throw new Exception("Invalid board location");

                if (board[x, y].IsHit || board[x, y].Name == "-")
                    return "Miss";

                board[x, y].IsHit = true;
                board[x, y].Name = "x";

                return "Hit!";
            }
            catch (Exception e)
            {
                return "Miss!";
            }
        }
    }
}
