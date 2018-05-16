using System;

namespace Launcher.Model
{
    public class Container
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Port { get; set; }
        public string Command { get; set; }
        public DateTime Created { get; set; }
        public string Image { get; set; }
        public string Status { get; set; }
    }
}