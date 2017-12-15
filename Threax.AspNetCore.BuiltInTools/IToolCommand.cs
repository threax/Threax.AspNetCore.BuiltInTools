using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Threax.AspNetCore.BuiltInTools
{
    public interface IToolCommand
    {
        /// <summary>
        /// Run the command.
        /// </summary>
        /// <param name="args"></param>
        void Execute(ToolArgs args);

        /// <summary>
        /// Get help text for this command.
        /// </summary>
        String Help { get; }
    }
}
