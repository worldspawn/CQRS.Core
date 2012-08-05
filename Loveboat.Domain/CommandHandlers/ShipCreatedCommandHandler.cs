using CQRS.Core;
using CQRS.Core.Infrastructure;
using Loveboat.Domain.Aggregates.Ship;
using Loveboat.Domain.Messages.Commands;

namespace Loveboat.Domain.CommandHandlers
{
    public class ShipCreatedCommandHandler : ICommandHandler<ShipCreatedCommand>
    {
        private readonly IEventRepository<ShipAggregate> _eventRepository;

        public ShipCreatedCommandHandler(IEventRepository<ShipAggregate> eventRepository)
        {
            _eventRepository = eventRepository;
        }

        #region ICommandHandler<DepartureCommand> Members

        public void Handle(ShipCreatedCommand message)
        {
            ShipAggregate aggregate = ShipAggregate.Create(message.Name, message.CurrentLocation);
            _eventRepository.Create(aggregate, message.CommandId);
        }

        #endregion
    }
}