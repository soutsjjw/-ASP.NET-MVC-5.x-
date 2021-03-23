using AutoMapper;
using MessageBoard.Helpers;
using MessageBoard.Models;
using MessageBoard.Services.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace MessageBoard.Controllers
{
    public class MembersController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly Models.WebConfig _webConfig;
        private readonly IMemberService _memberService;
        private readonly IMailService _mailService;
        private readonly IJwtService _jwtService;

        public MembersController(SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IMapper mapper,
            Models.WebConfig webConfig,
            IMemberService MemberService,
            IMailService MailService,
            IJwtService JwtService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _mapper = mapper;
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
                var newMember = _mapper.Map<ApplicationUser>(registerMember);

                var result = await _memberService.RegisterAsync(newMember, registerMember.Password);
                if (result.Succeeded)
                {
                    var authCode = await _userManager.GenerateEmailConfirmationTokenAsync(newMember);
                    authCode = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(authCode));
                    string tempMail = System.IO.File.ReadAllText("Views/Shared/RegisterEmailTemplate.html");
                    UriBuilder validateUrl = new UriBuilder("http", "localhost", Request.Host.Port ?? 5000)
                    {
                        Path = Url.Action("EmailValidate", "Members",
                        new
                        {
                            Account = newMember.UserName,
                            AuthCode = authCode
                        })
                    };
                    string mailBody = _mailService.GetRegiesterMailBody(tempMail,
                        newMember.Name,
                        validateUrl.ToString().Replace("%3F", "?"));
                    await _mailService.SendRegisterMailAsync(mailBody, newMember.Email);

                    StatusMessageHelper.AddMessage(message: "註冊成功，請去收信進行Email驗證");

                    return RedirectToAction("RegisterResult");
                }
                else
                {
                    foreach(var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
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
            return Json(_memberService.AccountCheckAsync(registerMember.Account));
        }

        public IActionResult EmailValidate(string account, string authCode)
        {
            if (account == null || authCode == null)
            {
                return RedirectToAction("Index", "Home");
            }
            
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
        public async Task<IActionResult> LoginAsync(ViewModels.Members.Login loginMember)
        {
            string validateStr = await _memberService.LoginCheckAsync(loginMember.Account, loginMember.Password);
            if (string.IsNullOrWhiteSpace(validateStr))
            {
                return RedirectToAction("Index", "Guestbooks");
            }
            else
            {
                ModelState.AddModelError("", validateStr);
                return View(loginMember);
            }
        }

        [Authorize]
        public IActionResult Logout()
        {
            _signInManager.SignOutAsync();

            return RedirectToAction(nameof(Login));
        }

        [Authorize]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePasswordAsync(ViewModels.Members.ChangePassword changeData)
        {
            if (ModelState.IsValid)
            {
                var applicationUser = await _userManager.FindByNameAsync(User.Identity.Name);
                var result = _memberService.ChangePassword(applicationUser, changeData.Password, changeData.NewPassword, out string message);

                StatusMessageHelper.AddMessage(message: message, contentType: result ? StatusMessageHelper.ContentType.Success : StatusMessageHelper.ContentType.Warning);
            }

            return View();
        }
    }
}
