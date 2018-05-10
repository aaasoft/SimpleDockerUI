using Microsoft.AspNetCore.Http;
using Quick.CoreMVC.Api;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Quick.CoreMVC;

namespace Launcher.Middleware
{
    public class ViewLoginMiddleware
    {
        //原始正则表达式：^(/|/(?'plugin'[^/]*?)/View/(?'path'.*))$
        private Regex regex = new Regex(
            "^(/|/(?'plugin'[^/]*?)/View/(?'path'.*))$",
            //编译为程序集提高匹配速度
            RegexOptions.Compiled
            );

        private RequestDelegate _next;
        public ViewLoginMiddleware(RequestDelegate next = null)
        {
            _next = next;
        }

        private List<string> whiteList = new List<string>(new[]
            {
                "/Launcher/View/login.html"
            }
        );

        public Task Invoke(HttpContext context)
        {
            var req = context.Request;
            var rep = context.Response;

            var path = req.Path.Value;
            if (!regex.IsMatch(path))
                return _next.Invoke(context);

            var session = context.Session;
            var isLogin = session.GetString("IsLogin");

            if (string.IsNullOrEmpty(isLogin) && !whiteList.Contains(path))
            {
                rep.ContentType = "text/html;charset=utf-8";
                rep.Redirect("/Launcher/View/login.html");
                return Task.Run(() => { });
            }
            else
                return _next.Invoke(context);
        }
    }
}
