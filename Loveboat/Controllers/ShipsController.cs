using System;
using System.Linq;
using System.Web.Mvc;
using CQRS.Core;
using CQRS.Core.Infrastructure;
using CQRS.Core.ViewModel;
using Loveboat.Domain.Messages.Commands;
using Loveboat.Domain.Messages.Events;
using Loveboat.Domain.ViewModels;
using Loveboat.Hubs;
using SignalR;

namespace Loveboat.Controllers
{
    public class ShipsController : Controller
    {
        private readonly IDtoRepository<ShipViewModel> _shipRepository;
        private readonly IBus bus;

        public ShipsController(IDtoRepository<ShipViewModel> shipRepository, IBus bus)
        {
            if (shipRepository == null) throw new ArgumentNullException("shipRepository");
            if (bus == null) throw new ArgumentNullException("bus");
            _shipRepository = shipRepository;
            this.bus = bus;
        }

        [HttpGet]
        public ActionResult Reset()
        {
            bus.Send(new ShipCreatedCommand("HMS Boobies", "Melbourne"));
            
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Replay()
        {
            bus.Send(new ReplayEventStoreCommand());

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ViewResult Index()
        {
            var ships = _shipRepository.Find();
            var model = new ShipsViewModel() {Ships = ships};

            return View("Index", model);
        }

        [HttpPost]
        public Guid Arrive(ArrivalCommand command)
        {
            if (!ModelState.IsValid)
                return Guid.Empty;

            bus.Send(command);

            return command.CommandId;
        }

        [HttpPost]
        public Guid Depart(DepartureCommand command)
        {
            if (!ModelState.IsValid)
                return Guid.Empty;

            bus.Send(command);

            return command.CommandId;
        }

        [HttpPost]
        public Guid Explode(ExplodingCommand command)
        {
            if (!ModelState.IsValid)
                return Guid.Empty;

            bus.Send(command);

            return command.CommandId;
        }
    }
}