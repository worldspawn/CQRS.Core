using CQRS.Core;
using CQRS.Core.Infrastructure;
using Loveboat.Domain.Aggregates.Ship;
using Loveboat.Domain.Messages.Commands;

namespace Loveboat.Domain.CommandHandlers
{
    public class ExplodedCommandHandler : ICommandHandler<ExplodingCommand>
    {
        private readonly IEventRepository<ShipAggregate> _eventRepository;

        public ExplodedCommandHandler(IEventRepository<ShipAggregate> eventRepository)
        {
            _eventRepository = eventRepository;
        }

        #region ICommandHandler<ArrivalCommand> Members

        public void Handle(ExplodingCommand message)
        {
            ShipAggregate aggregate = _eventRepository.GetById(message.ExplodingShipId);

            aggregate.Explode();

            _eventRepository.Update(aggregate, message.CommandId);
        }

        #endregion
    }
}