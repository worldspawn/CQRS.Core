using CQRS.Core.ViewModel;

namespace CQRS.Core
{
    public class ViewModelUpdatedEventHandler<TDto> : IViewModelUpdatedEventHandler<TDto>
        where TDto : IDto
    {
        private readonly IViewModelEventDispatcher _viewModelEventDispatcher;

        public ViewModelUpdatedEventHandler(IViewModelEventDispatcher viewModelEventDispatcher)
        {
            _viewModelEventDispatcher = viewModelEventDispatcher;
        }

        public void Handle(ViewModelUpdatedEvent<TDto> message)
        {
            _viewModelEventDispatcher.Change(message.Dto, message.UpdateType, message.SourceId);
        }
    }
}