using System;
using CQRS.Core.ViewModel;

namespace Loveboat.Domain.ViewModels
{
    [Serializable]
    public class ShipViewModel : IPersistedDto
    {
        public Guid Id { get; set; }
        public string Location { get; set; }
        public string Name { get; set; }
        public int PortsVisited { get; set; }
    }
}