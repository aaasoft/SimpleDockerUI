using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Quick.Properties;
using Quick.Properties.Utils;

namespace Launcher.Controllers.V1
{
    /// <summary>
    /// Apis about Login
    /// </summary>
    /// <remarks>
    /// Some remarks can put here.
    /// </remarks>
    [ApiVersion("1.0")]
    [Route( "api/v{api-version:apiVersion}/[controller]" )]
    //[Produces("application/json")]
    public class LoginController : Controller, IPropertyHunter
    {
        public LoginController()
        {
            //Init properties
            HunterUtils.TryHunt(this, Program.GlobalProperties);
        }

        internal string Name;
        internal string Password;

        void IPropertyHunter.Hunt(string key, string value)
        {
            switch (key)
            {
                case nameof(Name):
                    this.Name = value;
                    break;
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

        /// <summary>
        /// Logout
        /// </summary>
        [HttpDelete]
        public IActionResult Logout()
        {
            var session = base.HttpContext.Session;
            session.Clear();
            return Ok();
        }
    }
}