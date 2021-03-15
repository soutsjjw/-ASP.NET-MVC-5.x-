﻿using AutoMapper;
using MessageBoard.Helpers;
using MessageBoard.Models;
using MessageBoard.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using X.PagedList;

namespace MessageBoard.Controllers
{
    public class GuestbooksController : Controller
    {
        private readonly ILogger<GuestbooksController> _logger;
        private readonly IMapper _mapper;
        private readonly IGuestbookService _GuestbookService;
        private readonly IConfigHelper _configHelper;

        public GuestbooksController(ILogger<GuestbooksController> logger,
            IMapper mapper,
            IGuestbookService GuestbookService,
            IConfigHelper configHelper)
        {
            _logger = logger;
            _mapper = mapper;
            _GuestbookService = GuestbookService;
            _configHelper = configHelper;
        }

        public IActionResult Index(string search, int page = 1)
        {
            var data = new ViewModels.Guestbooks.Index();
            data.DataList = _GuestbookService.GetDataList(search).ToPagedList(page, _configHelper.GetInteger("PageSize", 1));
            data.Create = new ViewModels.Guestbooks.Create();
            data.Search = search;

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
            _GuestbookService.DeleteGuestbook(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
