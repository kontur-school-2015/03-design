using System;

namespace DIContainer.Commands
{
	class HelpCommand : BaseCommand
	{
		private readonly Lazy<ICommand[]> commands;

		public HelpCommand(Lazy<ICommand[]> commands)
		{
			this.commands = commands;
		}

		public override void Execute()
		{
			foreach (var command in commands.Value)
			{
				Console.WriteLine(command.Name);
			}
		}
	}
}