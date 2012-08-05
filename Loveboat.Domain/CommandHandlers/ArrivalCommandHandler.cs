using CQRS.Core;
using CQRS.Core.Infrastructure;
using Loveboat.Domain.Aggregates.Ship;
using Loveboat.Domain.Messages.Commands;

namespace Loveboat.Domain.CommandHandlers
{
    public class ArrivalCommandHandler : ICommandHandler<ArrivalCommand>
    {
        private readonly IEventRepository<ShipAggregate> _eventRepository;

        public ArrivalCommandHandler(IEventRepository<ShipAggregate> eventRepository)
        {
            _eventRepository = eventRepository;
        }

        #region ICommandHandler<ArrivalCommand> Members

        public void Handle(ArrivalCommand message)
        {
            ShipAggregate aggregate = _eventRepository.GetById(message.ArrivingShipId);

            aggregate.Arrive(message.ArrivalPort);

            _eventRepository.Update(aggregate, message.CommandId);
        }

        #endregion
    }
}