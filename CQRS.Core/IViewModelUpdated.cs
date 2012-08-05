using CQRS.Core.ViewModel;

namespace CQRS.Core
{
    public interface IViewModelUpdatedEvent<TDto> : IEvent
        where TDto : IDto
    {
        ViewModelUpdateType UpdateType { get; set; }
        TDto Dto { get; set; }
    }
}