using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Launcher.Middleware
{
    public class ViewLoginMiddleware
    {
        private const String RETURN_URL_KEY = "returnUrl";
        private Regex regex = new Regex(
            "^(/view/(?'path'.*))|(/swagger/.*)|(/)$",
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
                "/view/login.html"
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
                var returnUrl = $"{req.Path}";
                if (req.QueryString.HasValue)
                    returnUrl += req.QueryString.ToString();
                returnUrl = System.Text.Encodings.Web.UrlEncoder.Default.Encode(returnUrl);

                rep.ContentType = "text/html;charset=utf-8";
                
                rep.Redirect($"/view/login.html?{RETURN_URL_KEY}={returnUrl}");
                return Task.Run(() => { });
            }
            else
                return _next.Invoke(context);
        }
    }
}
