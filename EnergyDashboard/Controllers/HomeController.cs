using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace EnergyDashboard.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {
        }

        public ActionResult Index()
        {
            //var model = new IndexViewModel
            //{
            //    Rooms = (await _roomRepository.GetRoomsAsync())
            //        .Select(r => new RoomViewModel
            //        {
            //            RoomId = r.RoomId,
            //            RoomName = r.RoomName
            //        })
            //        .OrderBy(r => r.RoomName)
            //        .ToList()
            //};

            return View();
        }
    }
}
