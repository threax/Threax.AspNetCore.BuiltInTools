using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Threax.AspNetCore.BuiltInTools
{
    public interface IToolRunner
    {
        /// <summary>
        /// Add a tool to this tool runner. The name will be the name of the command.
        /// </summary>
        /// <param name="name">The name of the command.</param>
        /// <param name="command">The tool to run.</param>
        IToolRunner AddTool(String name, IToolCommand command);

        /// <summary>
        /// Run a tool, returns true if a tool was run, false if it was not and the website should be run.
        /// </summary>
        /// <param name="args">The tool args to run</param>
        /// <returns></returns>
        bool RunTool(ToolArgs args);

        /// <summary>
        /// The help messages for the tools.
        /// </summary>
        IEnumerable<String> HelpMessages { get; }

        /// <summary>
        /// Run the passed method after all tools have run.
        /// </summary>
        /// <param name="cb"></param>
        /// <returns></returns>
        IToolRunner AfterTools(Action<ToolArgs> cb);

        /// <summary>
        /// Run all the after tools events.
        /// </summary>
        void RunAfterTools(ToolArgs args);
    }
}
