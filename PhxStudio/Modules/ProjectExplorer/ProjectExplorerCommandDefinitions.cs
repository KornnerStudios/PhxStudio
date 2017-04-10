using System;
using Gemini.Framework.Commands;

namespace PhxStudio.Modules.ProjectExplorer.Commands
{
	[CommandDefinition]
	sealed class ViewProjectExplorerDefinition
		: CommandDefinition
	{
		public const string CommandName = "View.ProjectExplorer";

		public override string Name { get { return CommandName; } }

		public override string Text { get {
			return "Project Explorer";
		} }

		public override string ToolTip { get {
			return "Open Project Explorer";
		} }

		public override Uri IconSource { get { return new Uri(
				"pack://application:,,,/Gemini;component/Resources/Icons/Open.png"
			);
		} }
	}
}