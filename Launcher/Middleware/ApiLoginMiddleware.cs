using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Launcher.Middleware
{
    public class ApiLoginMiddleware
    {
        private Regex regex = new Regex(
            "^/api/(?'path'.*)$",
            //编译为程序集提高匹配速度
            RegexOptions.Compiled
            );

        private RequestDelegate _next;
        public ApiLoginMiddleware(RequestDelegate next = null)
        {
            _next = next;
        }

        private List<string> whiteList = new List<string>(new[]
            {
                "/api/Login"
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
                rep.StatusCode = 401;
                rep.WriteAsync("please login first.");
                return Task.Run(() => { });
            }
            else
                return _next.Invoke(context);
        }
    }
}
