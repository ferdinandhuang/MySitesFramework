using Framework.Core.Common;
using Framework.Core.Extensions;
using Framework.WebApi;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySites.DTO;
using System.Threading.Tasks;

namespace MySites.Web.Controllers
{
    public class LoginController : ApiBasicController
    {
        private readonly IConfiguration Configuration;
        private readonly string AuthLink;
        private readonly DiscoveryResponse DiscoveryResponse;
        public LoginController(IConfiguration _configuration)
        {
            Configuration = _configuration;
            AuthLink = Configuration["RelativeLink:Auth"];
            DiscoveryResponse = DiscoveryClient.GetAsync(AuthLink).Result;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            //登录授权直接跳转Home
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        /// <summary>
        /// 登陆
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        public JsonResult Login(LoginUser loginUser)
        {
            try
            {
                var tokenClient = new TokenClient(DiscoveryResponse.TokenEndpoint, Configuration["ApiInfo:ClientId"], Configuration["ApiInfo:Secrect"]);
                var tokenResponse = tokenClient.RequestResourceOwnerPasswordAsync(loginUser.Username, loginUser.Password).Result;
                
                if (tokenResponse.IsError)
                {
                    return Json(new Result<string>() { Status = Status.Failed, Message = tokenResponse.Error });
                }

                HttpContext.Response.Headers.Add("Authorization", "Bearer " + tokenResponse.AccessToken);
                HttpContext.Response.Headers.Add("Refresh_Token", tokenResponse.RefreshToken);

                return Json(new Result<string>() { Status = Status.Success });
            }
            catch
            {
                return Json(new Result<string>() { Status = Status.Error, Message = "Something Happend!" });
            }
        }

        /// <summary>
        /// 忘记密码
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        public IActionResult Foreget(string userName)
        {
            return View();
        }

        /// <summary>
        /// 刷新Token
        /// </summary>
        /// <param name="refresh_token"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> RefreshTokenAsync(string refresh_token)
        {
            try
            {
                var tokenClient = new TokenClient(DiscoveryResponse.TokenEndpoint, Configuration["ApiInfo:ClientId"], Configuration["ApiInfo:Secrect"]);
                var tokenResponse = await tokenClient.RequestRefreshTokenAsync(refresh_token);

                if (tokenResponse.IsError)
                {
                    return Json(new Result<string>() { Status = Status.Failed, Message = tokenResponse.Error });
                }

                HttpContext.Response.Headers.Add("Authorization", "Bearer " + tokenResponse.AccessToken);
                HttpContext.Response.Headers.Add("Refresh_Token", tokenResponse.RefreshToken);

                return Json(new Result<string>() { Status = Status.Success });
            }
            catch
            {
                return Json(new Result<string>() { Status = Status.Error, Message = "Something Happend!" });
            }
        }

        /// <summary>
        /// 权限测试
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public async Task<string> wawaAsync()
        {
            var claim = User;

            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var client = new DiscoveryClient("http://localhost:6001");
            client.Policy.RequireHttps = false;
            var disco = await client.GetAsync();
            var userInfoClient = new UserInfoClient(DiscoveryResponse.UserInfoEndpoint);

            var response = await userInfoClient.GetAsync(token);
            var claims = response.Claims;

            return "Success";
        }
    }
}