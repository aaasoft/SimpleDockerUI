using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Launcher.Middleware
{
    public class LoginMiddleware
    {
        //原始正则表达式：^/(?'plugin'[^/]*?)/View/(?'path'.*)$
        private Regex regex = new Regex(
            "^/(?'plugin'[^/]*?)/View/(?'path'.*)$",
            //编译为程序集提高匹配速度
            RegexOptions.Compiled
            );

        private RequestDelegate _next;
        public LoginMiddleware(RequestDelegate next = null)
        {
            _next = next;
        }

        public Task Invoke(HttpContext context)
        {
            var req = context.Request;
            var rep = context.Response;

            var path = req.Path.Value;
            if (!regex.IsMatch(path))
                return _next.Invoke(context);

            if(false)
            {
                var session = context.Session;
                session.SetString("test", Guid.NewGuid().ToString());
                rep.ContentType = "text/html;charset=utf-8";
                return rep.WriteAsync("请先登录！");
            }
            else
            {
                return _next.Invoke(context);
            }            
        }
    }
}
