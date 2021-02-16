using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BattleShip.Api.DependencyInjection;
using BattleShip.Api.Exceptions;
using BattleShip.Api.Services;
using Microsoft.Extensions.Caching.Memory;
using NUnit.Framework;

namespace BattleShip.Tests
{
    [TestFixture]
    public class BoardServiceTest
    {
        private readonly IBoardService _boardService;
        public BoardServiceTest()
        {
            _boardService = ServiceLocatorContainer.CurrentLocator.Resolve<IBoardService>();
        }

        // Positive test to check if we can create battleship board
        [Test, Order(1)]
        public async Task CanCreateBoardAsync()
        {
            var boardCreated = await _boardService.CreateBoardAsync();
            Assert.IsTrue(boardCreated);
            _boardService.ClearBoardFromCache();
        }

        [Test, Order(2)]
        public async Task CannotCreateBoardWhenOneExists()
        {
            var boardCreated = await _boardService.CreateBoardAsync();
            if (boardCreated)
            {
                var ex = Assert.ThrowsAsync<BadRequestException>(() => _boardService.CreateBoardAsync());
                Assert.That(ex.Message, Is.EqualTo("Board already exists, If you want to create a new board you will have to hit delete endpoint to clear existing board."));
            }
            _boardService.ClearBoardFromCache();
        }

        [Test, Order(3)]
        public async Task CanGetBoardAsync()
        {
            var boardCreated = await _boardService.CreateBoardAsync();
            if (boardCreated)
            {
                var board = await _boardService.GetBoardAsync();
                Assert.IsNotNull(board);
            }
            _boardService.ClearBoardFromCache();
        }


        [Test, Order(4)]
        public async Task CanClearBoardAsync()
        {
            var boardCreated = await _boardService.CreateBoardAsync();
            if (boardCreated)
            {
                _boardService.ClearBoardFromCache();
                var ex = Assert.ThrowsAsync<BadRequestException>(() => _boardService.GetBoardAsync());
                Assert.That(ex.Message, Is.EqualTo("No board exists, please create a new board!"));
            }
            _boardService.ClearBoardFromCache();

        }
    }
}
