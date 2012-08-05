using System;
using System.ComponentModel.DataAnnotations;
using CQRS.Core;

namespace Loveboat.Domain.Messages.Commands
{
    public class ExplodingCommand : Command
    {
        public ExplodingCommand()
        {
            CommandId = Guid.NewGuid();
        }

        [Required]
        public Guid ExplodingShipId { get; set; }
    }
}