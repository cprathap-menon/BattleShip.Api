using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BattleShip.Api.Models
{
    public class EnvironmentVariable
    {
        public string EnvironmentName { get; set; }
        public string SsmSecretPrefix { get; set; }
        public string ContainerId { get; set; }
        public string Culture { get; set; }
        public string GitCommit { get; set; }
        public string TimeZone { get; set; }
    }
}
