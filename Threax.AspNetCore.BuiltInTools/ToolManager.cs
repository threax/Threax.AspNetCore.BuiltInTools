using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Threax.AspNetCore.BuiltInTools
{
    /// <summary>
    /// This class is used to run tools.
    /// </summary>
    public class ToolManager
    {
        private String[] args;
        private int toolsIndex;
        public bool HasToolArg => toolsIndex < args.Length;

        /// <summary>
        /// Constructor, takes the command line args.
        /// </summary>
        /// <param name="args">The command line args.</param>
        public ToolManager(String[] args)
        {
            this.args = args;
            for (toolsIndex = 0; toolsIndex < args.Length && args[toolsIndex] != "tools"; ++toolsIndex) ;
        }

        /// <summary>
        /// Get the environment to use, will return null if no special environment should be set.
        /// </summary>
        /// <returns></returns>
        public String GetEnvironment()
        {
            if (HasToolArg)
            {
                return "tools";
            }
            return null;
        }

        /// <summary>
        /// Get args without the tools, so they can be passed onward without interfering.
        /// </summary>
        /// <returns></returns>
        public String[] GetCleanArgs()
        {
            return args.Take(toolsIndex).ToArray();
        }

        /// <summary>
        /// Run tools if specified by args, will return true if the website should be run, false if tools were run.
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        public bool ProcessTools(IWebHost host)
        {
            bool normalRun = true;

            if (HasToolArg)
            {
                normalRun = false;

                using (var scope = host.Services.GetService<IServiceScopeFactory>().CreateScope())
                {
                    var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>();
                    var log = logger.CreateLogger<ToolRunner>();

                    var runner = scope.ServiceProvider.GetService<IToolRunner>();

                    if (runner != null)
                    {
                        if (args.Length > toolsIndex + 1)
                        {
                            foreach (var tool in args.Skip(toolsIndex + 1))
                            {
                                using (var toolScope = host.Services.GetService<IServiceScopeFactory>().CreateScope())
                                {
                                    log.LogInformation($"Running tool {tool}.");
                                    var run = runner.RunTool(new ToolArgs(tool)
                                    {
                                        Host = host,
                                        Scope = toolScope,
                                        Log = log
                                    });
                                    if (!run)
                                    {
                                        log.LogError($"Could not find tool {tool}.");
                                    }
                                }
                            }
                        }
                        else
                        {
                            log.LogInformation("Supported tools:");
                            foreach (var help in runner.HelpMessages)
                            {
                                log.LogInformation(help);
                            }
                            log.LogInformation("Multiple tools can be run at once by specifying them on the command line.");
                        }

                        runner.RunAfterTools(new ToolArgs()
                        {
                            Host = host,
                            Scope = scope,
                            Log = log
                        });
                    }
                    else
                    {
                        log.LogError("Cannot find IToolRunner, did you forget to define one in services?");
                    }
                }
            }

            return normalRun;
        }
    }
}
