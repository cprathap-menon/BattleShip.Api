using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;

namespace BattleShip.Api.Exceptions
{
    public interface IExceptionBuilder
    {

        (int Status, Exception Error) BuildAndLogException(Exception exp, ILogger logger, string trackId);
    }
}
