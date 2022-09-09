using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using Microsoft.AspNetCore.Mvc;

namespace CloudLogin.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class LoginController : Controller
    {
        [HttpGet("Signin")]
        public async Task<ActionResult> Login(string RedirectUri, string Identity)
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = RedirectUri
            };
            if (Identity == "Microsoft")
            {
                return Challenge(properties, MicrosoftAccountDefaults.AuthenticationScheme);
            }
            else if (Identity == "Google")
            {
                return Challenge(properties, GoogleDefaults.AuthenticationScheme);
            }
            return null;
        }


        [HttpGet("logout")]
        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }
    }
}
