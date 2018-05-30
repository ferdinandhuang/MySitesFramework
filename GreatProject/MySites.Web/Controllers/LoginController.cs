using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Framework.WebApi;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySites.DTO;
using MySites.Web.Models;
using Newtonsoft.Json.Linq;

namespace MySites.Web.Controllers
{
    public class LoginController : ApiBasicController
    {
        private readonly IConfiguration configuration;
        public LoginController(IConfiguration _configuration)
        {
            configuration = _configuration;
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
        public async Task<string> Login(LoginUser loginUser)
        {
            var authLink = configuration["RelativeLink:Auth"];
            var dico = DiscoveryClient.GetAsync(authLink).Result;

            //token
            var tokenClient = new TokenClient(dico.TokenEndpoint, "DangguiSite", "secret");
            var tokenResponse = tokenClient.RequestResourceOwnerPasswordAsync(loginUser.Username, loginUser.Password).Result;

            //var a = await Test();
            HttpContext.Response.Headers.Add("Authorization", "Bearer " + tokenResponse.AccessToken);
            return tokenResponse.AccessToken.ToString();
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

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<string> wawaAsync()
        {
            var claim = User;

            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var client = new DiscoveryClient("http://localhost:6001");
            client.Policy.RequireHttps = false;
            var disco = await client.GetAsync();
            var userInfoClient = new UserInfoClient(disco.UserInfoEndpoint);

            var response = await userInfoClient.GetAsync(token);
            var claims = response.Claims;


            string a = "12312123";
            return a;
        }

        private async Task<string> Test()
        {
            var dico = DiscoveryClient.GetAsync("http://localhost:6001").Result;

            //token
            var tokenClient = new TokenClient(dico.TokenEndpoint, "DangguiSite", "secret");
            var tokenResponse = tokenClient.RequestResourceOwnerPasswordAsync("wyt", "123456").Result;

            var tokenResult = await tokenClient.RequestRefreshTokenAsync(tokenResponse.RefreshToken);

            Debug.WriteLine(tokenResponse.AccessToken);
            Debug.WriteLine(tokenResponse.RefreshToken);

            Debug.WriteLine(tokenResult.AccessToken);
            Debug.WriteLine(tokenResult.RefreshToken);

            if (tokenResponse.IsError)
            {
                return "";
            }
            
            //将Token添加到响应头信息
            HttpContext.Response.Headers.Add("Authorization", "Bearer " + tokenResult.AccessToken);


            //测试

            var introspectionClient = new IntrospectionClient(
                dico.IntrospectionEndpoint,
                "api",
                "secret");

            var vresponse = await introspectionClient.SendAsync
                (
                    new IntrospectionRequest { Token = tokenResult.AccessToken }
                );

            var isActive = vresponse.IsActive;



            return tokenResult.AccessToken;
        }
    }
}