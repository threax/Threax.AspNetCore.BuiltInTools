using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Threax.AspNetCore.BuiltInTools
{
    public class ToolRunner : IToolRunner
    {
        private Dictionary<String, IToolCommand> commands = new Dictionary<string, IToolCommand>();

        public ToolRunner()
        {

        }

        public IToolRunner AddTool(String name, IToolCommand command)
        {
            commands.Add(name, command);
            return this;
        }

        public bool RunTool(String name, ToolArgs args)
        {
            IToolCommand command;
            if (commands.TryGetValue(name, out command))
            {
                command.Execute(args);
                return true;
            }
            return false;
        }

        public IEnumerable<String> HelpMessages
        {
            get
            {
                foreach (var name in commands.Keys)
                {
                    var command = commands[name];
                    var msg = name;
                    var help = command.Help;
                    if (!String.IsNullOrEmpty(help))
                    {
                        msg += " - " + help;
                    }
                    yield return msg;
                }
            }
        }
    }
}
