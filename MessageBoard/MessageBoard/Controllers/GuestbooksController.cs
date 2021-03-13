using MessageBoard.Helpers;
using MessageBoard.Services;
using MessageBoard.Services.Interface;
using MessageBoard.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageBoard.Controllers
{
    public class GuestbooksController : Controller
    {
        private readonly ILogger<GuestbooksController> _logger;
        private readonly IGuestbookService _GuestbookService;

        public GuestbooksController(ILogger<GuestbooksController> logger, 
            IGuestbookService GuestbookService)
        {
            _logger = logger;
            _GuestbookService = GuestbookService;
        }

        public IActionResult Index()
        {
            var data = new MessageBoard.ViewModels.Guestbooks.Index();
            data.DataList = _GuestbookService.GetDataList();
            data.Create = new ViewModels.Guestbooks.Create();

            return View(_GuestbookService.GetDataList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(MessageBoard.ViewModels.Guestbooks.Create model)
        {
            if (!ModelState.IsValid)
            {
                foreach(var value in ModelState.Values)
                {
                    foreach(var error in value.Errors)
                    {
                        if(error.Exception == null)
                            _logger.LogError($"{error.ErrorMessage}");
                        else
                            _logger.LogError($"{error.Exception.Message}");
                    }
                }

                return Problem("建立留言時發生錯誤");
            }

            _GuestbookService.InsertGuestbook(model);

            NotificationsHelper.AddNotification(new NotificationsHelper.Notification { Message = "留言建立成功" });

            return Ok();
        }
    }
}
