// See https://aka.ms/new-console-template for more information

using Serilog;
using Squiddy.Gui.Bootstrap;

var bootstrapper = new SquiddyBootstrap(new LoggerConfiguration().MinimumLevel.Debug());

await bootstrapper.RunHostAsync(args);
