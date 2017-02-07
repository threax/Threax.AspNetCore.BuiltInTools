using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Threax.AspNetCore.BuiltInTools
{
    /// <summary>
    /// A tool command that uses an action.
    /// </summary>
    public class ToolCommand : IToolCommand
    {
        public Action<ToolArgs> action;

        public ToolCommand(Action<ToolArgs> action)
        {
            this.action = action;
        }

        public ToolCommand(String help, Action<ToolArgs> action)
        {
            this.action = action;
            this.Help = help;
        }

        public string Help { get; set; }

        public void Execute(ToolArgs args)
        {
            action(args);
        }
    }
}
