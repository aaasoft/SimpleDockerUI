using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Docker.DotNet;
using Docker.DotNet.Models;
using Launcher.Utils;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Launcher.Model;

namespace Launcher.Controllers.V1
{
    /// <summary>
    /// Apis about Container Controller
    /// </summary>
    [ApiVersion("1.0")]
    [Route( "api/v{api-version:apiVersion}/[controller]" )]
    public class ImageController : Controller
    {
        /// <summary>
        /// Get Image List
        /// </summary>
        /// <returns>Image List</returns>
        [HttpGet]
        public IEnumerable<Image> Get()
        {
            IEnumerable<Image> list = null;
#if DEBUG
            var content = System.IO.File.ReadAllText("wwwroot/resource/data/imageList.json");
            list = JsonConvert.DeserializeObject<Image[]>(content);
#else
            DockerClientUtils.UseDockerClient(client =>
            {
                list = client.Images.ListImagesAsync(new ImagesListParameters())
                .Result
                .Select(t => new Model.Image()
                {
                    Id = t.ID,
                    Created = t.Created,
                    SharedSize = t.SharedSize,
                    Size = t.Size,
                    VirtualSize = t.VirtualSize,
                    RepoTags = string.Join(",", t.RepoTags),
                    Labels = t.Labels
                });
            });
#endif
            return list;
        }

        
        /// <summary>
        /// Delete a Image
        /// </summary>
        /// <param name="id">Image's id</param>
        /// <returns></returns>
        /// <response code="500">Other error.</response>
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            try
            {
                DockerClientUtils.UseDockerClient(client =>
                {
                    client.Images.DeleteImageAsync(id,new ImageDeleteParameters()).Wait();
                });
                return base.Ok();
            }
            catch (Exception ex)
            {
                return base.StatusCode(500, $"Delete Error.\r\n{ExceptionUtils.GetExceptionMessage(ex)}");
            }
        }
    }
}