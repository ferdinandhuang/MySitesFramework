using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Framework.WebApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySites.IServices;
using MySites.Web.Models;

namespace MySites.Web.Controllers
{
    public class HomeController : ApiBasicController
    {
        private IClass1 class1;
        public HomeController(IClass1 _class)
        {
            class1 = _class;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            class1.Test();
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
