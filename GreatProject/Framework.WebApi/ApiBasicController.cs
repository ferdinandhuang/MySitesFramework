using Framework.WebApi.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.WebApi
{
    /// <summary>
    /// WebApi Basic Controller
    /// </summary>
    [ApiActionFilter]
    [Authorize]
    public class ApiBasicController : Controller
    {

    }
}
