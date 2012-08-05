using CQRS.Core;
using CQRS.Core.ViewModel;
using Loveboat.Domain.Messages.Events;
using Loveboat.Domain.ViewModels;

namespace Loveboat.Domain.EventHandlers
{
    public class ExplodedEventHandler : IEventHandler<ExplodedEvent>
    {
        private readonly IContextDtoRepositoryWrapper<ShipViewModel> _shipViewRepository;

        public ExplodedEventHandler(IDtoRepository<ShipViewModel> shipViewRepository, IBus bus)
        {
            _shipViewRepository = new ContextDtoRepositoryWrapper<ShipViewModel>(shipViewRepository, bus);
        }

        public void Handle(ExplodedEvent @event)
        {
            _shipViewRepository.SourceId = @event.SourceId;
            var shipViewModel = _shipViewRepository.Single(x => x.Id == @event.Id);
            
            if (shipViewModel == null) return;

            _shipViewRepository.Delete(shipViewModel);
        }
    }
}