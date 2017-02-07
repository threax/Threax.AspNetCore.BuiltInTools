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
        /// <summary>
        /// Run tools if specified by args, will return true if the website should be run, false if tools were run.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="host"></param>
        /// <returns></returns>
        public static bool ProcessTools(String[] args, IWebHost host)
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

                        if(runner != null)
                        {
                            if (args.Length > 1)
                            {
                                foreach(var tool in args.Skip(1))
                                {
                                    log.LogInformation($"Running tool {tool}.");
                                    var run = runner.RunTool(args[1], new ToolArgs()
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
