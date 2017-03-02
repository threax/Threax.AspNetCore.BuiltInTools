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
        public ToolArgs()
        {

        }

        public ToolArgs(String command)
        {
            var split = command.Split(default(char[]), StringSplitOptions.RemoveEmptyEntries);
            if(split.Length == 0)
            {
                throw new ToolException($"Tool command '{command}' did not have any items in it.");
            }
            Name = split[0];
            Args = new List<string>(split.Skip(1));
        }

        public IWebHost Host { get; set; }

        public IServiceScope Scope { get; set; }

        public ILogger Log { get; set; }

        public List<String> Args { get; set; }

        public String Name { get; set; }
    }
}
