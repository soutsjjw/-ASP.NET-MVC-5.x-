﻿using AutoMapper;
using MessageBoard.Helpers;
using MessageBoard.Models;
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
        private readonly IMapper _mapper;
        private readonly IGuestbookService _GuestbookService;

        public GuestbooksController(ILogger<GuestbooksController> logger,
            IMapper mapper,
            IGuestbookService GuestbookService)
        {
            _logger = logger;
            _mapper = mapper;
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

            var newData = new Guestbook();
            newData = _mapper.Map<Guestbook>(newData);

            _GuestbookService.InsertGuestbook(newData);

            NotificationsHelper.AddNotification(new NotificationsHelper.Notification { Message = "留言建立成功" });

            return Ok();
        }

        public IActionResult Edit(string id)
        {
            var entity = _GuestbookService.GetDataById(id);
            if (entity == null)
            {
                throw new ArgumentNullException("無法載入此留言板資料");
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
            if (entity == null)
            {
                throw new ArgumentNullException("無法載入此留言板資料");
            }

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
    }
}
