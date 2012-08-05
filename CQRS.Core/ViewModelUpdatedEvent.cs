using System;
using CQRS.Core.ViewModel;

namespace CQRS.Core
{
    public class ViewModelUpdatedEvent<TDto> : IViewModelUpdatedEvent<TDto>
        where TDto : IDto
    {
        public Guid SourceId { get; set; }
        public ViewModelUpdateType UpdateType { get; set; }
        public TDto Dto { get; set; }
    }
}