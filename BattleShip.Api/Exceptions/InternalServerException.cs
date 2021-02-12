using System;
using System.Collections.Generic;
using System.Text;

namespace BattleShip.Api.Exceptions
{
    public class InternalServerErrorException : Exception
    {
        public string TrackId { get; private set; }

        public InternalServerErrorException(string message, string trackId = null) : base($"{message} trackId:{trackId}")
        {
            TrackId = trackId;
        }

        public InternalServerErrorException(string message, Exception inner, string trackId = null) : base($"{message} trackId:{trackId}", inner)
        {
            TrackId = trackId;
        }
    }
}
