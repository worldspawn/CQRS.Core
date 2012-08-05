using System.Collections.Generic;
using CQRS.Core.ViewModel;

namespace Loveboat.Domain.ViewModels
{
    public class ShipsViewModel : IDto
    {
        public IEnumerable<ShipViewModel> Ships { get; set; }
    }
}