using System;
using System.Collections.Generic;
using System.Text;

namespace BattleShip.Api.Exceptions
{
    public class ValidationAggregateException : AggregateException
    {
        public string TrackId { get; private set; }

        public ValidationAggregateException(IEnumerable<ValidationException> innerExceptions, string trackId) : base(
            innerExceptions)
        {
            TrackId = trackId;
        }
    }
}
