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

        /// <summary>
        /// Constructor, takes the command line args.
        /// </summary>
        /// <param name="args">The command line args.</param>
        public ToolManager(String[] args)
        {
            this.args = args;
        }

        /// <summary>
        /// Get the environment to use, will return null if no special environment should be set.
        /// </summary>
        /// <returns></returns>
        public String GetEnvironment()
        {
            if(args.Length > 0 && args[0] == "tools")
            {
                return "tools";
            }
            return null;
        }

        /// <summary>
        /// Run tools if specified by args, will return true if the website should be run, false if tools were run.
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        public bool ProcessTools(IWebHost host)
        {
            bool normalRun = true;

            if (args.Length > 0)
            {
                if (args[0] == "tools")
                {
                    normalRun = false;

                    using (var scope = host.Services.GetService<IServiceScopeFactory>().CreateScope())
                    {
                        var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>();
                        var log = logger.CreateLogger<ToolRunner>();

                        var runner = scope.ServiceProvider.GetService<IToolRunner>();

                        if (runner != null)
                        {
                            if (args.Length > 1)
                            {
                                foreach (var tool in args.Skip(1))
                                {
                                    log.LogInformation($"Running tool {tool}.");
                                    var run = runner.RunTool(new ToolArgs(tool)
                                    {
                                        Host = host,
                                        Scope = scope,
                                        Log = log
                                    });
                                    if (!run)
                                    {
                                        log.LogError($"Could not find tool {tool}.");
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
            }

            return normalRun;
        }
    }
}
