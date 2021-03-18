using MessageBoard.Helpers;
using MessageBoard.Services.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageBoard.Controllers
{
    public class MembersController : BaseController
    {
        private readonly Models.WebConfig _webConfig;
        private readonly IMemberService _memberService;
        private readonly IMailService _mailService;
        private readonly IJwtService _jwtService;

        public MembersController(Models.WebConfig webConfig,
            IMemberService MemberService,
            IMailService MailService,
            IJwtService JwtService)
        {
            _webConfig = webConfig;
            _memberService = MemberService;
            _mailService = MailService;
            _jwtService = JwtService;
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
                var authCode = _mailService.GetValidateCode();
                registerMember.newMember.AuthCode = authCode;

                try
                {
                    _memberService.Register(registerMember.newMember);
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
                string mailBody = _mailService.GetRegiesterMailBody(tempMail,
                    registerMember.newMember.Name,
                    validateUrl.ToString().Replace("%3F", "?"));
                await _mailService.SendRegisterMailAsync(mailBody, registerMember.newMember.Email);

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
            return Json(_memberService.AccountCheck(registerMember.newMember.Account));
        }

        public IActionResult EmailValidate(string account, string authCode)
        {
            var blPass = _memberService.EmailValidate(account, authCode, out string message);
            StatusMessageHelper.AddMessage(message: message, contentType: blPass ? StatusMessageHelper.ContentType.Success : StatusMessageHelper.ContentType.Danger);

            return View();
        }

        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Guestbooks");
            return View();
        }

        [HttpPost]
        public IActionResult Login(ViewModels.Members.Login loginMember)
        {
            string validateStr = _memberService.LoginCheck(loginMember.Account, loginMember.Password);
            if (string.IsNullOrWhiteSpace(validateStr))
            {
                var member = _memberService.GetDataByAccount(loginMember.Account);
                string roleData = _memberService.GetRole(loginMember.Account);
                var token = _jwtService.GenerateToken(member.Id, member.Name, roleData);

                Response.Cookies.Append(_webConfig.Jwt.CookieName, token, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });
                //Response.Cookies.Append("X-Username", loginMember.Account, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });
                //Response.Cookies.Append("X-Refresh-Token", Guid.NewGuid().ToString(), new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });

                return RedirectToAction("Index", "Guestbooks");
            }
            else
            {
                ModelState.AddModelError("", validateStr);
                return View(loginMember);
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Logout()
        {
            Response.Cookies.Delete(_webConfig.Jwt.CookieName);

            return RedirectToAction(nameof(Login));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangePassword(ViewModels.Members.ChangePassword changeData)
        {
            if (ModelState.IsValid)
            {
                var result = _memberService.ChangePassword(this.UserId, changeData.Password, changeData.NewPassword, out string message);

                StatusMessageHelper.AddMessage(message: message, contentType: result ? StatusMessageHelper.ContentType.Success : StatusMessageHelper.ContentType.Warning);
            }

            return View();
        }
    }
}
