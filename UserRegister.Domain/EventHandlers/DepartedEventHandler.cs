using CQRS.Core;
using CQRS.Core.ViewModel;
using Loveboat.Domain.Messages.Events;
using Loveboat.Domain.ViewModels;

namespace Loveboat.Domain.EventHandlers
{
    public class DepartedEventHandler : IEventHandler<DepartedEvent>
    {
        private readonly IContextDtoRepositoryWrapper<ShipViewModel> _shipViewRepository;

        public DepartedEventHandler(IDtoRepository<ShipViewModel> shipViewRepository, IBus bus)
        {
            _shipViewRepository = new ContextDtoRepositoryWrapper<ShipViewModel>(shipViewRepository, bus);
        }

        public void Handle(DepartedEvent @event)
        {
            _shipViewRepository.SourceId = @event.SourceId;
            var shipViewModel = _shipViewRepository.Single(x => x.Id == @event.ShipId);
            
            if (shipViewModel == null) return;

            shipViewModel.Location = "At Sea";
            _shipViewRepository.Update(shipViewModel);
        }
    }
}