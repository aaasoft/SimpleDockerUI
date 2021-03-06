﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleDockerUI.App.Models
{
    public class DockerContainerItem
    {
        /// <summary>
        /// Container Id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Container Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Container Port
        /// </summary>
        public string Port { get; set; }
        /// <summary>
        /// Container Startup Command
        /// </summary>
        public string Command { get; set; }
        /// <summary>
        /// Container Creation Time
        /// </summary>
        public DateTime Created { get; set; }
        /// <summary>
        /// Container Useing Image
        /// </summary>
        public string Image { get; set; }
        /// <summary>
        /// Container's status
        /// </summary>
        public string Status { get; set; }
    }
}
