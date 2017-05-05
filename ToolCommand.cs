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
        public Func<ToolArgs, Task> action;

        public ToolCommand(Func<ToolArgs, Task> action)
        {
            this.action = action;
        }

        public ToolCommand(String help, Func<ToolArgs, Task> action)
        {
            this.action = action;
            this.Help = help;
        }

        public string Help { get; set; }

        public void Execute(ToolArgs args)
        {
            var t = action(args);
            t.Wait();
        }
    }
}
