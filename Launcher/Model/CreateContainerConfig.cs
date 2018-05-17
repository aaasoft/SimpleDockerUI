using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Docker.DotNet.Models;

namespace Launcher.Model
{
    public class CreateContainerConfig
    {
        public IList<string> Shell { get; set; }
        public IList<string> Entrypoint { get; set; }
        public string WorkingDir { get; set; }
        public IDictionary<string, EmptyStruct> Volumes { get; set; }
        public string Image { get; set; }
        public IList<string> Cmd { get; set; }
        public IList<string> Env { get; set; }
        public IDictionary<string, EmptyStruct> ExposedPorts { get; set; }
        public string Name { get; set; }
        public HostConfig HostConfig { get; set; }
        public NetworkingConfig NetworkingConfig { get; set; }
    }
}