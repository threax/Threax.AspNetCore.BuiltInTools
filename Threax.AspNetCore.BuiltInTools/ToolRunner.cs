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
        private List<Action<ToolArgs>> afterTools = new List<Action<ToolArgs>>();

        public ToolRunner()
        {

        }

        public IToolRunner AddTool(String name, IToolCommand command)
        {
            commands.Add(name, command);
            return this;
        }

        public bool RunTool(ToolArgs args)
        {
            IToolCommand command;
            if (commands.TryGetValue(args.Name, out command))
            {
                command.Execute(args);
                return true;
            }
            return false;
        }

        public IToolRunner AfterTools(Action<ToolArgs> cb)
        {
            afterTools.Add(cb);
            return this;
        }

        public void RunAfterTools(ToolArgs args)
        {
            foreach(var evt in afterTools)
            {
                evt.Invoke(args);
            }
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
