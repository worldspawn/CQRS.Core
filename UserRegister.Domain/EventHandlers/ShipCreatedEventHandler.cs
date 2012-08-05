using System.Linq;
using CQRS.Core;
using CQRS.Core.ViewModel;
using Loveboat.Domain.Messages.Events;
using Loveboat.Domain.ViewModels;

namespace Loveboat.Domain.EventHandlers
{
    public class ShipCreatedEventHandler: IEventHandler<ShipCreatedEvent>
    {
        private readonly IContextDtoRepositoryWrapper<ShipViewModel> _shipViewRepository;

        public ShipCreatedEventHandler(IDtoRepository<ShipViewModel> shipViewRepository, IBus bus)
        {
            _shipViewRepository = new ContextDtoRepositoryWrapper<ShipViewModel>(shipViewRepository, bus);
        }

        public void Handle(ShipCreatedEvent @event)
        {
            _shipViewRepository.SourceId = @event.SourceId;
            var shipViewModel = _shipViewRepository.Find(x => x.Id == @event.ShipId).FirstOrDefault();
            
            if (shipViewModel != null)
                return;

            shipViewModel = new ShipViewModel { Id = @event.ShipId, Name = @event.Name, Location = @event.CurrentLocation };
            _shipViewRepository.Insert(shipViewModel);
        }
    }
}