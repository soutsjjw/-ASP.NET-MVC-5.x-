using Microsoft.AspNetCore.Mvc;

namespace MessageBoard.Controllers
{
    public class BaseController : Controller
    {
        protected string UserName
        {
            get
            {
                if (User.Identity.IsAuthenticated)
                    return User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value;
                else
                    return null;
            }
        }

        protected string UserId
        {
            get
            {
                if (User.Identity.IsAuthenticated)
                    return User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                else
                    return null;
            }
        }
    }
}
