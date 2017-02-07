using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Threax.AspNetCore.BuiltInTools
{
    public class ToolArgs
    {
        public String[] Args { get; set; }

        public IWebHost Host { get; set; }

        public IServiceScope Scope { get; set; }

        public ILogger Log { get; set; }
    }
}
