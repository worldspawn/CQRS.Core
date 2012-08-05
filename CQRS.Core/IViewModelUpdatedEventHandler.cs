using CQRS.Core.ViewModel;

namespace CQRS.Core
{
    public interface IViewModelUpdatedEventHandler<TDto> : IEventHandler<ViewModelUpdatedEvent<TDto>>
        where TDto : IDto
    {
        
    }
}