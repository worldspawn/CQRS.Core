using System;
using CQRS.Core;
using SignalR;

namespace Loveboat.Hubs
{
    public class ViewModelEventDispatcher : IViewModelEventDispatcher
    {
        private readonly IConnectionManager _connectionManager;

        public ViewModelEventDispatcher(IConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
        }

        public void Change<TDto>(TDto dto, ViewModelUpdateType viewModelUpdateType, Guid sourceId)
        {
            var context = _connectionManager.GetHubContext<ViewModelDispatcher>();
            context.Clients["vm_" + typeof(TDto).Name].viewModelChanged(typeof(TDto).Name, dto, viewModelUpdateType.ToString(), sourceId);
        }
    }
}