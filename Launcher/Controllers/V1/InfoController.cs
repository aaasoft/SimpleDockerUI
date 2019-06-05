using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Launcher.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{api-version:apiVersion}/[controller]")]
    public class InfoController : Controller
    {
        [HttpGet]
        public string Get()
        {
            StringBuilder sb = new StringBuilder();
            var connection = ControllerContext.HttpContext.Connection;

            sb.AppendLine("---- Request ----");
            sb.AppendLine("Scheme: " + Request.Scheme);
            sb.AppendLine("Host: " + Request.Host);
            sb.AppendLine("Path: " + Request.Path);
            sb.AppendLine("QueryString: " + Request.QueryString);
            sb.AppendLine("ContentType: " + Request.ContentType);
            sb.AppendLine("Method: " + Request.Method);
            sb.AppendLine("Protocol: " + Request.Protocol);
            sb.AppendLine("IsHttps: " + Request.IsHttps);
            sb.AppendLine("  ---- Request.Headers ----");
            foreach (var header in Request.Headers)
            {
                sb.Append("    ");
                sb.AppendLine($"{header.Key}: {header.Value}");
            }
            return sb.ToString();
        }
    }
}
