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

namespace Launcher.Controllers
{
    /// <summary>
    /// Apis about Container Controller
    /// </summary>
    [Route("api/[controller]")]    
    public class ContainerController : Controller
    {
        /// <summary>
        /// Get Container List
        /// </summary>
        /// <returns>Container List</returns>
        [HttpGet]
        public IEnumerable<Container> Get()
        {
            IEnumerable<Container> list = null;
#if DEBUG
            var content = System.IO.File.ReadAllText("wwwroot/Resource/debug/containerList.json");
            list = JsonConvert.DeserializeObject<Container[]>(content);
#else
            DockerClientUtils.UseDockerClient(client =>
            {
                list = client.Containers.ListContainersAsync(new ContainersListParameters() { All = true })
                .Result
                .Select(t => new Model.Container()
                {
                    Id = t.ID,
                    Name = t.Names?.LastOrDefault().Substring(1),
                    Port = string.Join(", ", t.Ports?.Select(p => $"{p.IP}:{p.PublicPort}->{p.PrivatePort}/{p.Type}")),
                    Command = t.Command,
                    Created = t.Created,
                    Image = t.Image,
                    Status = t.Status
                });
            });
#endif
            return list;
        }

        /// <summary>
        /// Get a Container's info
        /// </summary>
        /// <param name="id">Container's id</param>
        /// <returns></returns>
        /// <response code="204">Can't found Container by input id.</response> 
        [HttpGet("{id}")]
        public Container Get(string id)
        {
            return Get().FirstOrDefault(t => t.Id == id);
        }

        /// <summary>
        /// Start a Container
        /// </summary>
        /// <param name="id">Container's id</param>
        /// <returns></returns>
        [HttpPut("{id}/start")]
        public IActionResult Start(string id)
        {
            bool ret = false;
            string message = "Start Error.";
            try
            {
                DockerClientUtils.UseDockerClient(client =>
                {
                    ret = client.Containers.StartContainerAsync(id, new ContainerStartParameters()).Result;
                });
            }
            catch (Exception ex)
            {
                message += "\r\n" + ExceptionUtils.GetExceptionMessage(ex);
            }
            if (!ret)
                return base.StatusCode(500, message);
            return base.Ok();
        }

        /// <summary>
        /// Stop a Conatiner
        /// </summary>
        /// <param name="id">Container's id</param>
        /// <returns></returns>
        [HttpPut("{id}/stop")]
        public IActionResult Stop(string id)
        {
            bool ret = false;
            string message = "Stop Error.";
            try
            {
                DockerClientUtils.UseDockerClient(client =>
                {
                    ret = client.Containers.StopContainerAsync(id, new ContainerStopParameters()).Result;
                });
            }
            catch (Exception ex)
            {
                message += "\r\n" + ExceptionUtils.GetExceptionMessage(ex);
            }
            if (!ret)
                return base.StatusCode(500, message);
            return base.Ok();
        }

        /// <summary>
        /// Restart a Container
        /// </summary>
        /// <param name="id">Container's id</param>
        /// <returns></returns>
        [HttpPut("{id}/restart")]
        public IActionResult Restart(string id)
        {
            bool ret = false;
            string message = "Restart Error.";
            try
            {
                DockerClientUtils.UseDockerClient(client =>
                {
                    ret = client.Containers.StopContainerAsync(id, new ContainerStopParameters()).Result;
                    if (!ret)
                        return;
                    ret = client.Containers.StartContainerAsync(id, new ContainerStartParameters()).Result;
                    if (!ret)
                        return;
                });
            }
            catch (Exception ex)
            {
                message += "\r\n" + ExceptionUtils.GetExceptionMessage(ex);
            }
            if (!ret)
                return base.StatusCode(500, message);
            return base.Ok();
        }
    }
}
