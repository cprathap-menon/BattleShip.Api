using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BattleShip.Api.Models
{
    public class Tile
    {
        public string Name { get; set; }

        public bool IsHit { get; set; }

        public Tile()
        {
            Name = "-";
        }
    }
}
