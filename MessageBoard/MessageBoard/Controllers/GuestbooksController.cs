using AutoMapper;
using MessageBoard.Helpers;
using MessageBoard.Models;
using MessageBoard.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using X.PagedList;

namespace MessageBoard.Controllers
{
    [Authorize]
    public class GuestbooksController : BaseController
    {
        private readonly ILogger<GuestbooksController> _logger;
        private readonly IMapper _mapper;
        private readonly IGuestbookService _GuestbookService;
        private readonly IMemberService _memberService;
        private readonly Models.WebConfig _config;

        public GuestbooksController(ILogger<GuestbooksController> logger,
            IMapper mapper,
            IGuestbookService GuestbookService,
            IMemberService memberService,
            Models.WebConfig config)
        {
            _logger = logger;
            _mapper = mapper;
            _GuestbookService = GuestbookService;
            _memberService = memberService;
            _config = config;
        }

        public IActionResult Index(string search, int page = 1)
        {
            var data = new ViewModels.Guestbooks.Index();
            data.DataList = _GuestbookService.GetDataList(search).ToPagedList(page, _config.Pagination.PageSize);
            data.Create = new ViewModels.Guestbooks.Create() { Name = this.UserName };
            data.Search = search;

            ViewBag.UserId = this.UserId;

            var isAjax = Request.Headers["X-Requested-With"] == "XMLHttpRequest";
            if (isAjax)
            {
                return PartialView("_DataListPartial", data);
            }

            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(MessageBoard.ViewModels.Guestbooks.Create model)
        {
            if (!ModelState.IsValid)
            {
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        if (error.Exception == null)
                            _logger.LogError($"{error.ErrorMessage}");
                        else
                            _logger.LogError($"{error.Exception.Message}");
                    }
                }

                return Problem("建立留言時發生錯誤");
            }

            var newData = _mapper.Map<Guestbook>(model);

            try
            {
                var member = _memberService.GetDataById(this.UserId);
                if (member == null)
                {
                    throw new Exception("使用者資料錯誤，請重新登入！");
                }

                newData.MemberId = this.UserId;
                _GuestbookService.InsertGuestbook(newData);

                NotificationsHelper.AddNotification(new NotificationsHelper.Notification { Message = "留言建立成功" });
                return Ok();
            }
            catch(Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        public IActionResult Edit(string id)
        {
            var entity = _GuestbookService.GetDataById(id);

            if (entity.MemberId != this.UserId)
            {
                StatusMessageHelper.AddMessage(message: "您無法修改此留言！", contentType: StatusMessageHelper.ContentType.Danger);
                return RedirectToAction(nameof(Index));
            }

            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, [Bind(include: "Name,Content")] Guestbook updateData)
        {
            if (_GuestbookService.CheckUpdate(id))
            {
                updateData.Id = id;

                _GuestbookService.UpdateGuestbook(updateData);
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Reply(string id)
        {
            var entity = _GuestbookService.GetDataById(id);

            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Reply(string id, [Bind(include: "Reply,ReplyTime")]Guestbook replyData)
        {
            if (_GuestbookService.CheckUpdate(id))
            {
                replyData.Id = id;

                _GuestbookService.ReplyGuestbook(replyData);
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(string id)
        {
            var entity = _GuestbookService.GetDataById(id);

            if (entity.MemberId != this.UserId)
            {
                StatusMessageHelper.AddMessage(message: "您無法刪除此留言！", contentType: StatusMessageHelper.ContentType.Danger);
                return RedirectToAction(nameof(Index));
            }

            _GuestbookService.DeleteGuestbook(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
