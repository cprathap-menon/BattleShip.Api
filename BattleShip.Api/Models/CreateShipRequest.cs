using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BattleShip.Api.Models.Enums;

namespace BattleShip.Api.Models
{
    public class CreateShipRequest
    {
        public string ShipName { get; set; }
        [Required]
        [EnumDataType(typeof(Orientation), ErrorMessage = "Invalid value for orientation")]
        public Orientation Orientation { get; set; }
        [Range(1, 10, ErrorMessage = "Invalid size for the ship!")]
        public int Size { get; set; }
        [Range(0, 9, ErrorMessage = "Invalid coordinate value!")]
        public int XCoordinate { get; set; }
        [Range(0, 9, ErrorMessage = "Invalid coordinate value!")]

        public int YCoordinate { get; set; }
    }
}
