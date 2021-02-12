using System;
using System.Collections.Generic;
using System.Text;

namespace BattleShip.Api.Exceptions
{
    public class BadRequestException : Exception
    {
        public string TrackId { get; private set; }

        public BadRequestException() : base()
        {

        }

        public BadRequestException(string message, string trackId = null) : base(message)
        {
            TrackId = trackId;
        }

        public BadRequestException(string message, Exception innerException, string trackId = null) : base(message, innerException)
        {
            TrackId = trackId;
        }
    }
}
