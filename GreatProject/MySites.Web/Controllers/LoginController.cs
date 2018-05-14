using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Framework.WebApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySites.DTO;
using MySites.Web.Models;

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
    }
}