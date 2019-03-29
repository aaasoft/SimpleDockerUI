using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleDockerUI.App.Models
{
    public interface ICheckable
    {
        CheckResult Check();
    }
}
