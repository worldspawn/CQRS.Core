using System;

namespace CQRS.Core
{
    public interface IViewModelEventDispatcher
    {
        void Change<TDto>(TDto dto, ViewModelUpdateType viewModelUpdateType, Guid commandId);
    }
}