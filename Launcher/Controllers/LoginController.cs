using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Quick.Properties;
using Quick.Properties.Utils;

namespace Launcher.Controllers
{
    [Route("api/[controller]")]
    public class LoginController : Controller, IPropertyHunter
    {
        public LoginController()
        {
            //Init properties
            HunterUtils.TryHunt(this, Program.GlobalProperties);
        }

        private string Password;

        void IPropertyHunter.Hunt(string key, string value)
        {
            switch (key)
            {
                case nameof(Password):
                    this.Password = value;
                    break;
            }
        }

        [HttpPost]
        public IActionResult Post([FromForm]string password)
        {
            if (this.Password == password)
            {
                var session = base.HttpContext.Session;
                session.SetString("IsLogin", "true");
                return Ok();
            }
            else
                return StatusCode(401, "Password not match.");
        }
    }
}