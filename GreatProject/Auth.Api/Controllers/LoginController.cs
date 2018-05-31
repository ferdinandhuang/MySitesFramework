using Framework.Core.Cache;
using Framework.Core.Common;
using Framework.Core.Extensions;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySites.DTO;
using System;
using System.Threading.Tasks;

namespace MySites.Web.Controllers
{
    public class LoginController : Controller
    {
        private readonly IConfiguration Configuration;
        private readonly string AuthLink;
        private readonly DiscoveryResponse DiscoveryResponse;
        public LoginController(IConfiguration _configuration)
        {
            Configuration = _configuration;
            AuthLink = Configuration["RelativeLink:Auth"];
            DiscoveryResponse = DiscoveryClient.GetAsync("http://localhost:6001").Result;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            //登录授权直接跳转Home
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return Redirect("url");
            }
            return View();
        }
        
        /// <summary>
        /// 注销
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var userID = await GetUserID(token);
            SetCache(userID);
            return View("Index");
        }


        /// <summary>
        /// 登陆
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> Login(LoginUser loginUser)
        {
            try
            {
                var tokenClient = new TokenClient(DiscoveryResponse.TokenEndpoint, Configuration["ApiInfo:ClientId"], Configuration["ApiInfo:Secrect"]);
                var tokenResponse = tokenClient.RequestResourceOwnerPasswordAsync(loginUser.Username, loginUser.Password).Result;

                if (tokenResponse.IsError)
                {
                    return Json(new Result<string>() { Status = Status.Failed, Message = tokenResponse.Error });
                }

                var userId = await GetUserID(tokenResponse.AccessToken);
                if (userId == null)
                    throw new Exception("No UserID");
                //设置缓存
                SetCache(userId.ToString(), new TokenCache { AccessToken = tokenResponse.AccessToken, RefreshToken = tokenResponse.RefreshToken });

                //添加头信息
                HttpContext.Response.Headers.Add("Authorization", "Bearer " + tokenResponse.AccessToken);
                HttpContext.Response.Headers.Add("Refresh_Token", tokenResponse.RefreshToken);

                return Json(new Result<string>() { Status = Status.Success });
            }
            catch(Exception ee)
            {
                return Json(new Result<string>() { Status = Status.Error, Message = "Something Happend!" });
            }
        }

        /// <summary>
        /// 忘记密码
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        [AllowAnonymous]
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
        [AllowAnonymous]
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

                var userId = GetUserID(tokenResponse.AccessToken);
                if (userId == null)
                    throw new Exception("No UserID");
                //设置缓存
                SetCache(userId.ToString(), new TokenCache { AccessToken = tokenResponse.AccessToken, RefreshToken = tokenResponse.RefreshToken });

                //添加头信息
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
        /// 获取用户ID
        /// </summary>
        /// <param name="token">access_token</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<string> GetUserID(string token)
        {

            var userInfoClient = new UserInfoClient(DiscoveryResponse.UserInfoEndpoint);
            var response = await userInfoClient.GetAsync(token);
            var claims = response.Claims;

            foreach (var claim in claims)
            {
                if (claim.Type == "sub")
                {
                    //在缓存中查找Token
                    var cacheToken = DistributedCacheManager.Get(claim.Value);
                    if (cacheToken == null)
                    {
                        return null;
                    }
                    
                    return claim.Value;
                }
            }

            return null;
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">cache_key</param>
        /// <param name="something">content if null : remove cache</param>
        /// <returns></returns>
        private void SetCache(string key, object something = null)
        {
            if(something == null)
            {
                //删除缓存
                DistributedCacheManager.Remove(key);
            }

            //根据key获取缓存
            var content = DistributedCacheManager.Get(key);
            if (content != null)
            {
                //删除缓存
                DistributedCacheManager.Remove(key);
            }

            //设置缓存
            DistributedCacheManager.Set(key, something, 10);
            return;
        }

        /// <summary>
        /// 权限测试
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
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

    [Serializable]
    class TokenCache
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}