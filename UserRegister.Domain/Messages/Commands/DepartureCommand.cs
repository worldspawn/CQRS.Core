using System;
using System.ComponentModel.DataAnnotations;
using CQRS.Core;
using Loveboat.Domain.ViewModels;

namespace Loveboat.Domain.Messages.Commands
{
    public class DepartureCommand : ShipsViewModel, ICommand
    {
        public DepartureCommand()
        {
            CommandId = Guid.NewGuid();
        }

        [Required]
        public Guid DepartingShipId { get; set; }

        public Guid CommandId
        {
            get; set;
        }
    }
}