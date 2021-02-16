using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BattleShip.Api.DependencyInjection;
using BattleShip.Api.Exceptions;
using BattleShip.Api.Models;
using BattleShip.Api.Models.Enums;
using BattleShip.Api.Services;
using Microsoft.AspNetCore.SignalR.Protocol;
using NUnit.Framework;

namespace BattleShip.Tests
{
    [TestFixture]
    public class ShipPlacementTest
    {
        private readonly IBoardService _boardService;
        public ShipPlacementTest()
        {
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
        [TestCaseSource(nameof(InValidShipsTestData))]
        public void CannotPlaceOverlappingShipAsync(
            CreateShipRequest request)
        {
            var ex = Assert.ThrowsAsync<BadRequestException>(() => _boardService.AddShipAsync(request));
            Assert.That(ex.Message, Is.EqualTo("Cannot place ship, invalid position!"));
        }

        [Test, Order(3)]
        [TestCaseSource(nameof(InValidShipCoordinateTestData))]
        public void CannotPlaceShipWithInvalidCoordinates(CreateShipRequest request)
        {
            var ex = Assert.ThrowsAsync<BadRequestException>(() => _boardService.AddShipAsync(request));
            Assert.That(ex.Message, Is.EqualTo("Cannot place ship, invalid position!"));
        }

        #region Test Data
        public static IEnumerable InValidShipCoordinateTestData
        {
            get
            {
                yield return new TestCaseData(new CreateShipRequest() { Orientation = Orientation.Horizontal, ShipName = "S7", Size = 2, XCoordinate = 1, YCoordinate = 9 });
                yield return new TestCaseData(new CreateShipRequest() { Orientation = Orientation.Horizontal, ShipName = "S8", Size = 1, XCoordinate = 2, YCoordinate = 20 });
            }
        }

        public static IEnumerable InValidShipsTestData
        {
            get
            {
                yield return new TestCaseData(new CreateShipRequest() { Orientation = Orientation.Horizontal, ShipName = "S5", Size = 2, XCoordinate = 1, YCoordinate = 0 });
                yield return new TestCaseData(new CreateShipRequest() { Orientation = Orientation.Horizontal, ShipName = "S6", Size = 1, XCoordinate = 2, YCoordinate = 0 });
            }
        }
        public static IEnumerable ValidShipsTestData
        {
            get
            {
                yield return new TestCaseData(new CreateShipRequest() {Orientation = Orientation.Horizontal, ShipName = "S1", Size = 1, XCoordinate = 0, YCoordinate = 0});
                yield return new TestCaseData(new CreateShipRequest() {Orientation = Orientation.Horizontal, ShipName = "S2", Size = 1, XCoordinate = 0, YCoordinate = 1});
                yield return new TestCaseData(new CreateShipRequest() {Orientation = Orientation.Horizontal, ShipName = "S3", Size = 1, XCoordinate = 0, YCoordinate = 2});
                yield return new TestCaseData(new CreateShipRequest() { Orientation = Orientation.Vertical, ShipName = "S4", Size = 1, XCoordinate = 1, YCoordinate = 0 });
                yield return new TestCaseData(new CreateShipRequest() { Orientation = Orientation.Vertical, ShipName = "S4", Size = 1, XCoordinate = 2, YCoordinate = 0 });
                yield return new TestCaseData(new CreateShipRequest() { Orientation = Orientation.Vertical, ShipName = "S4", Size = 1, XCoordinate = 3, YCoordinate = 0 });
            }
        }
        #endregion
    }
}
