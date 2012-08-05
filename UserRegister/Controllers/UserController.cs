using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CQRS.Core;
using UserRegister.Domain.Messages.Commands;

namespace UserRegister.Controllers
{
    public class UserController : Controller
    {
        private readonly IBus _bus;

        public UserController(IBus bus)
        {
            if (bus == null) throw new ArgumentNullException("bus");
            _bus = bus;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public Guid Create(UserCreatedCommand command)
        {
            _bus.Send(command);
            return command.CommandId;
        }
    }
}
