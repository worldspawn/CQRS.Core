using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CQRS.Core;

namespace Loveboat.Domain.Messages.Commands
{
    public class ShipCreatedCommand : Command
    {
        public ShipCreatedCommand(string name, string currentLocation)
        {
            Name = name;
            CurrentLocation = currentLocation;
        }

        public string Name { get; set; }
        public string CurrentLocation { get; set; }
    }
}
