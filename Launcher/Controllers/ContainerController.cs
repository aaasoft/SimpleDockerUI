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
        /// Create a Container
        /// </summary>
        /// <param name="config">Config to create container.</param>
        /// <returns></returns>
        /// <response code="200">return created container's info</response> 
        /// <response code="500">Other error.</response>
        [HttpPost]
        [ProducesResponseType(typeof(Container), 200)]
        public IActionResult Create([FromBody]CreateContainerConfig config)
        {
            try
            {
                string id = null;
                DockerClientUtils.UseDockerClient(client =>
                {
                    var ret = client.Containers.CreateContainerAsync(new CreateContainerParameters()
                    {
                        Shell = config.Shell,
                        Entrypoint = config.Entrypoint,
                        WorkingDir = config.WorkingDir,
                        Volumes = config.Volumes,
                        Image = config.Image,
                        Cmd = config.Cmd,
                        Env = config.Env,
                        ExposedPorts = config.ExposedPorts,
                        Name = config.Name,
                        HostConfig = config.HostConfig,
                        NetworkingConfig = config.NetworkingConfig
                    }).Result;
                    id = ret.ID;
                });
                return new ObjectResult(Get(id));
            }
            catch (Exception ex)
            {
                return base.StatusCode(500, $"Add Error.\r\n{ExceptionUtils.GetExceptionMessage(ex)}");
            }
        }

        /// <summary>
        /// Delete a Container
        /// </summary>
        /// <param name="id">Container's id</param>
        /// <returns></returns>
        /// <response code="500">Other error.</response>
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            try
            {
                DockerClientUtils.UseDockerClient(client =>
                {
                    client.Containers.RemoveContainerAsync(id, new ContainerRemoveParameters() { }).Wait();
                });
                return base.Ok();
            }
            catch (Exception ex)
            {
                return base.StatusCode(500, $"Delete Error.\r\n{ExceptionUtils.GetExceptionMessage(ex)}");
            }
        }

        /// <summary>
        /// Start a Container
        /// </summary>
        /// <param name="id">Container's id</param>
        /// <returns></returns>
        /// <response code="500">Other error.</response> 
        [HttpPut("{id}/Start")]
        public IActionResult Start(string id)
        {
            try
            {
                bool ret = false;
                DockerClientUtils.UseDockerClient(client =>
                {
                    ret = client.Containers.StartContainerAsync(id, new ContainerStartParameters()).Result;
                });
                if (ret)
                    return base.Ok();
                return base.StatusCode(500, $"Start Error.\r\nStart Method return 'false'.");
            }
            catch (Exception ex)
            {
                return base.StatusCode(500, $"Start Error.\r\n{ExceptionUtils.GetExceptionMessage(ex)}");
            }
        }

        /// <summary>
        /// Stop a Conatiner
        /// </summary>
        /// <param name="id">Container's id</param>
        /// <returns></returns>
        /// <response code="500">Other error.</response> 
        [HttpPut("{id}/Stop")]
        public IActionResult Stop(string id)
        {
            try
            {
                bool ret = false;
                DockerClientUtils.UseDockerClient(client =>
                {
                    ret = client.Containers.StopContainerAsync(id, new ContainerStopParameters()).Result;
                });
                if (ret)
                    return base.Ok();
                return base.StatusCode(500, $"Stop Error.\r\nStop Method return 'false'.");
            }
            catch (Exception ex)
            {
                return base.StatusCode(500, $"Stop Error.\r\n{ExceptionUtils.GetExceptionMessage(ex)}");
            }
        }

        /// <summary>
        /// Restart a Container
        /// </summary>
        /// <param name="id">Container's id</param>
        /// <returns></returns>
        /// <response code="500">Other error.</response> 
        [HttpPut("{id}/Restart")]
        public IActionResult Restart(string id)
        {
            var ret = Stop(id);
            if (!(ret is OkResult))
                return ret;
            ret = Start(id);
            if (!(ret is OkResult))
                return ret;
            return base.Ok();
        }
    }
}
