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
    /// <summary>
    /// Apis about Login
    /// </summary>
    /// <remarks>
    /// Some remarks can put here.
    /// </remarks>
        [Route("api/[controller]")]
    //[Produces("application/json")]
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

        /// <summary>
        /// Login to SimpleDockerUI
        /// </summary>
        /// <remarks>
        /// Some remarks can put here.
        /// </remarks>
        /// <param name="password">The password</param>
        /// <returns></returns>
        /// <response code="401">Password not match.</response> 
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