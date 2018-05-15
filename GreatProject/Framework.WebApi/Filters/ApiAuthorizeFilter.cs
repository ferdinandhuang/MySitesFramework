using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Framework.WebApi.Filters
{
    public class ApiAuthorizeFilter : AuthorizeFilter
    {
        public ApiAuthorizeFilter(AuthorizationPolicy policy) : base(policy)
        {
        }

        public override Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            try
            {
                var attr = context.ActionDescriptor.FilterDescriptors.ToList();
                bool isAnonymous = attr.Any(a => a.Filter is AllowAnonymousFilter);
                //是匿名用户，则继续执行；非匿名用户，抛出“未授权访问”信息
                if (isAnonymous)
                {
                    return base.OnAuthorizationAsync(context);
                }

                var token =context.HttpContext.Request.Headers["dangguitoken"];
                //不带Token
                if (String.IsNullOrEmpty(token))
                {
                    return base.OnAuthorizationAsync(context);
                }
            }
            catch (Exception ex)
            {

            }
            return base.OnAuthorizationAsync(context);
        }
    }
}
