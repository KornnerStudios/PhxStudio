using System;
using System.ComponentModel.Composition;
using System.Windows.Input;
using Gemini.Framework.Commands;

namespace PhxStudio.Modules.Main.Commands
{
	[CommandDefinition]
	public sealed class PhxOpenFileCommandDefinition
		: CommandDefinition
	{
		public const string CommandName = "File.OpenFile";

		public override string Name
		{
			get { return CommandName; }
		}

		public override string Text
		{
			get { return "_Open"; }
		}

		public override string ToolTip
		{
			get { return "Open File"; }
		}

		public override Uri IconSource
		{
			get { return new Uri("pack://application:,,,/Gemini;component/Resources/Icons/Open.png"); }
		}

		[Export]
		public static CommandKeyboardShortcut KeyGesture = new CommandKeyboardShortcut<PhxOpenFileCommandDefinition>(
			new KeyGesture(Key.O, ModifierKeys.Control));
	}
}