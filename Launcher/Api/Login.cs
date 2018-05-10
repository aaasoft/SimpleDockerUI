using Microsoft.AspNetCore.Http;
using Quick.CoreMVC.Api;
using Quick.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Launcher.Api
{
    public class Login : AbstractMethod<Login.Parameter>, IPropertyHunter
    {
        public class Parameter
        {
            public string Password { get; set; }
        }

        public override string Name => "登录";
        public override HttpMethod Method => HttpMethod.POST;

        private string Password;

        public void Hunt(string key, string value)
        {
            switch (key)
            {
                case nameof(Password):
                    this.Password = value;
                    break;
            }
        }

        public override Task Invoke(HttpContext context, Parameter parameter)
        {
            if (parameter.Password == Password)
            {
                var session = context.Session;
                session.SetString("IsLogin", "true");
                return Task.FromResult(true);
            }
            else
                return Task.FromResult(ApiResult.Error("Password not match."));
        }
    }
}
