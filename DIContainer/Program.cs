using System;
using System.IO;
using System.Linq;
using DIContainer.Commands;
using Ninject;

namespace DIContainer
{
    public class Program
    {
        private readonly CommandLineArgs arguments;
	    private readonly TextWriter writer;
	    private readonly ICommand[] commands;

        public Program(CommandLineArgs arguments, TextWriter writer, params ICommand[] commands)
        {
            this.arguments = arguments;
	        this.writer = writer;
	        this.commands = commands;
        
        }

		static void Main(string[] args)
		{
			var container = new StandardKernel();
			// Вариант 1:
			//container.Bind<CommandLineArgs>().To<CommandLineArgs>().WithConstructorArgument(args);
			// Вариант 2:
//			container.Bind<CommandLineArgs>().ToSelf().WithConstructorArgument(args);
			// Вариант 3:
			container.Bind<CommandLineArgs>().ToConstant(new CommandLineArgs(args));
			container.Bind<ICommand>().To<TimerCommand>();
			container.Bind<ICommand>().To<PrintTimeCommand>();
			container.Bind<ICommand>().To<HelpCommand>();
			container.Bind<TextWriter>().ToConstant(Console.Out); //Property Injection in BaseCommand

			container.Get<Program>().Run();
		}
		
        public void Run()
        {
            if (arguments.Command == null)
            {
                writer.WriteLine("Please specify <command> as the first command line argument");
                return;
            }
            var command = commands.FirstOrDefault(c => c.Name.Equals(arguments.Command, StringComparison.InvariantCultureIgnoreCase));
            if (command == null)
				writer.WriteLine("Sorry. Unknown command {0}", arguments.Command);
            else
                command.Execute();
        }
    }
}
