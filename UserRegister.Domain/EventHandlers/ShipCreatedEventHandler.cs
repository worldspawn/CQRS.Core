using System.Linq;
using CQRS.Core;
using CQRS.Core.ViewModel;
using UserRegister.Domain.Messages.Events;
using UserRegister.Domain.ViewModels;

namespace UserRegister.Domain.EventHandlers
{
    public class UserCreatedEventHandler : IEventHandler<UserCreatedEvent>
    {
        private readonly IContextDtoRepositoryWrapper<UserViewModel> _repository;

        public UserCreatedEventHandler(IDtoRepository<UserViewModel> repository, IBus bus)
        {
            _repository = new ContextDtoRepositoryWrapper<UserViewModel>(repository, bus);
        }

        #region IEventHandler<ShipCreatedEvent> Members

        public void Handle(UserCreatedEvent @event)
        {
            _repository.SourceId = @event.SourceId;
            var model = _repository.Find(x => x.Id == @event.Id).FirstOrDefault();

            if (model != null)
                return;

            model = new UserViewModel
                                {Id = @event.Id, UserName = @event.UserName, FirstName = @event.FirstName, Surname = @event.Surname, Email = @event.Email};
            _repository.Insert(model);
        }

        #endregion
    }
}