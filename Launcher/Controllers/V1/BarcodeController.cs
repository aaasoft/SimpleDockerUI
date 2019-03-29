using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Launcher.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{api-version:apiVersion}/[controller]")]
    public class BarcodeController : Controller
    {
        private string name;
        private string password;
        public BarcodeController()
        {
            name = Program.GlobalProperties[$"{typeof(LoginController).FullName}.{nameof(LoginController.Name)}"];
            password = Program.GlobalProperties[$"{typeof(LoginController).FullName}.{nameof(LoginController.Password)}"];
        }

        [HttpGet]
        public IActionResult Get()
        {
            var req = this.Request;
            switch(req.Host.Host)
            {
                case "localhost":
                case "127.0.0.1":
                    return Content("地址为127.0.0.1或localhost时，二维码不可用。");
            }
            var obj = new
            {
                Name = name,
                Url = $"{req.Scheme}://{req.Host}",
                Password = password
            };

            ZXing.Writer formatWriter = new ZXing.MultiFormatWriter();
            ZXing.IBarcodeWriterSvg barcodeWriter = new ZXing.BarcodeWriterSvg();
            ZXing.BarcodeFormat barcodeFormat = ZXing.BarcodeFormat.QR_CODE;
            Dictionary<ZXing.EncodeHintType, object> hints = new Dictionary<ZXing.EncodeHintType, object>()
            {
                [ZXing.EncodeHintType.CHARACTER_SET] = "UTF-8"
            };

            ZXing.Common.BitMatrix bitMatrix = formatWriter.encode(JsonConvert.SerializeObject(obj), barcodeFormat, 400, 400, hints);
            var svgImage = barcodeWriter.Write(bitMatrix);
            var svgContent = svgImage.Content;
            return this.Content(svgContent, "image/svg+xml", Encoding.UTF8);
        }
    }
}
