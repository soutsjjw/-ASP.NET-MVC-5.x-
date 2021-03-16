using MessageBoard.Helpers;
using MessageBoard.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageBoard.Controllers
{
    public class MembersController : Controller
    {
        private readonly IMemberService _MemberService;
        private readonly IMailService _MailService;

        public MembersController(IMemberService MemberService,
            IMailService MailService)
        {
            _MemberService = MemberService;
            _MailService = MailService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Guestbooks");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(ViewModels.Members.Register registerMember)
        {
            if (ModelState.IsValid)
            {
                registerMember.newMember.Password = registerMember.Password;
                var authCode = _MailService.GetValidateCode();
                registerMember.newMember.AuthCode = authCode;

                try
                {
                    _MemberService.Register(registerMember.newMember);
                }
                catch(Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);

                    registerMember.Password = null;
                    registerMember.PasswordCheck = null;

                    return View(registerMember);
                }

                string tempMail = System.IO.File.ReadAllText("Views/Shared/RegisterEmailTemplate.html");
                UriBuilder validateUrl = new UriBuilder("http", "localhost", Request.Host.Port ?? 5000)
                {
                    Path = Url.Action("EmailValidate", "Members", 
                    new
                    {
                        Account = registerMember.newMember.Account,
                        AuthCode= authCode
                    })
                };
                string mailBody = _MailService.GetRegiesterMailBody(tempMail,
                    registerMember.newMember.Name,
                    validateUrl.ToString().Replace("%3F", "?"));
                await _MailService.SendRegisterMailAsync(mailBody, registerMember.newMember.Email);

                StatusMessageHelper.AddMessage(message: "註冊成功，請去收信進行Email驗證");

                return RedirectToAction("RegisterResult");
            }

            registerMember.Password = null;
            registerMember.PasswordCheck = null;

            return View(registerMember);
        }

        public IActionResult RegisterResult()
        {
            return View();
        }

        public IActionResult AccountCheck(ViewModels.Members.Register registerMember)
        {
            return Json(_MemberService.AccountCheck(registerMember.newMember.Account));
        }

        public IActionResult EmailValidate(string Account, string AuthCode)
        {
            var blPass = _MemberService.EmailValidate(Account, AuthCode, out string message);
            StatusMessageHelper.AddMessage(message: message, contentType: blPass ? StatusMessageHelper.ContentType.Success : StatusMessageHelper.ContentType.Danger);

            return View();
        }
    }
}
