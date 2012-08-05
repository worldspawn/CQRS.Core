using CQRS.Core;
using CQRS.Core.Infrastructure;
using Loveboat.Domain.Aggregates.Ship;
using Loveboat.Domain.Messages.Commands;

namespace Loveboat.Domain.CommandHandlers
{
    public class DepartureCommandHandler : ICommandHandler<DepartureCommand>
    {
        private readonly IEventRepository<ShipAggregate> _eventRepository;

        public DepartureCommandHandler(IEventRepository<ShipAggregate> eventRepository)
        {
            _eventRepository = eventRepository;
        }

        #region ICommandHandler<DepartureCommand> Members

        public void Handle(DepartureCommand message)
        {
            ShipAggregate aggregate = _eventRepository.GetById(message.DepartingShipId);
            aggregate.Depart();
            _eventRepository.Update(aggregate, message.CommandId);
        }

        #endregion
    }
}