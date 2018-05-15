using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Framework.WebApi;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySites.DTO;
using MySites.Web.Models;
using Newtonsoft.Json.Linq;

namespace MySites.Web.Controllers
{
    public class LoginController : ApiBasicController
    {
        public LoginController()
        {

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
        public string Login(LoginUser loginUser)
        {
            System.Threading.Thread.Sleep(2000);
            return "aaaaa";
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
        [AllowAnonymous]
        public async Task<JsonResult> Test()
        {
            // discover endpoints from metadata
            var disco = await DiscoveryClient.GetAsync("http://localhost:5002");
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
            }

            // request token
            var tokenClient = new TokenClient(disco.TokenEndpoint, "client", "secret");
            var tokenResponse = await tokenClient.RequestClientCredentialsAsync("api1");

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
            }

            // call api
            var client = new HttpClient();
            client.SetBearerToken(tokenResponse.AccessToken);

            var response = await client.GetAsync("http://localhost:5002/api/account");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(JArray.Parse(content));
            }

            var ret = new JsonResult("");
            return await Task.FromResult(ret);
        }
    }
}