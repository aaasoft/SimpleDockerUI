using Microsoft.AspNetCore.Http;
using Quick.CoreMVC.Api;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Quick.CoreMVC;
using Newtonsoft.Json;

namespace Launcher.Middleware
{
    public class ApiLoginMiddleware
    {
        //原始正则表达式：^/(?'plugin'[^/]*?)/Api/(?'path'.*)$
        private Regex regex = new Regex(
            "^/(?'plugin'[^/]*?)/Api/(?'path'.*)$",
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
                "/Launcher/Api/Login"
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
                rep.ContentType = "application/json;charset=utf-8";
                rep.WriteAsync(JsonConvert.SerializeObject(ApiResult.Error("please login first.")));
                return Task.Run(() => { });
            }
            else
                return _next.Invoke(context);
        }
    }
}
