using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using DotNetTraining.RealTimeNoti.Hubs;
using DotNetTraining.RealTimeNoti.Models;
namespace DotNetTraining.RealTimeNoti.Controllers
{
    public class AnnouncementController : Controller
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public AnnouncementController(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Send(AnnouncementRequestModel RequestModel)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveAnnouncement", RequestModel.Title, RequestModel.Content);
            return RedirectToAction("Index");
        }
    }
}