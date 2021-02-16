using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BattleShip.Api.DependencyInjection;
using BattleShip.Api.Models;
using BattleShip.Api.Models.Enums;
using BattleShip.Api.Services;
using NUnit.Framework;

namespace BattleShip.Tests
{
    [TestFixture]
    public class GameServiceTest
    {
        private readonly IGameService _gameService;
        private readonly IBoardService _boardService;

        public GameServiceTest()
        {
            _gameService = ServiceLocatorContainer.CurrentLocator.Resolve<IGameService>();
            _boardService = ServiceLocatorContainer.CurrentLocator.Resolve<IBoardService>();
            _boardService.ClearBoardFromCache();
            _boardService.CreateBoardAsync();
        }

        [Test, Order(1)]
        [TestCaseSource(nameof(ValidShipsTestData))]
        public async Task CanPlaceShipAsync(
            CreateShipRequest request)
        {
            var shipCreated = await _boardService.AddShipAsync(request);
            Assert.IsTrue(shipCreated);
        }


        [Test, Order(2)]
        public async Task CanHitValidCoordinate()
        {
            var hit1Response = await _gameService.HitAsync(0, 0);
            Assert.AreEqual(hit1Response, "Hit!");

            var hit2Response = await _gameService.HitAsync(1, 0);
            Assert.AreEqual(hit2Response, "Hit!");

            var hit3Response = await _gameService.HitAsync(2, 0);
            Assert.AreEqual(hit3Response, "Hit!");
        }

        [Test, Order(3)]
        public async Task CannotHitInvalidValidCoordinate()
        {
            var miss1Response = await _gameService.HitAsync(0, 0);
            Assert.AreEqual(miss1Response, "Miss!");

            var miss2Response = await _gameService.HitAsync(1, 0);
            Assert.AreEqual(miss2Response, "Miss!");

            var miss3Response = await _gameService.HitAsync(2, 0);
            Assert.AreEqual(miss3Response, "Miss!");
        }


        public static IEnumerable ValidShipsTestData
        {
            get
            {
                yield return new TestCaseData(new CreateShipRequest()
                {
                    Orientation = Orientation.Horizontal, ShipName = "S1", Size = 1, XCoordinate = 0, YCoordinate = 0
                });
                yield return new TestCaseData(new CreateShipRequest()
                {
                    Orientation = Orientation.Horizontal, ShipName = "S2", Size = 1, XCoordinate = 1, YCoordinate = 0
                });
                yield return new TestCaseData(new CreateShipRequest()
                {
                    Orientation = Orientation.Horizontal, ShipName = "S3", Size = 2, XCoordinate = 2, YCoordinate = 0
                });
            }
        }
    }

}